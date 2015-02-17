using System;

namespace MinistryPlatform.Translation.Exceptions
{
    public class MultipleRecordsException: Exception
    {
        public MultipleRecordsException()
        {
        }

        public MultipleRecordsException(string message)
            : base(message)
        {
        }

        public MultipleRecordsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
