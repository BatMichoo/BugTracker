using Infrastructure.Models.CommentEntity;

namespace Core.Utilities.Comments
{
    public class CommentFilterFactory : ICommentFilterFactory
    {
        public IFilter<Comment> CreateFilter(CommentFilterType filterBy, string? value)
        {
            switch (filterBy)
            {
                case CommentFilterType.BugId:
                    return new CommentByBugIdFilter(int.Parse(value));
                default:
                    return default;
            }
        }

        public IList<IFilter<Comment>> CreateFilters(string? filterInput)
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

            return filters;
        }
    }
}
