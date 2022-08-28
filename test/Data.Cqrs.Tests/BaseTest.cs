namespace Olbrasoft.Data.Cqrs;

public class BaseTest
{
    public Mock<IDispatcher> DispatcherMock => new Mock<IDispatcher>();

    public Mock<IRequestHandler<IRequest<object>, object>> HandlerMock => new Mock<IRequestHandler<IRequest<object>, object>>();

    public Mock<IRequestHandler<IRequest<bool>, bool>> BooleanHandlerMock => new Mock<IRequestHandler<IRequest<bool>, bool>>();
}