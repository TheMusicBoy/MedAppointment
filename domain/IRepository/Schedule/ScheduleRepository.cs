using Domain.IRepository;
using Model = Domain.Schedule;

namespace Domain.Schedule
{
    public interface IScheduleRepository : IRepository<Schedule> {
        Result<Schedule> GetByDocTime(int doctorId, DateOnly date);
        
    }
}