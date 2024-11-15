namespace API.Utilities.ErrorMessages
{
    public static class ErrorMessage
    {
        public static class Bugs
        {
            public const string NotFound = "A bug with Id {0} has not been found.";
            public const string UnableToCreate = "Could not create requested bug.";
            public const string InvalidUpdateModel = "Bug model invalid.";
        }

        public static class Comments
        {
            public const string NotFound = "A comment with Id {0} has not been found.";
            public const string InvalidBugIdCommentIdPairing = "Invalid Id pairings.";
            public const string UnableToCreate = "Could not create requested comment.";
            public const string InvalidUpdateModel = "Comment model invalid.";
        }

        public static class Replies
        {
            public const string NotFound = "A reply with Id {0} has not been found.";
            public const string InvalidCommentIdReplyIdPairing = "Invalid Id pairings.";
        }

        public static class Users
        {
            public const string NotFound = "User with Id {0} does not exist.";
            public const string LoginFailed = "Wrong email or password.";
            public const string CouldNotAssignRole = "Could not assign role {0} to user";
        }
    }
}
