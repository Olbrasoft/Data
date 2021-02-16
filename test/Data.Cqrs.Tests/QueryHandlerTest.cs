using Moq;
using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Olbrasoft.Data.Cqrs
{
    public class QueryHandlerTest
    {
        [Fact]
        public void QueryHandler_Is_Abstract()
        {
            //Arrange
            var type = typeof(QueryHandler<,>);

            //Act
            var result = type.IsAbstract;

            //Assert
            Assert.True(result);
        }

        private static Mock<IProjector> CreateProjectorMock()
        {
            return new Mock<IProjector>();
        }

        [Fact]
        public void ProjectTo_Call_Method_ProjectTo_On_Projector()
        {
            //Arrange
            var projectorMock = CreateProjectorMock();
            var handler = new AwesomeQueryHandler(projectorMock.Object);
            var queryable = new List<char>().AsQueryable();

            //Act
            handler.CallProjectTo();

            //Assert
            projectorMock.Verify(p => p.ProjectTo<string>(queryable));
        }

        [Fact]
        public void AwesomeBooleanQueryHandler_Inherit_From_QueryHandler_Of_Request_Of_Bool()
        {
            //Arrange
            var type = typeof(QueryHandler<IRequest<bool>>);

            //Act
            var handler = new AwesomeBooleanQueryHandler();

            //Assert
            Assert.IsAssignableFrom(type, handler);
        }
    }
}