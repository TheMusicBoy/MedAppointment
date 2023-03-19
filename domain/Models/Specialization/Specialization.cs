using Domain.Logic;

namespace Domain.Models.Specialization
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