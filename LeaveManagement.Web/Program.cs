using LeaveManagement.Web.Extension;
using LeaveManagement.Web.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



//services in applicatinservicesextensions
builder.Services.AddApplicatinServices(builder.Configuration);

//serilog
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//seriloglogging
app.UseSerilogRequestLogging();

//middleware for globalhandler error
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
//reason use allowall cuz we use like serilog and react port so we need to allow all
app.UseCors("AllowAll");

//add this before cors
app.UseResponseCaching();

//caching (to reduce for loading page)
app.Use(async(context,next)=>
{
    context.Response.GetTypedHeaders().CacheControl=
    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
    {
        Public=true,
        MaxAge=TimeSpan.FromSeconds(10),
    };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary]=
    new string[]{"accept-Encoding"};
    await next();
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
