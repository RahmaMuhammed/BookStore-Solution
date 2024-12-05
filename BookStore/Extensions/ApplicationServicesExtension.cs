using BookStore.Core.Repositories;
using BookStore.Errors;
using BookStore.Helpers;
using BookStore.Repository;
using BookStore.Services.Implementations;
using BookStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Extentions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServicesExtension (this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.AddScoped<IBookServices, BookService>();
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                              .Where(d => d.Value.Errors.Count > 0)
                                              .SelectMany(p => p.Value.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                    var validationErrorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            return Services;
        }
    }
}
