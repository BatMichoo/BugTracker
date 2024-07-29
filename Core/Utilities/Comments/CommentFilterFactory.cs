using Infrastructure.Models.CommentEntity;

namespace Core.Utilities.Comments
{
    public class CommentFilterFactory : ICommentFilterFactory
    {
        public Task<IFilter<Comment>> CreateFilter(CommentFilterType filterBy, object value)
        {
            switch (filterBy)
            {
                case CommentFilterType.BugId:
                    return Task.FromResult((IFilter<Comment>) new CommentByBugIdFilter((int)value));
                    default:
                    return Task.FromResult(default(IFilter<Comment>));
            }
        }

        public Task<IList<IFilter<Comment>>> CreateFilters(string? filterInput)
        {
            var filters = new List<IFilter<Comment>>();

            if (filterInput != null)
            {
                string[] filtersData = filterInput.Split(';').ToArray();

                foreach (var filterData in filtersData)
                {
                    string[] filterInfo = filterData.Split("_");

                    if (!Enum.TryParse(filterInfo[0], true, out CommentFilterType type))
                    {
                        continue;
                    }

                    string propertyValue = filterInfo[1];

                    IFilter<Comment> filter;

                    switch (type)
                    {
                        case CommentFilterType.PostedOn:
                            string operation = filterInfo[2];
                            var success = DateTime.TryParse(propertyValue, out DateTime createdOn);

                            if (!success)
                                createdOn = DateTime.UtcNow;

                            filter = new CommentPostedOnFilter(createdOn, operation);
                            break;
                        case CommentFilterType.AuthorId:
                            filter = new CommentCreatedByFilter(propertyValue);
                            break;
                        case CommentFilterType.BugId:
                            filter = new CommentByBugIdFilter(int.Parse(propertyValue));
                            break;
                        case CommentFilterType.Likes:
                            string likesOperation = filterInfo[2];
                            filter = new CommentLikesFilter(int.Parse(propertyValue), likesOperation);
                            break;
                        default:
                            continue;

                    }

                    filters.Add(filter);
                }
            }

            return Task.FromResult((IList<IFilter<Comment>>)filters);
        }
    }
}
