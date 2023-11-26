using TodoApp.Repository;
using TodoApp.Services;

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

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", TodoItemsService.GetAllTodos);
todoItems.MapGet("/completed", TodoItemsService.GetCompletedTodos);
todoItems.MapGet("/{id}", TodoItemsService.GetTodo);
todoItems.MapPost("/", TodoItemsService.CreateTodo);
todoItems.MapPut("/{id}", TodoItemsService.UpdateTodo);
todoItems.MapDelete("/{id}", TodoItemsService.DeleteTodo);

app.Run();