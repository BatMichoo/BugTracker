﻿using Infrastructure;
using Infrastructure.Models.Bug;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class BugRepository : AdvancedRepository<Bug>
    {
        public BugRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        internal override IQueryable<Bug> AddInclusions(IQueryable<Bug> query)
        {
            query = query
                .Include(b => b.Creator)
                .Include(b => b.Assignee)
                .Include(b => b.LastUpdatedBy)
                .Include(b => b.Comments);

            return query;
        }

        internal override IQueryable<Bug> ApplySearch(IQueryable<Bug> query, string searchTerm)
            => query.Where(b => b.Description.Contains(searchTerm));
    }
}
