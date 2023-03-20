using Domain.IRepository;
using D = Domain.Doctor;
using S = Domain.Specialization;

namespace Domain.Doctor
{
    public class DoctorUseCases
    {
        private IDoctorRepository DoctorRepository;
        private S.ISpecializationRepository SpecializationRepository;

        public DoctorUseCases(IDoctorRepository doctorRepository, S.ISpecializationRepository specializationRepository)
        {
            DoctorRepository = doctorRepository;
            SpecializationRepository = specializationRepository;
        }

        public Result CreateDoctor(Doctor doctor)
        {
            if (DoctorRepository.Exists(doctor.Id))
                return Result.Fail("This doctor already exists");

            if (doctor.Specialization == null || doctor.FullName == string.Empty)
                return Result.Fail("Full name or specialization not specified");

            if (!SpecializationRepository.Exists(doctor.Specialization.Id))
                return Result.Fail("Specialization doesn't exists");

            DoctorRepository.Create(doctor);
            return Result.Ok();
        }

        public Result DeleteDoctor(int doctor_id)
        {
            if (!DoctorRepository.Exists(doctor_id))
                return Result.Fail("This doctor doesn't exists");

            DoctorRepository.Delete(doctor_id);
            return Result.Ok();
        }

        public Result<List<Doctor>> GetAllDoctors()
        {
            var result = DoctorRepository.GetAll();
            List<Doctor> result_list = result.ToList<Doctor>();
            return Result.Ok<List<Doctor>>(result_list);
        }

        public Result<Doctor> GetDoctorById(int doctor_id)
        {
            if (!DoctorRepository.Equals(doctor_id))
                return Result.Fail<Doctor>("Doctor doesn't exists");

            return Result.Ok<Doctor>(DoctorRepository.Get(doctor_id));
        }

        public Result<List<Doctor>> GetDoctorBySpec(int spec)
        {
            if (!SpecializationRepository.Exists(spec))
                return Result.Fail<List<Doctor>>("Specialization doesn't exists");

            return Result.Ok<List<Doctor>>(DoctorRepository.GetBySpec(spec).ToList<Doctor>());
        }

    }
}