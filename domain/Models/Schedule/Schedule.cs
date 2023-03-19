using System;
using Domain.Logic;

namespace Domain.Schedule.Models
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
    }
}