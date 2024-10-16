using Microsoft.AspNetCore.Mvc;
using Moq;
using SantanderTest.Contract.Entities;
using SantanderTest.Contract.Services;
using SantanderTest.Controllers;
using SantanderTest.ViewModels;
using Xunit;

public class HackerNewsControllerTests
{
    private readonly Mock<INewsService> _mockNewsService;
    private readonly HackerNewsController _controller;

    public HackerNewsControllerTests()
    {
        _mockNewsService = new Mock<INewsService>();
        _controller = new HackerNewsController(_mockNewsService.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithExpectedResponse()
    {
        // Arrange
        var expectedNews = new List<Story>
        {
            new Story
            {
                By = "author1",
                Score = 100,
                Title = "Title1",
                Url = "http://example.com/1",
                Time = 1625250000,
                Kids = new List<int> { 1, 2, 3 }
            },
            new Story
            {
                By = "author2",
                Score = 200,
                Title = "Title2",
                Url = "http://example.com/2",
                Time = 1625250100,
                Kids = new List<int> { 4, 5 }
            }
        };

        _mockNewsService.Setup(s => s.GetNewsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                        .ReturnsAsync(expectedNews);

        // Act
        var result = await _controller.Get(null, null) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var response = (IEnumerable<StoryViewModel>)result.Value;

        Assert.Equal(2, response.Count());
        Assert.Equal("author1", response.First().PostedBy);
        Assert.Equal(100, response.First().Score);
        Assert.Equal("Title1", response.First().Title);
        Assert.Equal("http://example.com/1", response.First().Uri);
        Assert.Equal(3, response.First().CommentCount);

        //Same checks for the 2nd item
    }
}