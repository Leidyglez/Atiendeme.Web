﻿using Atiendeme.Contratos.Repository.SQL;

namespace Atiendeme.Contratos.Repository
{
    public interface IAtiendemeUnitOfWork
    {
        IDoctorRepository DoctorRepository { get; }

        ISpecialtiesRepository SpecialtiesRepository { get; }

        IOfficeRepository OfficeRepository { get; }

        IReservationRepository ReservationRepository { get; }

        IUserRepository UserRepository { get; }
    }
}