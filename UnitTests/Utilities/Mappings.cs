using AutoMapper;
using Core.AutoMapper;

namespace UnitTests.Utilities
{
    public static class Mappings
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(BugProfile));
                cfg.AddProfile(typeof(BugUserProfile));
                cfg.AddProfile(typeof(CommentProfile));
                cfg.AddProfile(typeof(ReplyProfile));
            });

            return config.CreateMapper();
        }
    }
}
