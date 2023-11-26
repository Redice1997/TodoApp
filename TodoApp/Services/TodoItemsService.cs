using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repository;

namespace TodoApp.Services
{
    public class TodoItemsService
    {
        public static async Task<IResult> GetAllTodos(TodoDb db) => TypedResults.Ok(await db.Todos.ToListAsync());

        public static async Task<IResult> GetCompletedTodos(TodoDb db) => TypedResults.Ok(await db.Todos.Where(x => x.IsComplete).ToListAsync());

        public static async Task<IResult> GetTodo(int id, TodoDb db) => await db.Todos.FindAsync(id) is Todo todo ? TypedResults.Ok(todo) : TypedResults.NotFound();


        public static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }

        public static async Task<IResult> UpdateTodo(int id, Todo updatedTodo, TodoDb db)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Name = updatedTodo.Name;
            todo.IsComplete = updatedTodo.IsComplete;

            await db.SaveChangesAsync();

            return TypedResults.Ok(todo);
        }

        public static async Task<IResult> DeleteTodo(int id, TodoDb db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
}
