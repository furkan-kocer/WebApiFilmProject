using FilmProject.Contracts.DataTransferObjects.User;
using FilmProject.DataAccess.Entities;
using Mapster;

namespace FilmProject.Api.Extensions
{
    public static class MappingConfigurationsExtension
    {
        public static IServiceCollection AddMappingConfigurations(this IServiceCollection services)
        {
            TypeAdapterConfig<UserLoginRequest, User>
                .NewConfig()
                .Map(dest => dest.Email, src => src.Field.Contains("@") == true ? src.Field : null)
                .Map(dest => dest.PhoneNumber, src => src.Field.Contains("@") == false ? src.Field : null);
            TypeAdapterConfig<User, UserLoginTokenResponse>
                .NewConfig()
                .Map(dest => dest.userID, src => src.Id)
                .Map(dest => dest.email, src => src.Email)
                .Map(dest => dest.phoneNumber, src => src.PhoneNumber);
            return services;
        }
    }
}
