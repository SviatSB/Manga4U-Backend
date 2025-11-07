namespace SharedConfiguration.Interfaces
{
    public interface IAppConfiguration
    {
        public T GetOptions<T>() where T : IAppOptions, new();
        public T GetOptions<T>(string sectionName) where T : IAppOptions, new();
    }
}
