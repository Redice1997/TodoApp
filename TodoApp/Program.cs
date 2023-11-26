using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>();
var app = builder.Build();

app.MapGet("/items", async (TodoDb db) => 
    await db.Todos.ToListAsync());

app.MapGet("/items/complete", async (TodoDb db) =>
    await db.Todos.Where(x => x.IsComplete).ToListAsync());

app.MapGet("/items/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/items", async (Todo item, TodoDb db) =>
{
    db.Todos.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}", async (int id, Todo item, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = item.Name;
    todo.IsComplete = item.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
