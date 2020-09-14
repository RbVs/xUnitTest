using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using Xunit;

namespace PersonalPhotos.Test2
{
    public class LoginsTests
    {
        private readonly LoginsController _controller;
        private readonly Mock<ILogins> _logins;
        private readonly Mock<IHttpContextAccessor> _accessor;
        

        public LoginsTests()
        {
            _logins = new Mock<ILogins>();
            _accessor = new Mock<IHttpContextAccessor>();
            _controller = new LoginsController(_logins.Object, _accessor.Object);
            var session = Mock.Of<ISession>();
            var httpContext = Mock.Of<HttpContext>(x=>x.Session == session);
            _accessor.Setup(x => x.HttpContext).Returns(httpContext);
        }


        [Fact]
        public void Index_GivenNorReturnUrl_ReturnLoginView1()
        {
            var result = _controller.Index();

            Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void Index_GivenNorReturnUrl_ReturnLoginView2()
        {
            var result = _controller.Index();

            Assert.IsType<ViewResult>(result); // -> ViewResult implementiert IActionResult
        }
        [Fact]
        public void Index_GivenNorReturnUrl_ReturnLoginView3()
        {
            var result = (_controller.Index() as ViewResult);

            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName, ignoreCase:true);
        }

        [Fact]
        public async Task Login_ModelStateInvalid_ReturnModelView()
        {
            //to check modelstate is valid, add some error mo the modelstate
            _controller.ModelState.AddModelError("Test", "Test");

            //v1
            //var model = new Mock<LoginViewModel>();
            //var result = _controller.Login(model.Object);

            //v2
            var result = await _controller.Login(Mock.Of<LoginViewModel>()) as ViewResult;

            Assert.Equal("Login", result.ViewName, ignoreCase:true);
        }

        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectToDisplayAction()
        {
            const string password = "123";
            var modelView = Mock.Of<LoginViewModel>(x=>x.Email == "a@b.com" && x.Password == password);
            var model = Mock.Of<User>(x=>x.Password == password);

            //_logins.Setup(x => x.GetUser(modelView.Email)).ReturnsAsync(model); // -> specific value
            _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model); // -> any value
            var result = await _controller.Login(modelView);

            Assert.IsType<RedirectToActionResult>(result);
        }

    }
}