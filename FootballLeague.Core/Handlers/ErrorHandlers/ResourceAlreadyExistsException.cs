namespace FootballLeague.Core.Handlers.ErrorHandlers
{
    using System;

    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
