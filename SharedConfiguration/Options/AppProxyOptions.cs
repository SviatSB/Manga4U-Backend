using SharedConfiguration.Interfaces;

namespace SharedConfiguration.Options
{
    public class AppProxyOptions : IAppOptions
    {
        public string UserAgent { get; set; } = null!;
    }
}
