using Domain.Logic;

namespace Domain.Specialization.Models
{
    public class Specialization {
        public int Id { get; }
        public string Name { get; set; }

        public Specialization(string name) {
            Id = IdCreator.getInstance().NextId();
            Name = name;
        }
    }
}