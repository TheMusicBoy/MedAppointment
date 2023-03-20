using Domain.IRepository;
using Domain.Schedule;
using D = Domain.Doctor;

namespace Domain.Schedule {
    public class ScheduleUseCases {
        private IScheduleRepository ScheduleRepository;
        private D.IDoctorRepository DoctorRepository;

        public ScheduleUseCases(IScheduleRepository scheduleRepository, D.IDoctorRepository doctorRepository) {
            ScheduleRepository = scheduleRepository;
            DoctorRepository = doctorRepository;
        }

        public Result<Schedule> GetScheduleBy(int doctor_id, DateOnly date) {
            if (!DoctorRepository.Exists(doctor_id))
                return Result.Fail<Schedule>("Doctor doesn't exists");

            return ScheduleRepository.GetByDocTime(doctor_id, date);
        }
    }
}