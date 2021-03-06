﻿using Microsoft.AspNetCore.Hosting;
using UnitTests.Base;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using HealthChecks.UI.InMemory.Storage;
using HealthChecks.UI.Core.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace UnitTests.UI.DatabaseProviders
{
    public class postgre_sql_storage_provider_should
    {
        private const string ProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";
        [Fact]
        public void register_healthchecksdb_context_with_migrations()
        {
            var customOptionsInvoked = false;

            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHealthChecksUI()
                    .AddPostgreSqlStorage("connectionString", options => customOptionsInvoked = true);
                })
                .UseStartup<DefaultStartup>();

            var services = hostBuilder.Build().Services;
            var context = services.GetService<HealthChecksDb>();

            context.Should().NotBeNull();
            context.Database.GetMigrations().Count().Should().BeGreaterThan(0);
            context.Database.ProviderName.Should().Be(ProviderName);
            customOptionsInvoked.Should().BeTrue();
        }
    }
}
