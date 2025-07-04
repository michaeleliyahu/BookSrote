using System;

namespace BookstoreApi.Exceptions
{
    public class InvalidBookDataException : Exception
    {
        public InvalidBookDataException(string message)
            : base(message)
        {
        }
    }
}
