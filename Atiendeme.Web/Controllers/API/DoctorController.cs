﻿using Atiendeme.Contratos.Repository;
using Atiendeme.Entidades.Entidades.Dtos;
using Atiendeme.Entidades.Entidades.SQL;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Atiendeme.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;

        private readonly IAtiendemeUnitOfWork _atiendemeUnitOfWork;

        public readonly IMapper _mapper;

        public DoctorController(ILogger<DoctorController> logger, UserManager<ApplicationUser> userManager,
            IAtiendemeUnitOfWork atiendemeUnitOfWork, IMapper mapper)
        {
            _logger = logger;
            _atiendemeUnitOfWork = atiendemeUnitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<DoctorDto>>> Doctor()
        {
            var result = await _atiendemeUnitOfWork.DoctorRepository.GetDoctorsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Se debe de agregar lo de los rangos de fecha, especialidades y centros
        /// </summary>
        /// <param name="medico"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> Doctor(DoctorDto medico)
        {
            //var _medicoMapper = _mapper.Map<ApplicationUser>(medico);
            //_medicoMapper.UserName = medico.Email;
            //Crea al medico

            var user = new ApplicationUser
            {
                UserName = medico.Email,
                Email = medico.Email,
                LastName = medico.LastName,
                Name = medico.Name,
                Birthday = medico.Birthday,
                Genre = medico.Genre,
                PhoneNumber = medico.PhoneNumber,
            };

            var doctor = await _atiendemeUnitOfWork.DoctorRepository.SaveDoctorAsync(user, medico.Password);

            //Add validation
            if (doctor == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            var _doctorLaborDays = _mapper.Map<List<DoctorLaborDays>>(medico.DoctorLaborDays);

            List<OfficesDoctors> officesDoctors = new List<OfficesDoctors>();

            _doctorLaborDays.ForEach(_doctorLaborDay =>
            {
                _doctorLaborDay.DoctorId = doctor.Id;
                _doctorLaborDay.Office = null;

                if (!officesDoctors.Any(x => x.OfficeId == _doctorLaborDay.OfficeId))
                    officesDoctors.Add(new OfficesDoctors()
                    {
                        DoctorId = doctor.Id,
                        OfficeId = _doctorLaborDay.OfficeId
                    });
            });

            var resultSaveDoctorDays = await _atiendemeUnitOfWork.DoctorRepository.SaveDoctorLaborDays(_doctorLaborDays);

            var resultSaveDoctorOffices = await _atiendemeUnitOfWork.OfficeRepository.SaveOfficesDoctor(officesDoctors.ToArray());

            //Agrega los horarios del medico
            DoctorDto mapperResult = _mapper.Map<DoctorDto>(doctor);
            mapperResult.DoctorLaborDays = _mapper.Map<List<DoctorLaborDaysDto>>(resultSaveDoctorDays);

            mapperResult.Offices = _mapper.Map<List<OfficeDto>>(resultSaveDoctorOffices.ToList());

            return StatusCode((int)HttpStatusCode.Created, doctor);
        }

        [HttpDelete("{doctorId}")]
        public async Task<ActionResult<DoctorLaborDaysDto>> Doctor(string doctorId)
        {
            var search = await _atiendemeUnitOfWork.UserRepository.GetUserEntity(doctorId);

            if (search == null)
                return NotFound();

            var result = await _atiendemeUnitOfWork.DoctorRepository.RemoveDoctor(search);

            if (result == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<DoctorLaborDaysDto>> DoctorLaborDay(List<DoctorLaborDaysDto> doctorLaborDays)
        {
            foreach (var item in doctorLaborDays)
            {
                var doctor = await _atiendemeUnitOfWork.DoctorRepository.GetDoctorAsync(item.DoctorId);

                if (doctor == null)
                    return BadRequest(("Doctor with Id {0} not found", item.DoctorId));
            }

            var _doctorLaborDays = _mapper.Map<List<DoctorLaborDays>>(doctorLaborDays);
            var resultSaveDoctorDays = await _atiendemeUnitOfWork.DoctorRepository.SaveDoctorLaborDays(_doctorLaborDays);

            var resultDto = _mapper.Map<List<DoctorLaborDaysDto>>(resultSaveDoctorDays);

            return StatusCode((int)HttpStatusCode.Created, resultDto);
        }
    }
}