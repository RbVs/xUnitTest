using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using Xunit;

namespace PersonalPhotos.Test2
{
    public class PhotosTests
    {
        [Fact]
        public async Task Upload_GivenFileName_ReturnsDisplayAction()
        {
            // Arrange

            var session = Mock.Of<ISession>();
            session.Set("User", Encoding.UTF8.GetBytes("a@b.com"));
            var context = Mock.Of<HttpContext>(x => x.Session == session);
            var accessor = Mock.Of<IHttpContextAccessor>(x=>x.HttpContext == context);

            var fileStorage = Mock.Of<IFileStorage>();
            var keyGen = Mock.Of<IKeyGenerator>();
            var photoMetaData = Mock.Of<IPhotoMetaData>();

            var fromFile = Mock.Of<IFormFile>();
            var model = Mock.Of<PhotoUploadViewModel>(x => x.File == fromFile);

            var controller = new PhotosController(keyGen, accessor, photoMetaData, fileStorage);

            // Act

            var result = await controller.Upload(model) as RedirectToActionResult;

            // Assert

            Assert.Equal("Display", result.ActionName, ignoreCase:true);
        }
    }
}