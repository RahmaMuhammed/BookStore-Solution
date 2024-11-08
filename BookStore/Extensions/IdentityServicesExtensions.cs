using BookStore.Core.Entities.Identity;
using BookStore.Core.Services;
using BookStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Extentions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(); // UserManager / SigninManager / RoleManager
            return services;
        }
    }
}
