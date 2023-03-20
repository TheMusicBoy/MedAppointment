using Domain.IRepository;
using Rep = Domain.Session.IRepository;
using SRep = Domain.Schedule.IRepository;
using SpRep = Domain.Specialization.IRepository;
using DRep = Domain.Doctor.IRepository;
using DModel = Domain.Doctor.Models;
using SModel = Domain.Schedule.Models;
using Model = Domain.Session.Models;
using URep = Domain.User.IRepository;

namespace Domain.Session.UseCases
{
    public class SessionUseCases
    {
        Rep.ISessionRepository SessionRepository;
        SRep.IScheduleRepository ScheduleRepository;
        SpRep.ISpecializationRepository SpecializationRepository;
        DRep.IDoctorRepository DoctorRepository;
        URep.IUserRepository UserRepository;

        SessionUseCases(Rep.ISessionRepository sessionRepository, SRep.IScheduleRepository scheduleRepository, SpRep.ISpecializationRepository specializationRepository, DRep.IDoctorRepository doctorRepository, URep.IUserRepository userRepository)
        {
            SessionRepository = sessionRepository;
            ScheduleRepository = scheduleRepository;
            SpecializationRepository = specializationRepository;
            DoctorRepository = doctorRepository;
            UserRepository = userRepository;
        }

        Result<Model.Session> FreeTime(int doctor_id, int duration, DateOnly date)
        {
            if (!DoctorRepository.Exists(doctor_id))
                return Result.Fail<Model.Session>("Doctor doesn't exists");

            Result<SModel.Schedule> schedule_result = ScheduleRepository.GetByDocTime(doctor_id, date);
            if (schedule_result.Failure)
                return Result.Fail<Model.Session>("Doctor has no schedule this date");

            SModel.Schedule schedule = schedule_result.Value;
            Result<Model.Session> last_result = SessionRepository.GetLast(doctor_id, date);

            Model.Session session;

            if (last_result.Failure)
            {
                session = new Model.Session(0, doctor_id, schedule.Begin, schedule.Begin.AddMinutes(duration));
            }
            else
            {
                Model.Session last = last_result.Value;
                session = new Model.Session(0, doctor_id, last.End, last.End.AddMinutes(duration));
            }

            if (schedule.End < session.End)
                return Result.Fail<Model.Session>("Doctor hasn't enough time");

            return Result.Ok<Model.Session>(session);

        }

        Result<Model.Session> CreateSession(int patient_id, int doctor_id, int duration, DateOnly date)
        {
            if (!UserRepository.Exists(patient_id))
                return Result.Fail<Model.Session>("Patient doesn't exists");

            Result<Model.Session> result = FreeTime(doctor_id, duration, date);

            if (result.Failure)
                return result;

            result.Value.PatientId = patient_id;
            SessionRepository.Create(result.Value);
            return result;
        }

        Result<Model.Session> CreateSession(int patient_id, int duration, DateOnly date)
        {
            if (!UserRepository.Exists(patient_id))
                return Result.Fail<Model.Session>("Patient doesn't exists");

            List<DModel.Doctor> doctors = DoctorRepository.GetAll().ToList<DModel.Doctor>();

            foreach (var doctor in doctors)
            {
                Result<Model.Session> result = CreateSession(patient_id, doctor.Id, duration, date);
                if (result.Success)
                    return result;
            }

            return Result.Fail<Model.Session>("There are no doctor free this date");
        }

        Result<List<Model.Session>> GetFreeDateSince(int spec_id, int duration, DateOnly date)
        {
            if (!SpecializationRepository.Exists(spec_id))
                return Result.Fail<List<Model.Session>>("Specialization doesn't exists");

            List<DModel.Doctor> doctors = DoctorRepository.GetBySpec(spec_id).ToList<DModel.Doctor>();
            List<Model.Session> result = new List<Model.Session>();

            foreach (var doctor in doctors)
            {
                DateOnly res_date = date;
                while (true)
                {
                    Result<Model.Session> session = FreeTime(doctor.Id, duration, res_date);
                    if (session.Success) {
                        result.Add(session.Value);
                        break;
                    } else {
                        res_date.AddDays(1);
                    }
                }
            }

            return Result.Ok<List<Model.Session>>(result);
        }
    }
}
