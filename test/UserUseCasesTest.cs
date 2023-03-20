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

namespace Test.SessionUseCases
{

    [TestClass]
    public class UserUseCasesTest
    {

        private Mock<S.ISessionRepository> SessionRepository;
        private Mock<Sc.IScheduleRepository> ScheduleRepository;
        private Mock<Sp.ISpecializationRepository> SpecializationRepository;
        private Mock<D.IDoctorRepository> DoctorRepository;
        private Mock<U.IUserRepository> UserRepository;
        private U.UserUseCases UseCases;

        UserUseCasesTest()
        {
            SessionRepository = new Mock<S.ISessionRepository>();
            ScheduleRepository = new Mock<Sc.IScheduleRepository>();
            SpecializationRepository = new Mock<Sp.ISpecializationRepository>();
            DoctorRepository = new Mock<D.IDoctorRepository>();
            UserRepository = new Mock<U.IUserRepository>();
            UseCases = new U.UserUseCases(UserRepository.Object);
        }

        [X.Fact]
        public void SignUpUserByEmpty_Fail() {
            R.Role role = R.Role.Patient;
            U.User user = new U.User("", "", role, "", "");

            var result = UseCases.SignUpUser(user);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Some fields are empty");
        }

        [X.Fact]
        public void SignUpUserByUserExists_Fail() {
            R.Role role = R.Role.Patient;
            U.User user = new U.User("1", "2", role, "3", "4");

            UserRepository.Setup(rep => rep.ExistsByLogin(user.Login)).Returns(true);

            var result = UseCases.SignUpUser(user);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "User with this login already exists");

            UserRepository.Setup(rep => rep.ExistsByLogin(user.Login)).Returns(false);
            UserRepository.Setup(rep => rep.Exists(user.UserId)).Returns(true);

            result = UseCases.SignUpUser(user);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "User with this ID already exists");
        }

        [X.Fact]
        public void SignUpUser_Ok() {
            R.Role role = R.Role.Patient;
            U.User user = new U.User("1", "2", role, "3", "4");

            UserRepository.Setup(rep => rep.ExistsByLogin(user.Login)).Returns(false);
            UserRepository.Setup(rep => rep.Exists(user.UserId)).Returns(false);

            var result = UseCases.SignUpUser(user);

            X.Assert.True(result.Success);
        }

        [X.Fact]
        public void SignInUserByEmpty_Fail() {
            string login = "";
            string password = "";

            var result = UseCases.SignInUser(login, password);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Login or password are empty");
        }

        [X.Fact]
        public void SignInUserByIncorrect_Fail() {
            string login = "1";
            string password = "1";

            UserRepository.Setup(rep => rep.check(login, password)).Returns(true);

            
            var result = UseCases.SignInUser(login, password);

            X.Assert.False(result.Success);
            Assert.Equals(result.Error, "Login or password are empty");
        }
    }
}