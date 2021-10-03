using System;
using System.Threading.Tasks;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Clients.Firebase;
using Domain.Clients.Firebase.Models;
using Domain.Services;
using Moq;
using Persistence.Models;
using Persistence.Repositories;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class AuthService_Should
    {
        // SignIn_WithSignInRequest_ReturnsSignInResponse
        [Fact]
        public async Task SignInAsync_WithSignInRequest_ReturnsSignInResponse()
        {
            // Arrange
            var firebaseClientMock = new Mock<IFirebaseClient>();
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var email = "Rokas";
            var password = "Passwordas";
            
            var signInRequest = new SignInRequest
            {
                Email = "Rokas",
                Password = "Passwordas"
            };

            var firebaseSignInResponse = new FirebaseSignInResponse
            {
                IdToken = Guid.NewGuid().ToString(),
                Email = signInRequest.Email,
                FirebaseId = Guid.NewGuid().ToString()
            };

            var userReadModel = new UserReadModel
            {
                Id = Guid.NewGuid(),
                FirebaseId = firebaseSignInResponse.FirebaseId,
                Username = Guid.NewGuid().ToString(),
                Email = firebaseSignInResponse.Email,
                DateCreated = DateTime.Now
            };

            firebaseClientMock
                .Setup(firebaseClient => firebaseClient.SignInAsync("Roks", "Passwordas"))
                .ReturnsAsync(firebaseSignInResponse);

            usersRepositoryMock
                .Setup(userRepository => userRepository.GetAsync(firebaseSignInResponse.FirebaseId))
                .ReturnsAsync(userReadModel);

            var expectedResult = new SignInResponse
            {
                Username = userReadModel.Username,
                Email = signInRequest.Email,
                IdToken = firebaseSignInResponse.IdToken
            };

            // system under test
            var sut = new AuthService(firebaseClientMock.Object, usersRepositoryMock.Object);
            
            // Act
            var result = await sut.SignInAsync(signInRequest);

            // Assert
            Assert.Equal(expectedResult.Username, result.Username);
            Assert.Equal(expectedResult.Email, result.Email);
            Assert.Equal(expectedResult.IdToken, result.IdToken);
        }
    }
}