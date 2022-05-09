using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DemoLinqProvider
{
    public class SqlBuilder : ExpressionVisitor
    {
        private StringBuilder _stringBuilder;

        public SqlBuilder()
        {
            _stringBuilder = new StringBuilder();
        }
        public string Translate(Expression expression)
        {
            _stringBuilder.Clear();
            Visit(expression);
            return _stringBuilder.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if(node.Method.DeclaringType != typeof(Queryable) || node.Method.Name != "Where")
                throw new NotSupportedException($"Méthode {node.Method.Name} non supporté");

            Visit(node.Arguments[0]);
            _stringBuilder.Append(" WHERE ");
            LambdaExpression lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
            Visit(lambda.Body);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _stringBuilder.Append("(");
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AndAlso:
                    _stringBuilder.Append(" AND ");
                    break;
                case ExpressionType.Equal:
                    _stringBuilder.Append(" = ");
                    break;
                default:
                    throw new NotSupportedException($"Operateur {node.NodeType} non supporté");
            }
            Visit(node.Right);
            _stringBuilder.Append(")");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            IQueryable? queryable = node.Value as IQueryable;

            if(queryable is not null)
            {
                _stringBuilder.Append($"SELECT * FROM {queryable.ElementType.Name}");
            }
            else if (node.Value is null)
            {
                _stringBuilder.Append("NULL");
            }
            else
            {
                TypeCode typeCode = Type.GetTypeCode(node.Value.GetType());
                switch(typeCode)
                {
                    case TypeCode.Int32:
                        _stringBuilder.Append(node.Value);
                        break;
                    case TypeCode.String:
                        _stringBuilder.Append($"'{node.Value}'");
                        break;
                    default:
                        throw new NotSupportedException($"Type {typeCode} non supporté");
                }
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is null)
                throw new NullReferenceException("Expression is null!");

            switch(node.Expression.NodeType)
            {
                case ExpressionType.Constant:
                    {
                        object? value = (node.Member as FieldInfo)?.GetValue(((ConstantExpression)node.Expression).Value);
                        _stringBuilder.Append(value);
                        break;
                    }
                case ExpressionType.Parameter:
                    _stringBuilder.Append(node.Member.Name);
                    break;
                default:
                    throw new NotSupportedException($"Type de noeud '{node.Expression.NodeType}' non supporté");
            }

            return node;
        }
    }
}