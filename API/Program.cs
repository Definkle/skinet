using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "MyPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5000").AllowAnyHeader().AllowAnyMethod(); // This includes DELETE
        }
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("MyPolicy");
app.MapControllers();

app.Run();
