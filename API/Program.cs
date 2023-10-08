//main entry point to our application
//dotnet run executes the code inside here

using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);//creates our web application instance

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddDbContext<DataContext>//moved to extensions to clear up stuff

//using extended methods
builder.Services.AddApplicationServices(builder.Configuration);

//This also moved to extensions
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options => {
//         options.TokenValidationParameters = new TokenValidationParameters//how it will validate the tocken//on what factor
//         {
//             ValidateIssuerSigningKey = true,//check if the JWT is signed
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
//             ValidateIssuer = false,
//             ValidateAudience = false

//         };
//     });
// //inspect the request 

builder.Services.AddIdentityServices(builder.Configuration);



//everything before this is considered services container
var app = builder.Build();


// if (app.Environment.IsDevelopment())
// {
//     // app.UseSwagger();// we deleted swagger
//     // app.UseSwaggerUI();
//     //middleware code area//authorization code
// }

// app.UseHttpsRedirection();// redirects our http to https req// removed bcz we are always using https

// app.UseAuthorization();// for authentication // not used yet

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200") );//allow cross origin

//middleware for authentication and authorization
app.UseAuthentication();//asks do you have a valid token
app.UseAuthorization();//asks oky u have valid , what r u allowed to do?

app.MapControllers();// middleware to map our controller// tells our which api endpoint to go to

using var scope = app.Services.CreateScope();//this gives access to all the services we have inside this program class
var services = scope.ServiceProvider;
try{
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();//creates DB at start of the app if not there
  await Seed.SeedUsers(context);

}catch(Exception ex){
  var logger = services.GetService<ILogger<Program>>();
  logger.LogError(ex,"An error occured during Migration");
}

app.Run();
