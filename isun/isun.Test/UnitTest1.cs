using FluentAssertions;
using isun.Models.ResponseModels;
using isun.Services;
using Moq;
using System.Net;
using System.Text.Json;

namespace isun.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Returns_Response_Result_Plus_One3Good()
        {
            //Arrange
            var fakeBaseAddress = "https://weather-api.isun.ch/api/";

            var authResp = JsonSerializer.Deserialize<AuthResponse>("{\"token\":\"7cda42c91b0d47ad3df0b7bee41b45079b02475a5940f4309cd4e592b50ef27c\"}");

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler
                .SetupSendAsync(HttpMethod.Post, $"{fakeBaseAddress}/authorize")
                .ReturnsHttpResponseAsync(authResp, HttpStatusCode.OK);

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri(fakeBaseAddress)
            };

            var client = new WeatherServiceProcessor(httpClient);

            //Act
            var result = await client.Authorize();

            //Assert
            result.Object.Should().Be(authResp);
        }

        [Test]
        public async Task Returns_Response_Result_Plus_One3_CitiesGood()
        {
            //Arrange
            var fakeBaseAddress = "https://weather-api.isun.ch/api/";
            var respJson = "{\"city\":\"Vilnius\",\"temperature\":-19,\"precipitation\":89,\"windSpeed\":7,\"summary\":\"Chilly\"}";
            var weatherResp = JsonSerializer.Deserialize<WeatherResponse>(respJson);

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler
                .SetupSendAsync(HttpMethod.Get, $"{fakeBaseAddress}weathers/Vilnius")
                .ReturnsHttpResponseAsync(weatherResp, HttpStatusCode.OK);

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri(fakeBaseAddress)
            };

            var client = new WeatherServiceProcessor(httpClient);

            //Act
            var result = await client.GetWeather("Vilnius");

            //Assert
            result.Object.Should().Be(weatherResp);
        }
    }
}