using crud_web_api_mvc.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("crud_web_api_mvc"));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
