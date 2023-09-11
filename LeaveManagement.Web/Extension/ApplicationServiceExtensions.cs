using System.Text;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Application.MappingProfile;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace LeaveManagement.Web.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicatinServices(this IServiceCollection Services,IConfiguration config)
        {
            Services
            .AddControllers(opt=>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddOData(opt=>
            {
                opt.Select().Filter().OrderBy();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();

            //connectionstring
            Services.AddDbContext<ReactDbContext>(opt =>
                opt.UseSqlite(config.GetConnectionString("SqliteConnection")));
            Services.AddIdentityCore<ApiUser>()
            .AddRoles<IdentityRole>() //Role User
            .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("LeaveManagementReact") 
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ReactDbContext>(); //Dbcontext

            //config CORS for to use other thrid party host ex: serilog, react etc.
            Services.AddCors(opt=>{
                opt.AddPolicy("AllowAll",
                    b=>b.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod());
                });
            
            //Iservice 
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<ILeaveTypesRepository, LeaveTypesRepository>();
            Services.AddScoped<IAuthManager,AuthManager>();

            //add JWT Authentication 
            Services.AddAuthentication(opt=>{
                opt.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme; //"Bearer"
                opt.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt=>{
                opt.TokenValidationParameters= new TokenValidationParameters
                {
                    ValidateIssuerSigningKey=true, //for prevent trying to scope token if they get the key they still can't use it
                    ValidateIssuer=true, //make sure came from our user
                    ValidateAudience=true, //came from user who recongine
                    ValidateLifetime=true, //no lifetime must expire
                    ClockSkew=TimeSpan.Zero, //reset time 
                    
                    //get value from app.setting
                    ValidIssuer=config["JwtSettings:Issuer"],
                    ValidAudience=config["JwtSettings:Audience"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
                };
            });

            //Caching api
            Services.AddResponseCaching(opt=>
            {
                opt.MaximumBodySize=1024;
                opt.UseCaseSensitivePaths=true;
            });
            //autoMapper
            Services.AddAutoMapper(typeof(MappingProfile));

            //Change Api Versions
            Services.AddApiVersioning(opt=> 
            {
                opt.AssumeDefaultVersionWhenUnspecified=true;
                opt.DefaultApiVersion = new ApiVersion(1,0);
                opt.ReportApiVersions =true;
                opt.ApiVersionReader=ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver")
                );
            });
            Services.AddVersionedApiExplorer(opt=>
            {
                opt.GroupNameFormat="'v'VVV";
                opt.SubstituteApiVersionInUrl=true;
            }
            );
          
            return Services;
        }
    
    }
}