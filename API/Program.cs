//main entry point to our application
//dotnet run executes the code inside here

using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);//creates our web application instance

// Add services to the container.

builder.Services.AddControllers();

//to use DataContext class features throughout
builder.Services.AddDbContext<DataContext>(opt => 
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();


//everything before this is considered services container
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     // app.UseSwagger();// we deleted swagger
//     // app.UseSwaggerUI();
//     //middleware code area//authorization code
// }

// app.UseHttpsRedirection();// redirects our http to https req// removed bcz we are always using https

// app.UseAuthorization();// for authentication // not used yet

app.MapControllers();// middleware to map our controller// tells our which api endpoint to go to

app.Run();
