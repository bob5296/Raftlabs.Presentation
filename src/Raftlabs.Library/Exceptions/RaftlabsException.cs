using System;

namespace Raftlabs.Library.Exceptions
{
    /// <summary>
    /// Base class for Raftlabs exceptions.
    /// </summary>
    public abstract class RaftlabsException : Exception
    {
        public RaftlabsException()
        {
        }

        public RaftlabsException(string message)
            : base(message)
        {
        }

        public RaftlabsException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
