namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Represents a type that can be retried.
    /// </summary>
    public interface IRetryable
    {
        /// <summary>
        /// Gets the maximum number of retries that should be attempted.
        /// </summary>
        int MaxRetries { get; }
    }
}