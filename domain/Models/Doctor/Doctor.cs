using Domain.Logic;
using Sp = Domain.Specialization.Models;

namespace Domain.Doctor.Models
{
    public class Doctor
    {
        public int Id { get; }
        public string FullName { get; set; }
        public Sp.Specialization Specialization { get; set; }

        Doctor(string fullName, Sp.Specialization specialization) {
            Id = IdCreator.getInstance().NextId();

            FullName = fullName;
            Specialization = specialization;
        }
    }
}