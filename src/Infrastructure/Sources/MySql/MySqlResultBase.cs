using Core.Abstractions;

namespace Infrastructure.MySql
{
    /// <summary>
    /// Represents the base class for MySQL item results.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="ResultItemBase" />
    public abstract class MySqlResultBase<TResult> : ResultItemBase
        where TResult : class
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public TResult Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlResultBase{TResult}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        protected MySqlResultBase(TResult result)
        {
            Result = result;
        }
    }
}
