using System;
using System.Linq.Expressions;
using ExpressionExtensionSQL.Extensions;
using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using Xunit;

namespace ExpressionExtensionSQL.Tests
{
    public class ConcatenatedExpressionTest
    {
        [Fact(DisplayName = "ConcatenatedExpression - false OR Equal")]
        public void FalseOrEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => false || x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(0=1 OR ([Merchant].[Name] = @p1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - true OR Equal")]
        public void TrueOrEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => true || x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(1=1 OR ([Merchant].[Name] = @p1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - false AND Equal")]
        public void FalseAndEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => false && x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(0=1 AND ([Merchant].[Name] = @p1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - true AND Equal")]
        public void TrueAndEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => true && x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(1=1 AND ([Merchant].[Name] = @p1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - NOT HasValue AND Equal")]
        public void HasValueAndEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => !x.Status.HasValue && x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(([Merchant].[Status] IS NULL) AND ([Merchant].[Name] = @p1))");
        }
    }
}