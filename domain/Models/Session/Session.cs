using System;
using Domain.Logic;

namespace Domain.Session
{
    public class Session
    {
        public int Id { get; }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public Session (int patient, int doctor, DateTime begin, DateTime end)
        {
            Id = IdCreator.getInstance().NextId();

            Begin = begin;
            End = end;

            PatientId = patient;
            DoctorId = doctor;
        }

        public Session (Session other)
        {
            Id = other.Id;

            Begin = other.Begin;
            End = other.End;

            PatientId = other.PatientId;
            DoctorId = other.DoctorId;
        }

    }
}