﻿using Core.DTOs.Bugs;

namespace Core.DTOs.Users
{
    public class UserAssignedBugsModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<BugModel> AssignedBugs { get; set; } = new List<BugModel>();
    }
}
