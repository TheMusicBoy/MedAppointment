using Domain.IRepository;
using Model = Domain.Doctor.Models;

namespace Domain.Doctor.IRepository
{
    public interface IDoctorRepository : IRepository<Model.Doctor> {
        IEnumerable<Model.Doctor> GetBySpec(int specialization);

        IEnumerable<Model.Doctor> GetAll();

    }
}