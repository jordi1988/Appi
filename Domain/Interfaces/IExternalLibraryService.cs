namespace Domain.Interfaces
{
    public interface IExternalLibraryService
    {
        bool IsAllowed();

        void Allow();

        void Prohibit();
    }
}
