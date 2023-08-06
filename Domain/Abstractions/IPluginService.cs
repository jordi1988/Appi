namespace Core.Abstractions
{
    public interface IPluginService
    {
        bool IsAllowed();

        void Allow();

        void Prohibit();
    }
}
