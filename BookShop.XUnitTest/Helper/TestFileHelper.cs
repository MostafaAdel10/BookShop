using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;

namespace BookShop.XUnitTest.Helper
{
    public static class TestFileHelper
    {
        public static IFormFile CreateTestFormFile(string content = "Fake image content", string fileName = "test.jpg", string contentType = "image/jpeg")
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "ImageData", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
