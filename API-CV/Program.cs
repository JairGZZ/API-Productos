using API_CV.BdContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var RulesCORS = "ReglasCors";

builder.Services.AddCors(option => option.AddPolicy(name: RulesCORS,
    builder =>
    {

        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }));

builder.Configuration.AddJsonFile("appsettings.json");

var secretkey = builder.Configuration["Settings:secretkey"];

if (secretkey == null)

{

    throw new Exception();
}
var keyBytes = Encoding.UTF8.GetBytes(secretkey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false

    };

});



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PruebaApiRestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PruebaApiRest")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(RulesCORS);


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
