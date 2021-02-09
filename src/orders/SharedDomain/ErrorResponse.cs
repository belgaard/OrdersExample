namespace Orders.SharedDomain
{
    /// <summary>Generic minimum error response from any service returning a 400- 40x HTTP Error Code</summary>
    public class ErrorResponse<T>
    {
        /// <summary>Internal error from Qte.</summary>
        internal string InternalError { get; set; }
        /// <summary>ErrorCode</summary>
        public T ErrorCode { get; set; }

        /// <summary>
        ///     Optional Textual information about the error to aid the developer.
        ///     NOTE: Remember to NOT include sensitive information here, which could help a hacker!
        /// </summary>
        public string Message { get; set; }
    }
}