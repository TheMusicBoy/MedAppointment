using Domain.IRepository;
using Rep = Domain.Schedule.IRepository;
using DRep = Domain.Doctor.IRepository;
using Model = Domain.Schedule.Models;

namespace Domain.Schedule.UseCases {
    public class ScheduleUseCases {
        private Rep.IScheduleRepository ScheduleRepository;
        private DRep.IDoctorRepository DoctorRepository;

        ScheduleUseCases(Rep.IScheduleRepository scheduleRepository, DRep.IDoctorRepository doctorRepository) {
            ScheduleRepository = scheduleRepository;
            DoctorRepository = doctorRepository;
        }

        Result<Model.Schedule> GetScheduleBy(int doctor_id, DateOnly date) {
            if (!DoctorRepository.Exists(doctor_id))
                return Result.Fail<Model.Schedule>("Doctor doesn't exists");

            return ScheduleRepository.GetByDocTime(doctor_id, date);
        }
    }
}