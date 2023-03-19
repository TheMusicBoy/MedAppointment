using Domain.IRepository;
using DRep = Domain.Doctor.IRepository;
using Model = Domain.Doctor.Models;
using SRep = Domain.Specialization.IRepository;

namespace Domain.Doctor.UseCases
{
    public class DoctorUseCases
    {
        private DRep.IDoctorRepository DoctorRepository;
        private SRep.ISpecializationRepository SpecializationRepository;

        public DoctorUseCases(DRep.IDoctorRepository doctorRepository, SRep.ISpecializationRepository specializationRepository)
        {
            DoctorRepository = doctorRepository;
            SpecializationRepository = specializationRepository;
        }

        Result CreateDoctor(Model.Doctor doctor)
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

        Result DeleteDoctor(int doctor_id)
        {
            if (!DoctorRepository.Exists(doctor_id))
                return Result.Fail("This doctor doesn't exists");

            DoctorRepository.Delete(doctor_id);
            return Result.Ok();
        }

        Result<List<Model.Doctor>> GetAllDoctors()
        {
            var result = DoctorRepository.GetAll();
            List<Model.Doctor> result_list = result.ToList<Model.Doctor>();
            return Result.Ok<List<Model.Doctor>>(result_list);
        }

        Result<Model.Doctor> GetDoctorById(int doctor_id)
        {
            if (!DoctorRepository.Equals(doctor_id))
                return Result.Fail<Model.Doctor>("Doctor doesn't exists");

            return Result.Ok<Model.Doctor>(DoctorRepository.Get(doctor_id));
        }

        Result<List<Model.Doctor>> GetDoctorBySpec(int spec)
        {
            if (!SpecializationRepository.Exists(spec))
                return Result.Fail<List<Model.Doctor>>("Specialization doesn't exists");

            return Result.Ok<List<Model.Doctor>>(DoctorRepository.GetBySpec(spec).ToList<Model.Doctor>());
        }

    }
}