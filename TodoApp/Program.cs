using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

var todoItems = app.MapGroup("/items");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/completed", GetCompletedTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db) => TypedResults.Ok(await db.Todos.ToListAsync());

static async Task<IResult> GetCompletedTodos(TodoDb db) => TypedResults.Ok(await db.Todos.Where(x => x.IsComplete).ToListAsync());

static async Task<IResult> GetTodo(int id, TodoDb db) => await db.Todos.FindAsync(id) is Todo todo ? TypedResults.Ok(todo) : TypedResults.NotFound();


static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return TypedResults.Created($"items/{todo.Id}", todo);
}

static async Task<IResult> UpdateTodo(int id, Todo updatedTodo, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = updatedTodo.Name;
    todo.IsComplete = updatedTodo.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.Ok(todo);
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}