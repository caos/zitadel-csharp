using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;
using Zitadel.Authentication;
using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication
{
    public class ServiceAccountTest : IClassFixture<AuthenticationHandlerWebFactory>
    {
        private readonly AuthenticationHandlerWebFactory _factory;

        public ServiceAccountTest(AuthenticationHandlerWebFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public Task Throws_On_Relative_File_Not_Found() =>
            Assert.ThrowsAsync<FileNotFoundException>(() => ServiceAccount.LoadFromJsonFileAsync("./foobar.json"));

        [Fact]
        public Task Throws_On_Absolute_File_Not_Found() =>
            Assert.ThrowsAsync<FileNotFoundException>(() => ServiceAccount.LoadFromJsonFileAsync("/root/foobar.json"));

        [Fact]
        public async Task Should_Load_ServiceAccount_From_Relative_Path()
        {
            var sa = await ServiceAccount.LoadFromJsonFileAsync("./TestData/ServiceAccount.json");
            sa.UserId.Should().Be("85328809577999392");
            sa.KeyId.Should().Be("85329282544495260");
        }

        [Fact]
        public async Task Should_Load_ServiceAccount_From_Absolute_Path()
        {
            var path = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), "./TestData/ServiceAccount.json"));
            var sa = await ServiceAccount.LoadFromJsonFileAsync(path);
            sa.UserId.Should().Be("85328809577999392");
            sa.KeyId.Should().Be("85329282544495260");
        }

        [Fact]
        public async Task Should_Load_ServiceAccount_From_FileStream()
        {
            var path = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), "./TestData/ServiceAccount.json"));
            var sa = await ServiceAccount.LoadFromJsonStreamAsync(File.OpenRead(path));
            sa.UserId.Should().Be("85328809577999392");
            sa.KeyId.Should().Be("85329282544495260");
        }

        [Fact]
        public async Task Should_Load_ServiceAccount_From_JsonString()
        {
            var path = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), "./TestData/ServiceAccount.json"));
            var json = await File.ReadAllTextAsync(path);
            var sa = await ServiceAccount.LoadFromJsonStringAsync(json);
            sa.UserId.Should().Be("85328809577999392");
            sa.KeyId.Should().Be("85329282544495260");
        }

        [Fact]
        public Task Throws_On_Broken_Json() =>
            Assert.ThrowsAsync<JsonException>(() => ServiceAccount.LoadFromJsonStringAsync("{d"));

        [Fact]
        public async Task Should_Fetch_An_AccessToken()
        {
            var sa = await ServiceAccount.LoadFromJsonFileAsync("./TestData/ServiceAccount.json");
            var token = await sa.AuthenticateAsync(
                ZitadelDefaults.DiscoveryEndpoint,
                new ServiceAccount.AuthenticationOptions
                {
                    Issuer = ZitadelDefaults.Issuer,
                });

            token.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task Should_Be_Able_To_Login_With_Plain_Token()
        {
            var sa = await ServiceAccount.LoadFromJsonFileAsync("./TestData/ServiceAccount.json");
            var token = await sa.AuthenticateAsync(
                ZitadelDefaults.DiscoveryEndpoint,
                new ServiceAccount.AuthenticationOptions
                {
                    Issuer = ZitadelDefaults.Issuer,
                });

            var client = _factory.CreateClient();
            var response = await client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, "/authed")
                {
                    Headers = { { "Authorization", $"Bearer {token}" } },
                });
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Should_asdf()
        {
            var sa = await ServiceAccount.LoadFromJsonFileAsync("./TestData/ServiceAccount.json");
            var token = await sa.AuthenticateAsync(
                ZitadelDefaults.DiscoveryEndpoint,
                new ServiceAccount.AuthenticationOptions
                {
                    Issuer = ZitadelDefaults.Issuer,
                    ProjectAudiences = { "84856448403694484" },
                });

            var client = _factory.CreateClient();
            var response = await client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, "/authed")
                {
                    Headers = { { "Authorization", $"Bearer {token}" } },
                });
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
