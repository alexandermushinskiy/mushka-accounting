using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;

namespace Mushka.Tests.Common
{
    internal static class MockExtensions
    {
        public static Mock<TMockObject> Setup<TMockObject, TResult>(
            this Mock<TMockObject> mock,
            Expression<Func<TMockObject, TResult>> setupExpression,
            TResult returnValue)
            where TMockObject : class
        {
            mock.Setup(setupExpression).Returns(returnValue);
            return mock;
        }

        public static Mock<TMockObject> SetupGet<TMockObject, TResult>(
            this Mock<TMockObject> mock,
            Expression<Func<TMockObject, TResult>> setupExpression,
            TResult returnValue)
            where TMockObject : class
        {
            mock.SetupGet(setupExpression).Returns(returnValue);
            return mock;
        }

        public static Mock<TMockObject> SetupAsync<TMockObject, TResult>(
            this Mock<TMockObject> mock,
            Expression<Func<TMockObject, Task<TResult>>> setupExpression,
            TResult returnValue)
            where TMockObject : class
        {
            mock.Setup(setupExpression).ReturnsAsync(returnValue);
            return mock;
        }
    }
}