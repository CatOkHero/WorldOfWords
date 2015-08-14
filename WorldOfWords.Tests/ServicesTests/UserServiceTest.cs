using Moq;
using NUnit.Framework;
using WorldOfWords.API.Models;
using WorldOfWords.API.Models.Models;

namespace WorldOfWords.Tests.ServicesTests
{
    [TestFixture]
    public class UserServiceTest
    {
        [Test]
        public void CheckUserAuthorizationTest()
        {
            //Arrange
            var userNotInDb = new UserWithPasswordModel
            {
                Email = "roman@example.com",
                Password = "3452"
            };
            var userInDb = new UserWithPasswordModel
            {
                Email = "roman@example.com",
                Password = "5422",
            };

            //Action
            Mock<IUserMapper> userMapper = new Mock<IUserMapper>();
            UserWithPasswordModel userModel = new UserWithPasswordModel();
            //UserService userService = new UserService(userMapper.Object);
            ////We need to get rid off dependency of DbManager to test correctly

            ////Assert
            //Assert.AreEqual(userService.CheckUserAuthorization(userInDb), true);
            //Assert.AreEqual(userService.CheckUserAuthorization(userNotInDb), true);
            //Assert.AreEqual(userInDb.Id, 1);
            //Assert.AreEqual(userInDb.Claim, new[] { "Teacher", "Student" });
        }
    }
}
