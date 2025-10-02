using System.Text.Json.Serialization;
using Core.AutoMapper;
using Core.Entities.UserEntity;
using Core.Services.UserService;
using Core.Utilities.JsonConverters;

public static class OtherServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(opt =>
        {
            opt.AddProfile(typeof(BugProfile));
            opt.AddProfile(typeof(BugUserProfile));
            opt.AddProfile(typeof(CommentProfile));
            opt.AddProfile(typeof(ReplyProfile));
            opt.AddProfile(typeof(QueryProfile));
        });

        return services;
    }
}
