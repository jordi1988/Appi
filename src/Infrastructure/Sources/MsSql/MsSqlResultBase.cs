using Core.Abstractions;

namespace Infrastructure.MsSql
{
    public abstract class MsSqlResultBase<TResult> : ResultItemBase
        where TResult : class
    {
        public TResult Result { get; set; }

        protected MsSqlResultBase(TResult result)
        {
            Result = result;
        }
    }
}
