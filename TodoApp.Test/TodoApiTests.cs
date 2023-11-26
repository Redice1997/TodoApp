using Moq;

namespace TodoApp.Test
{
    public class TodoApiTests
    {
        //private TodoDb CreateDbContext()
        //{
        //    var todos = new Todo[]
        //    {
        //        new()
        //        {
        //            Id = 1,
        //            Name = "Первое",
        //            IsComplete = true,
        //        },
        //        new()
        //        {
        //            Id = 2,
        //            Name = "Второе",
        //            IsComplete = false,
        //        },
        //        new()
        //        {
        //            Id = 2,
        //            Name = "Третье",
        //            IsComplete = false,
        //        }
        //    };

        //    var mock = new Mock<TodoDb>();
        //    mock.Setup(db => db.Todos).Returns(todos);
        //}

        [Fact]
        public async Task GetAllTodos_ReturnsOkOfTodosResult()
        {
            // Arrange
            // var db = CreateDbContext();

            // Act
            var result = await TodoItemsService.GetAllTodos(db);

            // Assert: Check for the correct returned type
            Assert.IsType<Ok<Todo[]>>(result);
        }
                
    }
}