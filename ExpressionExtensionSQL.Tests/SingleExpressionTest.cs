using System;
using System.Linq;
using System.Linq.Expressions;
using ExpressionExtensionSQL.Extensions;
using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using Xunit;

namespace ExpressionExtensionSQL.Tests
{
    public class SingleExpressionTest
    {
        [Fact(DisplayName = "SingleExpression - Equal")]
        public void EqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] = @1)");
        }

        [Fact(DisplayName = "SingleExpression - Annotation - Equal")]
        public void EqualWithAnnotationExpression()
        {
            Expression<Func<Order, bool>> expression = x => x.TotalAmount == 1;
            var where = expression.ToSql();
            where.Sql.Should().Be("([tblOrder].[amount] = 1)");
        }

        [Fact(DisplayName = "SingleExpression - NotEqual")]
        public void NotEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.Name != "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] <> @1)");
        }

        [Fact(DisplayName = "SingleExpression - GreaterThan")]
        public void GreaterThanExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount > 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] > 10)");
        }

        [Fact(DisplayName = "SingleExpression - GreaterThanOrEqual")]
        public void GreaterThanOrEqualExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount >= 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] >= 10)");
        }

        [Fact(DisplayName = "SingleExpression - Contains")]
        public void ContainsExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Name.Contains("pedro");
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Name] LIKE @1)");
        }

        [Fact(DisplayName = "SingleExpression - NotContains")]
        public void NotContainsExpression()
        {
            Expression<Func<Product, bool>> expression = x => !x.Name.Contains("pedro");
            var where = expression.ToSql();
            where.Sql.Should().Be("(NOT ([Product].[Name] LIKE @1))");
        }

        [Fact(DisplayName = "SingleExpression - LessThan")]
        public void LessThanExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount < 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] < 10)");
        }

        [Fact(DisplayName = "SingleExpression - LessThanOrEqual")]
        public void LessThanOrEqualExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount <= 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] <= 10)");
        }

        [Fact(DisplayName = "SingleExpression - FluentMap - Equal")]
        public void EqualWithFluentMapExpression()
        {
            Configuration.GetInstance().Entity<Order>().ToTable("tblTeste");
            Configuration.GetInstance().Entity<Order>().Property(p => p.TotalAmount).ToColumn("valor");
            Expression<Func<Order, bool>> expression = x => x.TotalAmount == 1;
            var where = expression.ToSql();
            where.Sql.Should().Be("([tblTeste].[valor] = 1)");
        }

        [Fact(DisplayName = "SingleExpression - IS NULL")]
        public void IsNullExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.DeletedAt == null;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[DeletedAt] IS NULL)");
        }

        [Fact(DisplayName = "SingleExpression - IS NOT NULL")]
        public void IsNotNullExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.DeletedAt != null;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[DeletedAt] IS NOT NULL)");
        }

        [Fact(DisplayName = "SingleExpression - boolean - true")]
        public void BooleanExpressionTrue()
        {
            Expression<Func<Merchant, bool>> expression = x => x.IsEnabled == true;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[IsEnabled] = 1)");
        }

        [Fact(DisplayName = "SingleExpression - boolean - false")]
        public void BooleanExpressionFalse()
        {
            Expression<Func<Merchant, bool>> expression = x => x.IsEnabled == false;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[IsEnabled] = 0)");
        }

        [Fact(DisplayName = "SingleExpression - boolean - true - unary")]
        public void BooleanExpressionTrueUnary()
        {
            Expression<Func<Merchant, bool>> expression = x => x.IsEnabled;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[IsEnabled] = 1)");
        }

        [Fact(DisplayName = "SingleExpression - boolean - false - unary")]
        public void BooleanExpressionFalseUnary()
        {
            Expression<Func<Merchant, bool>> expression = x => !x.IsEnabled;
            var where = expression.ToSql();
            where.Sql.Should().Be("(NOT ([Merchant].[IsEnabled] = 1))");
        }

        [Fact(DisplayName = "SingleExpression - true")]
        public void BooleanExpressionOnlyTrueUnary()
        {
            Expression<Func<Merchant, bool>> expression = x => true;
            var where = expression.ToSql();
            where.Sql.Should().Be("1=1");
        }

        [Fact(DisplayName = "SingleExpression - HasValue")]
        public void BooleanExpressionHasValueUnary()
        {
            Expression<Func<Merchant, bool>> expression = x => x.Status.HasValue;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Status] IS NOT NULL)");
        }

        [Fact(DisplayName = "SingleExpression - NOT HasValue")]
        public void BooleanExpressionNotHasValueUnary()
        {
            Expression<Func<Merchant, bool>> expression = x => !x.Status.HasValue;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Status] IS NULL)");
        }

        [Fact(DisplayName = "SingleExpression - Nullable Contains")]
        public void BooleanExpressionInUnary()
        {
            Expression<Func<Merchant, bool>> expression = x => new[] { StatusEnum.Enable }.Contains(x.Status.Value);
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Status] IN @p1)");
        }

        [Fact(DisplayName = "SingleExpression - TRIM")]
        public void BooleanExpressionTrimUnary()
        {
            var aa = "1111 ";
            Expression<Func<Merchant, bool>> expression = x => x.Name == aa.Trim();
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] = @p1)");
        }
    }
}