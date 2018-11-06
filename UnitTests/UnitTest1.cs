using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
namespace UnitTests
{
    public class UnitTest1
    {
        private const string PostsApi = "http://localhost:5001/api/posts";
        [Fact]
        public void ApiQuestions_GetWithNoArguments_Ok()
        {

        }

    }
}
