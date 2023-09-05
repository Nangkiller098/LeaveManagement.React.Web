using System.Text;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Application.MappingProfile;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connectionstring
builder.Services.AddDbContext<ReactDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
builder.Services.AddIdentityCore<ApiUser>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ReactDbContext>();

//config CORS
builder.Services.AddCors(opt=>{
    opt.AddPolicy("AllowAll",
        b=>b.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
    });
builder.Host.UseSerilog((ctx,lc)=>lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ILeaveTypesRepository, LeaveTypesRepository>();
builder.Services.AddScoped<IAuthManager,AuthManager>();

//add JWT Authentication 
builder.Services.AddAuthentication(opt=>{
    opt.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme; //"Bearer"
    opt.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt=>{
    opt.TokenValidationParameters= new TokenValidationParameters
    {
        ValidateIssuerSigningKey=true, //for prevent trying to scope token if they get the key they still can't use it
        ValidateIssuer=true, //make sure came from our user
        ValidateAudience=true, //came from user who recongine
        ValidateLifetime=true, //no lifetime must expire
        ClockSkew=TimeSpan.Zero,
        ValidIssuer=builder.Configuration["JwtSettings:Issuer"],
        ValidAudience=builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
    };
});

//autoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
