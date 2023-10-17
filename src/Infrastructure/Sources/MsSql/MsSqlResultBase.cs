using Core.Abstractions;

namespace Infrastructure.MsSql
{
    /// <summary>
    /// Represents the base class for SQL Server item results.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="ResultItemBase" />
    public abstract class MsSqlResultBase<TResult> : ResultItemBase
        where TResult : class
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public TResult Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlResultBase{TResult}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        protected MsSqlResultBase(TResult result)
        {
            Result = result;
        }
    }
}
