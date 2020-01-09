using System;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Exception indicating that the test result is inconclusive.
    /// </summary>
    public class InconclusiveTestResultException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveTestResultException"/> class.
        /// </summary>
        public InconclusiveTestResultException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveTestResultException"/> class with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InconclusiveTestResultException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveTestResultException"/> class with the given <paramref name="message"/> and <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InconclusiveTestResultException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}