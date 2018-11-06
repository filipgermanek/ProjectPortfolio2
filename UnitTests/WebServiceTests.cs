using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
namespace UnitTests
{
    public class WebServiceTests
    {
        private const string PostsApi = "https://localhost:5001/api/posts";
        [Fact]
        public void ApiQuestionById_Ok()
        {
            var url = $"{PostsApi}/5158603";
            var (data, statusCode) = GetObject(url);
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("jQuery and Uploadify session in the php file", data["title"]);
            Assert.Equal(2, data["score"]);
        }

        [Fact]
        public void ApiPostCommentById_Ok()
        {
            var url = $"{PostsApi}/26583319/comments/41782719";
            var (data, statusCode) = GetObject(url);
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(0, data["score"]);
            Assert.Equal("loadClass just loads the class. You then need to create a new instance of the class.", data["text"]);
        }


        //Helpers
        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

    }
}
