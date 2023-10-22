using Core.Abstractions;

namespace Infrastructure.Sources.Sql
{
    /// <summary>
    /// Represents the base class for SQL item results.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="ResultItemBase" />
    public abstract class SqlResultBase<TResult> : ResultItemBase
        where TResult : class
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public TResult Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlResultBase{TResult}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        protected SqlResultBase(TResult result)
        {
            Result = result;
        }
    }
}
