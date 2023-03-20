using Domain.Logic;
using Sp = Domain.Specialization;

namespace Domain.Doctor
{
    public class Doctor
    {
        public int Id { get; }
        public string FullName { get; set; }
        public Sp.Specialization Specialization { get; set; }

        public Doctor(string fullName, Sp.Specialization specialization)
        {
            Id = IdCreator.getInstance().NextId();

            FullName = fullName;
            Specialization = specialization;
        }

        public Doctor(Doctor other)
        {
            Id = other.Id;

            FullName = other.FullName;
            Specialization = other.Specialization;
        }
    }
}