using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

using Services.DTOs.HistoryDTOs;
using Services.DtoValidators;
using Services.Interfaces;
using Services.Services;

namespace Services.Extensions
{
    public static class FluetnValidationExtension
    {
        public static IServiceCollection AddFluentValidationExtensions(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<UpdateHistoryDtoValidator>(); //Жесткая зависимость не есть хорошо

            return services;
        }
    }
}
