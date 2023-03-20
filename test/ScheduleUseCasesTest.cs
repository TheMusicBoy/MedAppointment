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
using Domain.IRepository;

namespace Test.ScheduleUseCases
{
    [TestClass]
    public class ScheduleUseCasesTest
    {

        private Mock<S.ISessionRepository> SessionRepository;
        private Mock<Sc.IScheduleRepository> ScheduleRepository;
        private Mock<Sp.ISpecializationRepository> SpecializationRepository;
        private Mock<D.IDoctorRepository> DoctorRepository;
        private Mock<U.IUserRepository> UserRepository;
        private Sc.ScheduleUseCases UseCases;

        ScheduleUseCasesTest()
        {
            SessionRepository = new Mock<S.ISessionRepository>();
            ScheduleRepository = new Mock<Sc.IScheduleRepository>();
            SpecializationRepository = new Mock<Sp.ISpecializationRepository>();
            DoctorRepository = new Mock<D.IDoctorRepository>();
            UserRepository = new Mock<U.IUserRepository>();
            UseCases = new Sc.ScheduleUseCases(ScheduleRepository.Object, DoctorRepository.Object);
        }

        [X.Fact]
        public void GetScheduleByDoctorNotExists_Fail()
        {
            int doctor_id = 1;
            DateOnly date = new DateOnly();

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(false);

            var result = UseCases.GetScheduleBy(doctor_id, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Doctor doesn't exists");
        }

        [X.Fact]
        public void GetScheduleBy_Ok()
        {
            int doctor_id = 1;
            DateOnly date = new DateOnly();

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(true);

            var result = UseCases.GetScheduleBy(doctor_id, date);

            X.Assert.True(result.Success == ScheduleRepository.Object.GetByDocTime(doctor_id, date).Success);
        }
    }
}