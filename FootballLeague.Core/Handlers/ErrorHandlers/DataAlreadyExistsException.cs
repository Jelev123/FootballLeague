namespace FootballLeague.Core.Handlers.ErrorHandlers
{
    using System;

    public class DataAlreadyExistsException : Exception
    {
        public DataAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
