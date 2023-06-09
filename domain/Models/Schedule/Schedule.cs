using System;
using Domain.Logic;

namespace Domain.Schedule
{
    public class Schedule
    {
        public int Id { get; }

        public int DoctorId { get; set; }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public Schedule(int doctorId, DateTime begin, DateTime end)
        {
            Id = IdCreator.getInstance().NextId();

            DoctorId = doctorId;

            Begin = begin;
            End = end;
        }

        public Schedule(Schedule other)
        {
            Id = other.Id;

            DoctorId = other.DoctorId;

            Begin = other.Begin;
            End = other.End;
        }
    }
}