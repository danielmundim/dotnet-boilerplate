using DotnetBoilerplate.Infrastructure.Interfaces;

namespace DotnetBoilerplate.Infrastructure.Configuration
{
    public class ApplicationSettings : IApplicationSettings
    {
        public AmazonSettings AmazonSettings { get; }
    }
}