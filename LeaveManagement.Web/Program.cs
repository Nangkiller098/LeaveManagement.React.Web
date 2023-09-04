using LeaveManagement.Application.Contracts;
using LeaveManagement.Application.MappingProfile;
using LeaveManagement.Application.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddIdentityCore<IdentityUser>()
.AddRoles<IdentityRole>()

;
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
