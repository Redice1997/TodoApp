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

todoItems.MapGet("/", async (TodoDb db) => 
    await db.Todos.ToListAsync());

todoItems.MapGet("/completed", async (TodoDb db) =>
    await db.Todos.Where(x => x.IsComplete).ToListAsync());

todoItems.MapGet("/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

todoItems.MapPost("/", async (Todo item, TodoDb db) =>
{
    db.Todos.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/{item.Id}", item);
});

todoItems.MapPut("/{id}", async (int id, Todo item, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = item.Name;
    todo.IsComplete = item.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

todoItems.MapDelete("/{id}", async (int id, TodoDb db) =>
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
