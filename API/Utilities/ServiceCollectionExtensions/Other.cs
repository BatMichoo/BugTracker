using Core.AutoMapper;

namespace API.Utilities.ServiceCollectionExtensions
{
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
}
