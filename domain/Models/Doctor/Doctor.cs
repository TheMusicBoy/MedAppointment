using Domain.Logic;
using Domain.Models.Specialization;

namespace Domain.Models.Doctor
{
    public class Doctor
    {
        public int Id { get; }
        public string FullName { get; set; }
        public Specialization.Specialization Specialization { get; set; }

        Doctor(string fullName, Specialization.Specialization specialization) {
            Id = IdCreator.getInstance().NextId();

            FullName = fullName;
            Specialization = specialization;
        }
    }
}