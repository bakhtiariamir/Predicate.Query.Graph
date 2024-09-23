using Priqraph.Exception;

namespace Priqraph.Test.Query
{
    public class QueryBuilderTest(QueryableFixture queryableFixture) : IClassFixture<QueryableFixture>
    {
        [Fact]
        public void Queryable_NULL_Return_Exception()
        {
            //Arrange
            var exceptionType = typeof(QueryNullException);
            var message = "The query object is null.";
            var codeName = "query_No_Argument_Provided";
            
            //Act
            var ex = Assert.Throws<QueryNullException>( () => queryableFixture.QueryBuilder.SetQuery(null));
            //Assert
            Assert.Equal(ex.GetType(), exceptionType);
            Assert.Equal(ex.Message, message);
            Assert.Equal(ex.CodeName, codeName);
        }

        [Fact]
        public void Query_Builder_Only_Init_Generate_Should_Validate_False()
        {
            //Arrange
            var exceptionType = typeof(NotSupportedOperationException);
            var codeName = "query_Not_Supported";
            
            //Act
            queryableFixture.InitQuery();
            var ex = Assert.Throws<NotSupportedOperationException>(() => queryableFixture.QueryBuilder.Generate());
            //Assert
            Assert.Equal(ex.GetType(), exceptionType);
            Assert.Equal(ex.CodeName, codeName);
        }
        
        [Fact]
        public void Query_Builder_Without_Init_Generate_Should_Validate_False()
        {
            //Arrange
            var exceptionType = typeof(NotValidException);
            var codeName = "query_No_Argument_Provided";
            
            //Act
            var ex = Assert.Throws<NotValidException>(() => queryableFixture.QueryBuilder.Generate());
            //Assert
            Assert.Equal(ex.GetType(), exceptionType);
            Assert.Equal(ex.CodeName, codeName);
        }
    }
    
    
}