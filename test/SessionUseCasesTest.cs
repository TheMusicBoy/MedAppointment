using Moq;
using X = Xunit;

using System;
using System.Collections.Generic;

using Domain.IRepository;
using S = Domain.Session;
using Sc = Domain.Schedule;
using Sp = Domain.Specialization;
using D = Domain.Doctor;
using U = Domain.User;

namespace Test.SessionUseCases
{

    [TestClass]
    public class SessionUseCasesTest
    {

        private Mock<S.ISessionRepository> SessionRepository;
        private Mock<Sc.IScheduleRepository> ScheduleRepository;
        private Mock<Sp.ISpecializationRepository> SpecializationRepository;
        private Mock<D.IDoctorRepository> DoctorRepository;
        private Mock<U.IUserRepository> UserRepository;
        private S.SessionUseCases UseCases;

        SessionUseCasesTest()
        {
            SessionRepository = new Mock<S.ISessionRepository>();
            ScheduleRepository = new Mock<Sc.IScheduleRepository>();
            SpecializationRepository = new Mock<Sp.ISpecializationRepository>();
            DoctorRepository = new Mock<D.IDoctorRepository>();
            UserRepository = new Mock<U.IUserRepository>();
            UseCases = new S.SessionUseCases(SessionRepository.Object, ScheduleRepository.Object, SpecializationRepository.Object, DoctorRepository.Object, UserRepository.Object);
        }

        [X.Fact]
        public void FreeTimeByDoctorNotExists_Fail()
        {
            int doctor_id = 1;
            int duration = 1;
            DateOnly date = new DateOnly(2023, 1, 1);

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(false);

            var result = UseCases.FreeTime(doctor_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Doctor doesn't exists");
        }

        [X.Fact]
        public void FreeTimeByDoctorHasNoSchedule_Fail()
        {
            int doctor_id = 1;
            int duration = 1;
            DateOnly date = new DateOnly(2023, 1, 1);

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(true);
            ScheduleRepository.Setup(rep => rep.GetByDocTime(doctor_id, date)).Returns(Result.Fail<Sc.Schedule>("_"));

            var result = UseCases.FreeTime(doctor_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Doctor has no schedule this date");
        }

        [X.Fact]
        public void FreeTimeByDurationTooLong_Fail()
        {
            int doctor_id = 1;
            int duration = 21;
            DateOnly date = new DateOnly(2023, 1, 1);

            DateTime sc_begin = new DateTime(2023, 1, 1, 8, 0, 0);
            DateTime sc_end = new DateTime(2023, 1, 1, 8, 0, 1);

            Sc.Schedule schedule = new Sc.Schedule(doctor_id, sc_begin, sc_end);

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(true);
            ScheduleRepository.Setup(rep => rep.GetByDocTime(doctor_id, date)).Returns(Result.Ok<Sc.Schedule>(schedule));
            SessionRepository.Setup(rep => rep.GetLast(doctor_id, date)).Returns(Result.Fail<S.Session>("_"));

            var result = UseCases.FreeTime(doctor_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Doctor hasn't enough time");
        }

        [X.Fact]
        public void FreeTimeByDoctorHasNoTime_Fail()
        {
            int doctor_id = 1;
            int duration = 21;
            DateOnly date = new DateOnly(2023, 1, 1);

            DateTime sc_begin = new DateTime(2023, 1, 1, 8, 0, 0);
            DateTime sc_end = new DateTime(2023, 1, 1, 8, 40, 0);

            Sc.Schedule schedule = new Sc.Schedule(doctor_id, sc_begin, sc_end);

            S.Session session = new S.Session(0, doctor_id, sc_begin, sc_begin.AddMinutes(21));

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(true);
            ScheduleRepository.Setup(rep => rep.GetByDocTime(doctor_id, date)).Returns(Result.Ok<Sc.Schedule>(schedule));
            SessionRepository.Setup(rep => rep.GetLast(doctor_id, date)).Returns(Result.Ok<S.Session>(session));

            var result = UseCases.FreeTime(doctor_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Doctor hasn't enough time");
        }
        
        [X.Fact]
        public void FreeTime_Ok()
        {
            int doctor_id = 1;
            int duration = 10;
            DateOnly date = new DateOnly(2023, 1, 1);

            DateTime sc_begin = new DateTime(2023, 1, 1, 8, 0, 0);
            DateTime sc_end = new DateTime(2023, 1, 1, 8, 40, 0);

            Sc.Schedule schedule = new Sc.Schedule(doctor_id, sc_begin, sc_end);

            S.Session session = new S.Session(0, doctor_id, sc_begin, sc_begin.AddMinutes(21));

            DoctorRepository.Setup(rep => rep.Exists(doctor_id)).Returns(true);
            ScheduleRepository.Setup(rep => rep.GetByDocTime(doctor_id, date)).Returns(Result.Ok<Sc.Schedule>(schedule));
            SessionRepository.Setup(rep => rep.GetLast(doctor_id, date)).Returns(Result.Ok<S.Session>(session));

            var result = UseCases.FreeTime(doctor_id, duration, date);

            X.Assert.True(result.Success);
        }

        [X.Fact]
        public void CreateSessionPatientNotExists_Fail() {
            int patient_id = 1;
            int doctor_id = 1;
            int duration = 10;
            DateOnly date = new DateOnly(2023, 1, 1);

            UserRepository.Setup(rep => rep.Exists(patient_id)).Returns(false);

            var result = UseCases.CreateSession(patient_id, doctor_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Patient doesn't exists");
        }

        [X.Fact]
        public void CreateSessionNoFreeDoctor_Fail() {
            int patient_id = 1;
            int duration = 10;
            DateOnly date = new DateOnly(2023, 1, 1);

            UserRepository.Setup(rep => rep.Exists(patient_id)).Returns(true);
            DoctorRepository.Setup(rep => rep.GetAll()).Returns(new List<D.Doctor>());

            var result = UseCases.CreateSession(patient_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "There are no doctor free this date");
        }

        [X.Fact]
        public void GetFreeDateSinceByNoSpecialization_Fail() {
            int spec_id = 1;
            int duration = 1;
            DateOnly date = new DateOnly(2023, 1, 1);

            SpecializationRepository.Setup(rep => rep.Exists(spec_id)).Returns(false);

            var result = UseCases.GetFreeDateSince(spec_id, duration, date);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Specialization doesn't exists");
        }
    }
}