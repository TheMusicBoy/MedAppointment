using Moq;
using X = Xunit;

using System;
using System.Collections.Generic;

using S = Domain.Session;
using R = Domain.Role;
using Sc = Domain.Schedule;
using Sp = Domain.Specialization;
using D = Domain.Doctor;
using U = Domain.User;

namespace Test.DoctorUseCases
{
    [TestClass]
    public class DoctorUseCasesTest
    {

        private Mock<S.ISessionRepository> SessionRepository;
        private Mock<Sc.IScheduleRepository> ScheduleRepository;
        private Mock<Sp.ISpecializationRepository> SpecializationRepository;
        private Mock<D.IDoctorRepository> DoctorRepository;
        private Mock<U.IUserRepository> UserRepository;
        private D.DoctorUseCases UseCases;

        DoctorUseCasesTest()
        {
            SessionRepository = new Mock<S.ISessionRepository>();
            ScheduleRepository = new Mock<Sc.IScheduleRepository>();
            SpecializationRepository = new Mock<Sp.ISpecializationRepository>();
            DoctorRepository = new Mock<D.IDoctorRepository>();
            UserRepository = new Mock<U.IUserRepository>();
            UseCases = new D.DoctorUseCases(DoctorRepository.Object, SpecializationRepository.Object);
        }

        [X.Fact]
        public void CreateDoctorByExists_Fail() {
            Sp.Specialization spec = new Sp.Specialization("");
            D.Doctor doctor = new D.Doctor(".", spec);

            DoctorRepository.Setup(rep => rep.Exists(doctor.Id)).Returns(true);

            var result = UseCases.CreateDoctor(doctor);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "This doctor already exists");
        }

        [X.Fact]
        public void CreateDoctorByEmpty_Fail() {
            Sp.Specialization spec = null;
            D.Doctor doctor = new D.Doctor("", spec);

            DoctorRepository.Setup(rep => rep.Exists(doctor.Id)).Returns(false);

            var result = UseCases.CreateDoctor(doctor);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Full name or specialization not specified");
        }

        [X.Fact]
        public void CreateDoctorBySpecNotExists_Fail() {
            Sp.Specialization spec = new Sp.Specialization("");
            D.Doctor doctor = new D.Doctor(".", spec);

            DoctorRepository.Setup(rep => rep.Exists(doctor.Id)).Returns(false);
            SpecializationRepository.Setup(rep => rep.Exists(doctor.Specialization.Id)).Returns(false);

            var result = UseCases.CreateDoctor(doctor);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Specialization doesn't exists");
        }

        [X.Fact]
        public void CreateDoctorForManyBySpecNotExists_Fail() {
            Sp.Specialization spec = new Sp.Specialization("");
            D.Doctor doctor = new D.Doctor(".", spec);

            DoctorRepository.Setup(rep => rep.Exists(doctor.Id)).Returns(false);
            SpecializationRepository.Setup(rep => rep.Exists(doctor.Specialization.Id)).Returns(true);

            var result = UseCases.CreateDoctor(doctor);

            X.Assert.True(result.Success);
        }

        [X.Fact]
        public void DeleteDoctorByDoctorNotExists_Fail() {
            int doctor_id = 1;

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(false);

            var result = UseCases.DeleteDoctor(doctor_id);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "This doctor doesn't exists");
        }

        [X.Fact]
        public void DeleteDoctor_Ok() {
            int doctor_id = 1;

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(true);

            var result = UseCases.DeleteDoctor(doctor_id);

            X.Assert.True(result.Success);
        }

        [X.Fact]
        public void GetAllDoctors_Ok() {
            DoctorRepository.Setup(rep => rep.GetAll()).Returns(new List<D.Doctor>());

            var result = UseCases.GetAllDoctors();

            X.Assert.True(result.Success);
            Assert.Equals(result.Value, new List<D.Doctor>());
        }
    }
}