using Domain.Entities;

namespace Infrastructure.Sources.File
{
    public abstract class FileResult : Result
    {
        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
