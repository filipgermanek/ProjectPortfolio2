using System;
using ProjectPortfolio2.DatabaseModel;
using Xunit;

namespace UnitTests
{
    public class DataServiceTests
    {
        [Fact]
        public void Question_Object_HasIdTitleAndBody()
        {
            var question = new Question();
            Assert.Equal(0, question.Id);
            Assert.Null(question.Title);
            Assert.Null(question.Body);
        }

        [Fact]
        public void CreateUser_ValidData_CreteUserAndRetunsNewObject()
        {
            var service = new DataService();
            var user = service.CreateUser("testmail@mail", "Test Password", "Test", "Test Location");
            Assert.True(user.Id > 0);
            Assert.Equal("Test", user.Name);
            Assert.Equal("testmail@mail", user.Email);
            Assert.Equal("Test Location", user.Location);
            Assert.Equal("Test Password", user.Password);

            // cleanup
            service.DeleteUser(user.Id);
        }

        [Fact]
        public void UpdateUser_NewNameAndLocation_UpdateWithNewValues()
        {
            var service = new DataService();
            var user = service.CreateUser("testmail@mail", "Test Password", "Test", "Test Location");

            var result = service.UpdateUser(user.Id, "Changed Email", "Changed PWD", "Changed Name", "Changed Location");

            user = service.GetUser(user.Id);

            Assert.Equal("Changed Name", user.Name);
            Assert.Equal("Changed Email", user.Email);
            Assert.Equal("Changed Location", user.Location);
            Assert.Equal("Changed PWD", user.Password);

            // cleanup
            service.DeleteUser(user.Id);
        }

        [Fact]
        public void Search_Posts_Number_Ok()
        {
            var service = new DataService();
            var posts1 = service.SearchPosts("M_PI", 1);
            //expected to return 2 questions
            Assert.Equal(2, posts1.Count);

            //expected to return 19 questions
            var posts3 = service.SearchPosts("GCC M_PI", 1);
            Assert.Equal(19, posts3.Count);
        }

        [Fact]
        public void Search_Posts_Verify_Title_Ok()
        {
            var service = new DataService();
            var posts = service.SearchPosts("M_PI", 1);
            Assert.Equal("What is the fastest way to get the value of π?", posts[0].Title);
        }
    }
}
