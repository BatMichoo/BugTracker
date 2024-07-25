using Infrastructure.Models.Bug;
using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public class BugDateFilter : IFilter<Bug>
    {
        private readonly DateTime _targetDate;
        private readonly string _operation;

        public BugDateFilter(DateTime targetDate, string operation)
        {
            _targetDate = targetDate;
            _operation = operation;
        }

        public Expression<Func<Bug, bool>> ToExpression()
        {
            switch (_operation)
            {
                case "=>":
                    return b => b.CreatedOn >= _targetDate;
                case "<=":
                    return b => b.CreatedOn <= _targetDate;
                default:
                    return b => true;
            }
        }
    }
}
