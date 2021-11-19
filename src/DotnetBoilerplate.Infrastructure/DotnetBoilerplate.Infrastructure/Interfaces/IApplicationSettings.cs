using DotnetBoilerplate.Infrastructure.Configuration;

namespace DotnetBoilerplate.Infrastructure.Interfaces
{
    public interface IApplicationSettings
    {
        public AmazonSettings AmazonSettings { get; }
    }
}