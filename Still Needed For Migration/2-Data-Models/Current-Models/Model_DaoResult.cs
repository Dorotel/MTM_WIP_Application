using System;

namespace MTM_Inventory_Application.Models
{
    #region DAO Result Models

    /// <summary>
    /// Standard result class for DAO operations that provides success/failure status and error information
    /// </summary>
    /// <typeparam name="T">The type of data returned by the operation</typeparam>
    public class DaoResult<T>
    {
        #region Properties

        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The data returned by the operation (null if operation failed)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Error message if the operation failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Exception details if available
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Additional status information
        /// </summary>
        public string StatusMessage { get; set; } = string.Empty;

        /// <summary>
        /// Number of rows affected (for update/delete operations)
        /// </summary>
        public int RowsAffected { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a successful result with data
        /// </summary>
        public static DaoResult<T> Success(T data, string statusMessage = "", int rowsAffected = 0)
        {
            return new DaoResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusMessage = statusMessage,
                RowsAffected = rowsAffected
            };
        }

        /// <summary>
        /// Creates a successful result without data
        /// </summary>
        public static DaoResult<T> Success(string statusMessage = "", int rowsAffected = 0)
        {
            return new DaoResult<T>
            {
                IsSuccess = true,
                StatusMessage = statusMessage,
                RowsAffected = rowsAffected
            };
        }

        /// <summary>
        /// Creates a failed result with error information
        /// </summary>
        public static DaoResult<T> Failure(string errorMessage, Exception? exception = null)
        {
            return new DaoResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        /// <summary>
        /// Creates a failed result from an exception
        /// </summary>
        public static DaoResult<T> Failure(Exception exception)
        {
            return new DaoResult<T>
            {
                IsSuccess = false,
                ErrorMessage = exception.Message,
                Exception = exception
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implicitly converts successful results to the data type
        /// </summary>
        public static implicit operator T?(DaoResult<T> result)
        {
            return result.IsSuccess ? result.Data : default;
        }

        /// <summary>
        /// Returns a string representation of the result
        /// </summary>
        public override string ToString()
        {
            if (IsSuccess)
            {
                return $"Success: {StatusMessage} (Rows: {RowsAffected})";
            }
            return $"Failed: {ErrorMessage}";
        }

        #endregion
    }

    /// <summary>
    /// Simple DAO result for operations that don't return data
    /// </summary>
    public class DaoResult
    {
        #region Properties

        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if the operation failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Exception details if available
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Additional status information
        /// </summary>
        public string StatusMessage { get; set; } = string.Empty;

        /// <summary>
        /// Number of rows affected (for update/delete operations)
        /// </summary>
        public int RowsAffected { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a successful result
        /// </summary>
        public static DaoResult Success(string statusMessage = "", int rowsAffected = 0)
        {
            return new DaoResult
            {
                IsSuccess = true,
                StatusMessage = statusMessage,
                RowsAffected = rowsAffected
            };
        }

        /// <summary>
        /// Creates a failed result with error information
        /// </summary>
        public static DaoResult Failure(string errorMessage, Exception? exception = null)
        {
            return new DaoResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        /// <summary>
        /// Creates a failed result from an exception
        /// </summary>
        public static DaoResult Failure(Exception exception)
        {
            return new DaoResult
            {
                IsSuccess = false,
                ErrorMessage = exception.Message,
                Exception = exception
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implicitly converts to boolean (true if successful)
        /// </summary>
        public static implicit operator bool(DaoResult result)
        {
            return result.IsSuccess;
        }

        /// <summary>
        /// Returns a string representation of the result
        /// </summary>
        public override string ToString()
        {
            if (IsSuccess)
            {
                return $"Success: {StatusMessage} (Rows: {RowsAffected})";
            }
            return $"Failed: {ErrorMessage}";
        }

        #endregion
    }

    #endregion
}
