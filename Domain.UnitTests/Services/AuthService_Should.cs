using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Clients.Firebase;
using Domain.Clients.Firebase.Models;
using Domain.Services;
using Domain.UnitTests.Attributes;
using FluentAssertions;
using Moq;
using Persistence.Models;
using Persistence.Repositories;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class AuthService_Should
    {
        
        [Theory, AutoMoqData]
        public async Task SignInAsync_WithSignInRequest_ReturnsSignInResponse(
            SignInRequest signInRequest,
            FirebaseSignInResponse firebaseSignInResponse,
            UserReadModel userReadModel,
            [Frozen] Mock<IFirebaseClient> firebaseClientMock,
            [Frozen] Mock<IUsersRepository> usersRepositoryMock,
            AuthService sut)
        {
            // Arrange
            firebaseSignInResponse.Email = signInRequest.Email;

            userReadModel.FirebaseId = firebaseSignInResponse.FirebaseId;
            userReadModel.Email = firebaseSignInResponse.Email;
  
            firebaseClientMock
                .Setup(firebaseClient => firebaseClient.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(firebaseSignInResponse);
        
            usersRepositoryMock
                .Setup(userRepository => userRepository.GetAsync(firebaseSignInResponse.FirebaseId))
                .ReturnsAsync(userReadModel);
        
            // Act
            var result = await sut.SignInAsync(signInRequest);
        
            // Assert
            userReadModel.Username.Should().BeEquivalentTo(result.Username);
            signInRequest.Email.Should().BeEquivalentTo(result.Email);
            firebaseSignInResponse.IdToken.Should().BeEquivalentTo(result.IdToken);
        }

        [Theory]
        [AutoMoqData]
        public async Task SignUpAsync_With_SignUpRequest(
            SignUpRequest signUpRequest, 
            FirebaseSignUpResponse firebaseSignUpResponse,
            [Frozen] Mock<IFirebaseClient> firebaseClientMock,
            [Frozen] Mock<IUsersRepository> usersRepositoryMock,
            AuthService sut)
        {
            // Arrange
            firebaseSignUpResponse.Email = signUpRequest.Email;

            firebaseClientMock
                .Setup(firebaseClient => firebaseClient.SignUpAsync(signUpRequest.Email, signUpRequest.Password))
                .ReturnsAsync(firebaseSignUpResponse);
            
            // Act
            var result = await sut.SignUpAsync(signUpRequest);
            
            // Assert
            usersRepositoryMock.Verify(userRepository => userRepository.SaveAsync(It.Is<UserReadModel>(model => 
            model.FirebaseId.Equals(firebaseSignUpResponse.FirebaseId) &&
            model.Username.Equals(signUpRequest.Username) &&
            model.Email.Equals(firebaseSignUpResponse.Email))), Times.Once);
            
            firebaseClientMock
                .Verify(firebaseClient => firebaseClient.SignUpAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            
            result.Id.GetType().Should().Be<Guid>();
            result.IdToken.Should().BeEquivalentTo(firebaseSignUpResponse.IdToken);
            result.Email.Should().BeEquivalentTo(firebaseSignUpResponse.Email);
            result.Email.Should().BeEquivalentTo(signUpRequest.Email);
            result.Username.Should().BeEquivalentTo(signUpRequest.Username);
            result.DateCreated.GetType().Should().Be<DateTime>();
        }
    }
}