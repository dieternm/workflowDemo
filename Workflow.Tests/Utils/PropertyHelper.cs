using System.Linq.Expressions;
using System.Reflection;

namespace Workflow.Tests.Utils
{
    public static class PropertyHelper
    {
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object?>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
