using Domain.IRepository;
using Model = Domain.Schedule.Models;

namespace Domain.Schedule.IRepository
{
    public interface IScheduleRepository : IRepository<Model.Schedule> {
        Result<Model.Schedule> GetByDocTime(int doctorId, DateOnly date);
        
    }
}