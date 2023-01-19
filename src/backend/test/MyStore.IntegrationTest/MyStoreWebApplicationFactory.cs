namespace MyStore.IntegrationTest
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Login;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class MyStoreWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SLADbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<SLADbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<SLADbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<MyStoreWebApplicationFactory<TStartup>>>();

                    context.Database.EnsureCreated();

                    try
                    {
                        SeedSampleData(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database with sample data. Error: {ex.Message}.");
                    }
                })
                .UseEnvironment("Test");
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAdminClientAsync()
        {
            return await GetAuthenticatedClientAsync("Admin", "12345678");
        }

        public async Task<HttpClient> GetNonAdminClientAsync(string userName, string password)
        {
            return await GetAuthenticatedClientAsync(userName, password);
        }

        private async Task<HttpClient> GetAuthenticatedClientAsync(string userName, string password)
        {
            var client = CreateClient();

            var token = await GetAccessTokenAsync(client, userName, password);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        private async Task<string> GetAccessTokenAsync(HttpClient client, string userName, string password)
        {
            var command = new UserLoginCommand
            {
                UserName = userName,
                Password = password,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.EnsureSuccessStatusCode();
            var result = await response.GetResponseContent<UserLoginViewModel>();
            return result.Token;
        }

        public void CreateEntity<T>(T entity)
            where T : class
        {
            var scopeFactory = Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetService<SLADbContext>();

            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var scopeFactory = Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetService<SLADbContext>();
            return context.Set<T>().FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetEntities<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var scopeFactory = Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetService<SLADbContext>();
            return context.Set<T>().Where(predicate).ToList();
        }

        public void SeedSampleData(SLADbContext context)
        {
            {
                var admin = new User(Guid.NewGuid(), "Admin", "Admin", "MyStore", PasswordHelper.Hash("12345678"));
                admin.Active();
                var role = new Role(Guid.NewGuid(), "Admin", "SystemAdmin");
                role.UpdateIsSystemAdmin(true);
                context.Users.Add(admin);
                context.Roles.Add(role);
                admin.AddRole(role.Id);
                context.SaveChanges();
            }
            context.SaveChanges();
        }
    }
}
