using AppStoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GetNewGames()
        {
            var model = await FetchNewGamesFromiTunes();
            var gameViewModel = new GameViewModel { Games = model };
            return PartialView("_GamesPartial", gameViewModel);
        }

        private async Task<List<GameInfo>> FetchNewGamesFromiTunes()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync("https://itunes.apple.com/us/rss/newapplications/limit=10/json");
                    var result = JsonConvert.DeserializeObject<iTunesApiResponse>(response);

                    var newGames = new List<GameInfo>();

                    if (result != null && result.feed != null && result.feed.entry != null)
                    {
                        foreach (var entry in result.feed.entry)
                        {
                            try
                            {
                                var gameInfo = new GameInfo
                                {
                                    Name = entry?.title?.label ?? "N/A",
                                    ReleaseDate = entry?.imReleaseDate?.attributes?.label ?? "N/A",
                                    Genre = entry?.category?.attributes?.term ?? "N/A",
                                    AppStoreLink = entry?.link?.attributes?.href ?? "N/A"
                                };
                                newGames.Add(gameInfo);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error parsing game info: {ex}");
                            }
                        }
                    }

                    return newGames;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching new games from iTunes API: {ex}");
                    return new List<GameInfo>();
                }
            }
        }
    }
}
