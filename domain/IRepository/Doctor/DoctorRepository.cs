using Domain.IRepository;
using Model = Domain.Doctor;

namespace Domain.Doctor
{
    public interface IDoctorRepository : IRepository<Doctor> {
        IEnumerable<Doctor> GetBySpec(int specialization);

        IEnumerable<Doctor> GetAll();

    }
}