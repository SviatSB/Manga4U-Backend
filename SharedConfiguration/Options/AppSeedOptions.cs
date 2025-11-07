using SharedConfiguration.Interfaces;

namespace SharedConfiguration.Options
{
    public class AppSeedOptions : IAppOptions
    {
        public string OwnerLogin { get; set; } = null!;
        public string OwnerPassword { get; set; } = null!;
    }
}
