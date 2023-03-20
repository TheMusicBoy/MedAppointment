using Domain.IRepository;
using Sc = Domain.Schedule;
using Sp = Domain.Specialization;
using D = Domain.Doctor;
using U = Domain.User;

namespace Domain.Session
{
    public class SessionUseCases
    {
        ISessionRepository SessionRepository;
        Sc.IScheduleRepository ScheduleRepository;
        Sp.ISpecializationRepository SpecializationRepository;
        D.IDoctorRepository DoctorRepository;
        U.IUserRepository UserRepository;

        public SessionUseCases(ISessionRepository sessionRepository, Sc.IScheduleRepository scheduleRepository, Sp.ISpecializationRepository specializationRepository, D.IDoctorRepository doctorRepository, U.IUserRepository userRepository)
        {
            SessionRepository = sessionRepository;
            ScheduleRepository = scheduleRepository;
            SpecializationRepository = specializationRepository;
            DoctorRepository = doctorRepository;
            UserRepository = userRepository;
        }

        public Result<Session> FreeTime(int doctor_id, int duration, DateOnly date)
        {
            if (!DoctorRepository.Exists(doctor_id))
                return Result.Fail<Session>("Doctor doesn't exists");

            Result<Sc.Schedule> schedule_result = ScheduleRepository.GetByDocTime(doctor_id, date);
            if (schedule_result.Failure)
                return Result.Fail<Session>("Doctor has no schedule this date");

            Sc.Schedule schedule = schedule_result.Value;
            Result<Session> last_result = SessionRepository.GetLast(doctor_id, date);

            Session session;

            if (last_result.Failure)
            {
                session = new Session(0, doctor_id, schedule.Begin, schedule.Begin.AddMinutes(duration));
            }
            else
            {
                Session last = last_result.Value;
                session = new Session(0, doctor_id, last.End, last.End.AddMinutes(duration));
            }

            if (schedule.End < session.End)
                return Result.Fail<Session>("Doctor hasn't enough time");

            return Result.Ok<Session>(session);

        }

        public Result<Session> CreateSession(int patient_id, int doctor_id, int duration, DateOnly date)
        {
            if (!UserRepository.Exists(patient_id))
                return Result.Fail<Session>("Patient doesn't exists");

            Result<Session> result = FreeTime(doctor_id, duration, date);

            if (result.Failure)
                return result;

            result.Value.PatientId = patient_id;
            SessionRepository.Create(result.Value);
            return result;
        }

        public Result<Session> CreateSession(int patient_id, int duration, DateOnly date)
        {
            if (!UserRepository.Exists(patient_id))
                return Result.Fail<Session>("Patient doesn't exists");

            List<D.Doctor> doctors = DoctorRepository.GetAll().ToList<D.Doctor>();

            foreach (var doctor in doctors)
            {
                Result<Session> result = CreateSession(patient_id, doctor.Id, duration, date);
                if (result.Success)
                    return result;
            }

            return Result.Fail<Session>("There are no doctor free this date");
        }

        public Result<List<Session>> GetFreeDateSince(int spec_id, int duration, DateOnly date)
        {
            if (!SpecializationRepository.Exists(spec_id))
                return Result.Fail<List<Session>>("Specialization doesn't exists");

            List<D.Doctor> doctors = DoctorRepository.GetBySpec(spec_id).ToList<D.Doctor>();
            List<Session> result = new List<Session>();

            foreach (var doctor in doctors)
            {
                DateOnly res_date = date;
                while (true)
                {
                    Result<Session> session = FreeTime(doctor.Id, duration, res_date);
                    if (session.Success) {
                        result.Add(session.Value);
                        break;
                    } else {
                        res_date.AddDays(1);
                    }
                }
            }

            return Result.Ok<List<Session>>(result);
        }
    }
}
