using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DevMeeting.API.Controllers;
using DevMeeting.API.Models;
using Xunit;

namespace DevMeeting.API.Tests.Controllers
{
    public class UserControllerShould
    {
        private readonly UserController _userController = new UserController();

        [Theory]
        [InlineData("Randerson")]
        public void GetAll_Ok(string userName)
        {
            var createdUser = (CreatedResult) _userController.Register(new User {Name = userName});
            Assert.IsType<CreatedResult>(createdUser);
            var userCollection = (OkObjectResult) _userController.GetAll();
            Assert.True(((List<User>) userCollection.Value).Count > 0);
        }

        [Theory]
        [InlineData("0000-0000-0000-000000-0000")]
        public void GetById_With_NotFound(string invalidTokenId)
        {
            var user = (NotFoundResult) _userController.GetById(invalidTokenId);
            Assert.IsType<NotFoundResult>(user);
        }

        [Theory]
        [InlineData(null)]
        public void GetById_With_BadRequest(string invalidTokenId)
        {
            var user = (BadRequestResult) _userController.GetById(invalidTokenId);
            Assert.IsType<BadRequestResult>(user);
        }

        [Theory]
        [InlineData("L2N")]
        public void GetById_With_Ok(string userName)
        {
            var createdUser = (CreatedResult) _userController.Register(new User {Name = userName});
            Assert.IsType<CreatedResult>(createdUser);
            var user = (OkObjectResult) _userController.GetById(((User) createdUser.Value).Id);
            Assert.Equal(((User) createdUser.Value).Id, ((User) user.Value).Id);
            Assert.IsType<OkObjectResult>(user);
        }

        [Theory]
        [InlineData(null, null)]
        public void Update_With_BadRequest(string invalidTokenId, string userName)
        {
            var notcreatedUser =
                (BadRequestResult) _userController.Update(invalidTokenId, new User {Name = userName});
            Assert.IsType<BadRequestResult>(notcreatedUser);
        }

        [Theory]
        [InlineData("324324234324234234234234", null)]
        public void Update_With_NotFound(string invalidTokenId, string userName)
        {
            var notFoundUser =
                (NotFoundResult) _userController.Update(invalidTokenId, new User {Name = userName});
            Assert.IsType<NotFoundResult>(notFoundUser);
        }

        [Theory]
        [InlineData("Julio", "Juliao")]
        public void Update_With_Accepted(string olduserName, string newuserName)
        {
            var createdUser = (CreatedResult) _userController.Register(new User {Name = olduserName});
            Assert.IsType<CreatedResult>(createdUser);
            var updatedUser = (AcceptedResult) _userController.Update(((User) createdUser.Value).Id,
                new User {Name = newuserName});
            Assert.Equal(((User) createdUser.Value).Id, ((User) updatedUser.Value).Id);
            Assert.NotEqual(((User) createdUser.Value).Name, ((User) updatedUser.Value).Name);
            Assert.Equal(((User) updatedUser.Value).Name, newuserName);
            Assert.IsType<AcceptedResult>(updatedUser);
        }

        [Theory]
        [InlineData(null)]
        public void Delete_With_BadRequest(string invalidTokenId)
        {
            var createdUser = (BadRequestResult) _userController.Delete(invalidTokenId);
            Assert.IsType<BadRequestResult>(createdUser);
        }

        [Theory]
        [InlineData("123123123123213123")]
        public void Delete_With_NotFound(string invalidTokenId)
        {
            var notFoundUser = (NotFoundResult) _userController.Delete(invalidTokenId);
            Assert.IsType<NotFoundResult>(notFoundUser);
        }

        [Theory]
        [InlineData("Machado")]
        public void Delete_With_Ok(string userName)
        {
            var createdUser = (CreatedResult) _userController.Register(new User {Name = userName});
            Assert.IsType<CreatedResult>(createdUser);
            var deleted = (OkResult) _userController.Delete(((User) createdUser.Value).Id);
            Assert.IsType<OkResult>(deleted);
        }
    }
}