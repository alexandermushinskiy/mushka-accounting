namespace Mushka.Core.Validation.Enums
{
    /// <summary>
    /// Validation result type.
    /// </summary>
    public enum ValidationStatusType
    {
        /// <summary>
        /// No Validation
        /// </summary>
        None = 0,

        /// <summary>
        /// Validation succeeded
        /// </summary>
        Success = 1,

        /// <summary>
        /// Bad input data
        /// </summary>
        BadOperation = 2,

        /// <summary>
        /// Not found
        /// </summary>
        NotFound = 3,

        /// <summary>
        /// Error result
        /// </summary>
        Error = 4,

        /// <summary>
        /// Forbidden resource
        /// </summary>
        Forbidden = 5
    }
}