using Microsoft.AspNetCore.Mvc;
using SantanderTest.Contract.Services;
using SantanderTest.ViewModels;

namespace SantanderTest.Controllers;

[ApiController]
[Route("news/hackernews")]
public class HackerNewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public HackerNewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    //TODO add attributes: return type, authentication, Https, throttling 
    public async Task<IActionResult> Get(int? take, int? skip)
    {
        var news = await _newsService.GetNewsAsync(take, skip);

        //TODO introduce AutoMapper for more convenient mapping
        var response =  news.Select(n => new StoryViewModel
        {
            PostedBy = n.By,
            Score = n.Score,
            Title = n.Title,
            Uri = n.Url,
            Time = DateTimeOffset.FromUnixTimeSeconds(n.Time).LocalDateTime,
            CommentCount = n.Kids.Count
        }).AsEnumerable();

        return Ok(response);
    }
}

