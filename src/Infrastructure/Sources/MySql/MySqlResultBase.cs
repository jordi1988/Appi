using Core.Abstractions;

namespace Infrastructure.MySql
{
    public abstract class MySqlResultBase<TResult> : ResultItemBase
        where TResult : class
    {
        public TResult Result { get; set; }

        protected MySqlResultBase(TResult result)
        {
            Result = result;
        }
    }
}
