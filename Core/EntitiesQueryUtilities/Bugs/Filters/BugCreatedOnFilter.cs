using Core.Entities.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugCreatedOnFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugFilterType.CreatedOn);
        private readonly DateTime _targetDate;
        private readonly string _operation;

        public BugCreatedOnFilter(DateTime targetDate, string operation) : base(_name, $"{targetDate}{(operation != string.Empty ? $"_{operation}" : string.Empty)}")
        {
            _targetDate = targetDate;
            _operation = operation;
        }

        public Expression<Func<Bug, bool>> Apply()
        {
            switch (_operation)
            {
                case ">=":
                    return b => b.CreatedOn.Date >= _targetDate.Date;
                case ">":
                    return b => b.CreatedOn.Date > _targetDate.Date;
                case "<=":
                    return b => b.CreatedOn.Date <= _targetDate.Date;
                case "<":
                    return b => b.CreatedOn.Date < _targetDate.Date;
                default:
                    return b => b.CreatedOn.Date == _targetDate.Date;
            }
        }
    }
}
