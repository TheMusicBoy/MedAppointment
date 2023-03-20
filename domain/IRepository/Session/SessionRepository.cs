using Domain.IRepository;
using Model = Domain.Session;

namespace Domain.Session
{
    public interface ISessionRepository : IRepository<Session> {
        IEnumerable<Model.Session> GetBy(int doctor_id, DateTime begin, DateTime end);

        Result<Model.Session> GetLast(int doctor_id, DateOnly date);

    }
}