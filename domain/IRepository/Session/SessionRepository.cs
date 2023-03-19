using Domain.IRepository;
using Model = Domain.Session.Models;

namespace Domain.Session.IRepository
{
    public interface ISessionRepository : IRepository<Model.Session> {
        IEnumerable<Model.Session> GetBy(int doctor_id, DateTime begin, DateTime end);

        Result<Model.Session> GetLast(int doctor_id, DateOnly date);

    }
}