﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.ActivitiesDTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Helper;

namespace Test.UnitTest.ActivitiesControllerTest
{
    [TestClass]
    public class ActivitiesTestGetAll : BaseTest
    {
        #region Object 
        private ApplicationDbContext _context;
        private AuthController authController;
        private IConfiguration _configuration;

        private IActivitiesServices _activitiesServices;
        private ActivitiesController _activitiesController;

        private IUserServices _userService;
        private IMailService _mailService;
        private IImageService _imageServices;
        private IOptions<JWTSettings> _jWTSettings;

        protected HttpClient _client;

        #endregion

        #region TestInitialize 
        [TestInitialize]
        public void MakeArrange()
        {
            var config = new Dictionary<string, string>{
                {"JWTSettings:Key", "1234567890qwertyuiop"},
                };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            _context = MakeContext("TestDb");

            _jWTSettings = Options.Create(new JWTSettings()
            {
                Key = "1234567890qwertyuiop",
                Issuer = "prueba",
                Audience = "prueba",
                DurationInDays = 1
            });
            IUnitOfWork unitOfWork = new UnitOfWork(_context);

            _activitiesServices = new ActivitiesServices(unitOfWork, _imageServices);
            _activitiesController = new ActivitiesController(_activitiesServices);

            _userService = new UserServices(unitOfWork, _configuration, _imageServices, _jWTSettings);
            authController = new AuthController(_userService, _mailService);
            SeedContacts(_context);
        }
        #endregion

        #region TestCleanup 
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
        #endregion

        #region GetAll_Should_Return_Count_Equals 
        [TestMethod]
        public async Task GetAll_Should_Return_Count_Equals()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedCount = 1;
            //Act
            var response = await _activitiesController.GetAll();
            // Assert
            Assert.AreEqual(expectedCount, response.Count());
        }
        #endregion

        #region GetAll_Should_Return_Ok
        [TestMethod]
        public async Task GetAll_Should_Return_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedCount = 1;
            //Act
            var response = await _activitiesController.GetAll();
            // Assert

            Assert.AreEqual(expectedCount, response.Count());
        }
        #endregion

        #region Test method Get_Unauthorized_User
        [TestMethod]
        public async Task Get_Unauthorized_User()
        {
            //Arrange
            Cleanup();
            MakeArrange();

            UserLoginRequestDTO userLogin = new UserLoginRequestDTO();
            userLogin.Email = "user@example.com";
            userLogin.Password = "12345";
            //Act
            var response = await authController.LoginAsync(userLogin) as ObjectResult;
            var user = response.Value as UserLoginResponseDTO;

            var result = UserRol(user);
            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region Private method UserRol
        private bool UserRol(UserLoginResponseDTO user)
        {
            var claims = new[] { new Claim(ClaimTypes.Role, user.Role) };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);

            authController.ControllerContext.HttpContext = contextMock.Object;

            return authController.User.IsInRole("Administrator");
        }
        #endregion

        #region Private method SeedContacts
        private void SeedContacts(ApplicationDbContext context)
        {
            var role = new Role
            {
                Id = 1,
                Name = "Administrator",
                Description = "User Administrator"
            };

            var roleStandard = new Role
            {
                Id = 2,
                Name = "Standard",
                Description = "User Standard"
            };

            var user = new User
            {
                FirstName = "User",
                LastName = "Test",
                Email = "user@example.com",
                Password = Encrypt.GetSHA256("12345"),
                Photo = null,
                RoleId = 2
            };
            var userLogin = new User
            {
                FirstName = "UserLogin",
                LastName = "TestLogin",
                Email = "ailenadrianagomez@gmail.com",
                Password = Encrypt.GetSHA256("12345"),
                Photo = null,
                RoleId = 1
            };

            var activities = new Activities
            {
                Name = "Organization ",
                Content = "Content",
                Image = null
            };

            context.Add(role);
            context.Add(roleStandard);

            context.Add(user);
            context.Add(userLogin);

            context.Add(activities);
            context.SaveChanges();
        }
        #endregion
    }
}