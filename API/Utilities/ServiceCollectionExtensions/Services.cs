using Core.EntitiesQueryUtilities.Bugs;
using Core.EntitiesQueryUtilities.Comments;
using Core.EntitiesQueryUtilities.QueryBuilders;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;
using Core.EntitiesQueryUtilities.QueryParameters.Comments;
using Core.EntitiesQueryUtilities.QueryParameters.Replies;
using Core.EntitiesQueryUtilities.Replies;
using Core.Repositories;
using Core.Services.BugService;
using Core.Services.CommentService;
using Core.Services.ReplyService;
using Core.Services.SearchesService;
using Infrastructure.QueryBuilders;
using Infrastructure.Repositories;

public static class BugServiceCollectionExtensions
{
    public static IServiceCollection AddBugServices(this IServiceCollection services)
    {
        services.AddScoped<IBugService, BugService>();
        services.AddScoped<IBugRepository, BugRepository>();
        services.AddScoped<IBugQueryableBuilder, BugQueryableBuilder>();
        services.AddScoped<IBugQueryParametersFactory, BugQueryParametersFactory>();
        services.AddScoped<IBugFilterFactory, BugFilterFactory>();
        services.AddScoped<IBugSortingOptionsFactory, BugSortingOptionsFactory>();
        return services;
    }
}

public static class CommentServiceCollectionExtensions
{
    public static IServiceCollection AddCommentServices(this IServiceCollection services)
    {
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentQueryableBuilder, CommentQueryableBuilder>();
        services.AddScoped<ICommentQueryParametersFactory, CommentQueryParametersFactory>();
        services.AddScoped<ICommentFilterFactory, CommentFilterFactory>();
        services.AddScoped<ICommentSortingOptionsFactory, CommentSortingOptionsFactory>();
        return services;
    }
}

public static class ReplyServiceCollectionExtensions
{
    public static IServiceCollection AddReplyServices(this IServiceCollection services)
    {
        services.AddScoped<IReplyService, ReplyService>();
        services.AddScoped<IReplyRepository, ReplyRepository>();
        services.AddScoped<IReplyQueryableBuilder, ReplyQueryableBuilder>();
        services.AddScoped<IReplyQueryParametersFactory, ReplyQueryParametersFactory>();
        services.AddScoped<IReplyFilterFactory, ReplyFilterFactory>();
        services.AddScoped<IReplySortingOptionFactory, ReplySortingOptionsFactory>();
        return services;
    }
}

public static class MiscellaneousServiceCollectionExtensions
{
    public static IServiceCollection AddMiscellaneousServices(this IServiceCollection services)
    {
        services.AddScoped<INotifRepository, NotifRepository>();
        services.AddScoped<ISearchesRepository, SearchesRepository>();
        services.AddScoped<ISearchesService, SearchesService>();
        return services;
    }
}




