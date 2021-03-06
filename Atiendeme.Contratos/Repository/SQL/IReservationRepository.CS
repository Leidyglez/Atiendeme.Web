﻿using Atiendeme.Entidades.Entidades.SQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atiendeme.Contratos.Repository.SQL
{
    public interface IReservationRepository
    {
        Task<List<Reservations>> GetReservationsAsync();

        Task<List<Reservations>> GetReservationsAsync(DateTime day);

        Task<Reservations> GetReservationAsync(int reservationId);

        Task<List<Reservations>> GetReservationsFromDoctor(string doctorId, int officeId, DateTime day);

        Task<List<Reservations>> GetReservationFromUserAsync(string userId);

        Task<List<Reservations>> GetReservationFromUserAsync(string userId, string state);

        Task<List<Reservations>> GetReservationFromDoctorAsync(string doctorId);

        Task<Reservations> SaveReservation(Reservations reservation);

        Task<List<Reservations>> GetReservationFromDoctorsAsync(List<string> doctorsId);

        bool DeleteReservation(Reservations reservation);

        Reservations ChangeReserveStatus(Reservations changeReserveStatus);
    }
}