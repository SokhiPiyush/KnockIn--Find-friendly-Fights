//main entry point to our application
//dotnet run executes the code inside here

using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
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

var connString = "";
if (builder.Environment.IsDevelopment()) 
    connString = builder.Configuration.GetConnectionString("DefaultConnection");
else 
{
// Use connection string provided at runtime by fly.io.
        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        // Parse connection URL to connection string for Npgsql
        connUrl = connUrl.Replace("postgres://", string.Empty);
        var pgUserPass = connUrl.Split("@")[0];
        var pgHostPortDb = connUrl.Split("@")[1];
        var pgHostPort = pgHostPortDb.Split("/")[0];
        var pgDb = pgHostPortDb.Split("/")[1];
        var pgUser = pgUserPass.Split(":")[0];
        var pgPass = pgUserPass.Split(":")[1];
        var pgHost = pgHostPort.Split(":")[0];
        var pgPort = pgHostPort.Split(":")[1];
        var updatedHost = pgHost.Replace("flycast", "internal");

    connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";

        // connString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
}
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connString);
});



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

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200") );//allow cross origin

//middleware for authentication and authorization
app.UseAuthentication();//asks do you have a valid token
app.UseAuthorization();//asks oky u have valid , what r u allowed to do?

app.UseDefaultFiles();//picks the index.html from the wwwroot folder
app.UseStaticFiles();//looks for wwwroot folder and its content

app.MapControllers();// middleware to map our controller// tells our which api endpoint to go to

app.MapHub<PresenceHub>("hubs/presence");//SignalR hub
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();//this gives access to all the services we have inside this program class
var services = scope.ServiceProvider;
try{
  var context = services.GetRequiredService<DataContext>();
  var userManager = services.GetRequiredService<UserManager<AppUser>>();
  var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
  await context.Database.MigrateAsync();//creates DB at start of the app if not there
  // await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");//wrote method in seed class fro postgres
  // await Seed.SeedUsers(context);
  await Seed.ClearConnections(context);
  await Seed.SeedUsers(userManager, roleManager);

}catch(Exception ex){
  var logger = services.GetService<ILogger<Program>>();
  logger.LogError(ex,"An error occured during Migration");
}

app.Run();
