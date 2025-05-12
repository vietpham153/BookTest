using System.Diagnostics;
using System.Threading.Tasks;
using BookTest.Models;
using BookTest.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sTerm ="",int? genreId = null)
        {
            if (string.IsNullOrWhiteSpace(sTerm) && genreId == null && Request.QueryString.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            BookDisplayModel bookModel = new()
            {
                Books = await _homeRepository.GetBooks(sTerm, genreId),
                Genres = await _homeRepository.Genres(),
                STerm = sTerm,
                GenreId = genreId
            };
            
            return View(bookModel);
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
    }
}
