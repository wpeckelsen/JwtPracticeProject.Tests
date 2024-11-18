using Moq;
using Xunit;
using JwtPracticeProject.Models;
using JwtPracticeProject.Service;
using JwtPracticeProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JwtPracticeProject
{
    public class UserServiceTests : IDisposable
    {



        // init user set
        private readonly Mock<DbSet<User>> _mockUserSet;

        // init db interaction
        private readonly ApplicationDbContext _context;


        // init userservice
        private readonly UserService _userService;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public UserServiceTests()
        {
            // Mocking DbSet<User>
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseSqlite("DataSource=:memory:")  // In-memory SQLite database
             .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.BeginTransaction();
            _context.Database.EnsureCreated();
            _mockConfiguration = new Mock<IConfiguration>();




            // Passing both mocks to the UserService constructor
            _userService = new UserService(_context, _mockConfiguration.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnUser()
        {

            // arrange
            var userId = 451;
            var username = "fahrenheit";
            var hashedPassword = "123";
            var role = "fireman";
            var expectedUser = new User { Id = userId, Username = username, HashedPassword = hashedPassword, Role = role };
            _context.Users.Add(expectedUser);
            await _context.SaveChangesAsync();



            // act
            var result = await _userService.GetUserByIdAsync(userId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.Role, result.Role);
            Assert.Equal(expectedUser.HashedPassword, result.HashedPassword);

        }

        [Fact]
        public async Task DoesUserExistByUsernameAsync_UserExists_ReturnTrue()
        {

            var userId = 451;
            var username = "fahrenheit";
            var hashedPassword = "123";
            var role = "fireman";
            var expectedUser = new User { Id = 451, Username = username, HashedPassword = hashedPassword, Role = role };
            _context.Users.Add(expectedUser);
            await _context.SaveChangesAsync();

            var result = await _userService.doesUserExistByUsernameAsync(username);

            Assert.True(result);

        }

        [Fact]
        public async Task DoesUserExistByUsernameAsync_UserExistsNot_ReturnFalse()
        {
            // arrange
            var user = "fahrenheit";

            // act
            var result = await _userService.doesUserExistByUsernameAsync(user);

            // assert
            Assert.False(result);

        }

        // 
        [Fact]
        public async Task CreateUserAsync_UserCreated_ReturnUser()
        {

 
            // arrange
            var username = "fahrenheit";
            var plainPassword = "123";
            // act
            var newUser = await _userService.CreateUserAsync(username, plainPassword);
            // Assert 
            Assert.NotNull(newUser);
            Assert.Equal(username, newUser.Username);
            Assert.True(BCrypt.Net.BCrypt.Verify(plainPassword, newUser.HashedPassword)); // Verify the hashed password matches
        }

        public void Dispose()
        {
            _context.Database.RollbackTransaction();
            _context.Database.CloseConnection();
        }
    }
}