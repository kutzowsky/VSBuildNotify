using System;

namespace VSBuildNotify.Notifiers.Pushbullet
{
    class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message) : base("Server response: " + message)
        {
        }
    }
}
