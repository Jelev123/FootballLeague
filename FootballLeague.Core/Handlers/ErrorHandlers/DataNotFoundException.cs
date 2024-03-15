namespace FootballLeague.Core.Handlers.ErrorHandlers
{
    using System;

    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message) : base(message)
        {
        }
    }
}
