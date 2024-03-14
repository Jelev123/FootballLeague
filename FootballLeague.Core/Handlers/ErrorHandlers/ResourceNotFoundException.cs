namespace FootballLeague.Core.Handlers.ErrorHandlers
{
    using System;

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
