using AutoMapper;
using Xunit;

namespace AcrylicWindow.Client.Tests
{
    public class AutomapperTests
    {
        [Fact]
        public void CorrectlyConfigured()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());

            config.AssertConfigurationIsValid();
        }
    }
}
