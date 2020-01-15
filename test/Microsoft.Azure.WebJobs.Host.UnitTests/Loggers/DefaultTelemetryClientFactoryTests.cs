// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Azure.WebJobs.Logging.ApplicationInsights;
using Xunit;

namespace Microsoft.Azure.WebJobs.Host.UnitTests.Loggers
{
    public class DefaultTelemetryClientFactoryTests
    {
        private readonly string _mockInstrumentationKey = "some_key";
        private readonly string _mockConnectionString = "InstrumentationKey=some_other_key";

        [Fact]
        public void InitializeConfiguguration_Configures()
        {
            var factory = new DefaultTelemetryClientFactory(string.Empty, string.Empty, null, null);
            var config = factory.InitializeConfiguration();

            // Verify Initializers
            Assert.Equal(3, config.TelemetryInitializers.Count);
            // These will throw if there are not exactly one
            config.TelemetryInitializers.OfType<WebJobsRoleEnvironmentTelemetryInitializer>().Single();
            config.TelemetryInitializers.OfType<WebJobsTelemetryInitializer>().Single();
            config.TelemetryInitializers.OfType<WebJobsSanitizingInitializer>().Single();

            // Verify Channel
            Assert.IsType<ServerTelemetryChannel>(config.TelemetryChannel);
        }

        [Fact]
        public void InitializeConfiguration_Configures_WithConnectionString()
        {
            var factory = new DefaultTelemetryClientFactory(_mockInstrumentationKey, _mockConnectionString, null, null);
            var config = factory.InitializeConfiguration();

            // Verify Key/ConnectionString
            Assert.Equal(_mockConnectionString, config.ConnectionString);
            Assert.Equal("some_other_key", config.InstrumentationKey);
        }
    }
}
