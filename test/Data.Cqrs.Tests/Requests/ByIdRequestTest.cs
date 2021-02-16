using Olbrasoft.Dispatching;
using Xunit;

namespace Olbrasoft.Data.Cqrs.Requests
{
    public class ByIdRequestTest : BaseTest
    {
        [Fact]
        public void ByIdRequest_Inherit_From_Request()
        {
            //Arrange
            var type = typeof(Request<object>);

            //Act
            var request = new ByIdRequest<object>(DispatcherMock.Object);

            //Assert
            Assert.IsAssignableFrom(type, request);
        }

        [Fact]
        public void ByIdRequest_Has_An_Id_Property_Of_Type_Integer()
        {
            //Arrange
            var request = new ByIdRequest<object>(HandlerMock.Object);

            //Act
            var id = request.Id;

            //Assert
            Assert.IsAssignableFrom<int>(id);
        }

        [Fact]
        public void BooleanBy_Inherit_From_ByIdRequest_Of_Bool()
        {
            //Arrange
            var type = typeof(ByIdRequest<bool>);

            //Act
            var request = new ByIdRequest(DispatcherMock.Object);

            //Assert
            Assert.IsAssignableFrom(type, request);
        }

        /// <summary>
        /// if the constructor creates a request with a handler, then the asynchronous execute method calls the asynchronous handle method on the handler
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_Call_HandleAsync()
        {
            //Arrange
            var handlerMock = BoleanHandlerMock;
            var request = new ByIdRequest(handlerMock.Object);

            //Act
            var response = await request.ExecuteAsync();

            //Assert
            handlerMock.Verify(p => p.HandleAsync(request, default));
        }
    }
}