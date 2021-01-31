using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DTO;
using System.Linq;
using System.Net;

namespace SharedLibrary.Extensions
{
    public static class CustomValidationResponse
    {
        public static void AddCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    ErrorDTO error = new ErrorDTO(errors.ToList(), true);

                    var response = Response<NoContentResult>.Fail(error, (int)HttpStatusCode.BadRequest);
                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
