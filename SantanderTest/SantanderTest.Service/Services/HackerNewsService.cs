using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SantanderTest.Contract.Entities;
using SantanderTest.Contract.Infrastructure;
using SantanderTest.Contract.Services;

namespace SantanderTest.Service.Services
{
    public class HackerNewsService : BaseService, INewsService
    {
        private const string BaseUrl = "https://hacker-news.firebaseio.com/v0";
        private const short MaxRetries = 3;
        private const short DelayMilliseconds = 500;
        private readonly HttpClient _httpClient;
        private readonly ICacheService<int, Story> _cacheService;
        private readonly ILogger<HackerNewsService> _logger;

        public HackerNewsService(HttpClient httpClient,
            ICacheService<int, Story> cacheService,
            ILogger<HackerNewsService> logger)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<IEnumerable<Story>> GetNewsAsync(int? take, int? skip)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/beststories.json");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var storyIds = JsonSerializer.Deserialize<IEnumerable<int>>(content);

                if (storyIds != null && storyIds.Any())
                {
                    var tasks = Paginate(take, skip, ref storyIds)
                        .Select(FetchStoryAsync)
                        .ToList();

                    var stories = await Task.WhenAll(tasks);
                    return stories.OrderByDescending(s => s.Score);
                }

                return Enumerable.Empty<Story>();
            }
            else
            {
                throw new Exception($"Unable to get beststories. HackerNews server Http Response is {response.StatusCode}.");
            }
        }

        /// <summary>
        /// Get Story description by Id
        /// 1. Using cache if story was previously donwloaded
        /// 2. In case of error attempt to retry. Could be a temrory issue on HackerNews, or TooManyReqeusts (429)
        /// 3. In case of unsuccessul download return default story with story Id only
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        private async Task<Story> FetchStoryAsync(int storyId)
        {
            if (_cacheService.ContainsKey(storyId))
                return _cacheService.Get(storyId);

            var story = new Story
            {
                Id = storyId
            };

            for (int attempt = 0; attempt < MaxRetries; attempt++)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{BaseUrl}/item/{storyId}.json");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var contetntStory = JsonSerializer.Deserialize<Story>(content, CamelCaseJsonSerializerOption);

                        if (contetntStory != null)
                        {
                            story = contetntStory;
                            _cacheService.Set(storyId, contetntStory);
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unable to get story description by Id={storyId}. Attempt {attempt + 1} failed");
                }
                // Delay before retrying
                if (attempt < MaxRetries - 1) // Avoid waiting after the last attempt
                {
                    await Task.Delay(DelayMilliseconds);
                }
            }

            return story;
        }

        private static JsonSerializerOptions CamelCaseJsonSerializerOption => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}

