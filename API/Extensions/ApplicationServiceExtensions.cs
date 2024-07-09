using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
services.AddControllers();
services.AddDbContext<DataContext>(opt =>
{
  opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
});
services.AddCors();
services.AddScoped<ITokenService, TokenService>();

return services;

  }
}
