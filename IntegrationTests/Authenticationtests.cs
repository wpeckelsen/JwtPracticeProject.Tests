
using JwtPracticeProject.Models;

namespace JwtPracticeProject.Tests.IntegrationTests
{

    public class Authenticationtests : IntegrationTestBase
    {
        [Fact]
        public async void Should_return_correct_authentication()
        {
            var username = "testuser";
            var password = "123";

            var newUser = new User
            {
                Username = username,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(password)
            };

            //         await AddAsync(newUser);



        }




    }
}

