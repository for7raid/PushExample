using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PushExample.Abstraction;
using PushExample.Models;
using PushExample.Implementation;
using System.Diagnostics;

namespace PushExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _messageService;

        public HomeController(ILogger<HomeController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        [HttpPost, Route("/")]
        public async Task<IActionResult> SendMessageAsync(MessageBase message)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            await _messageService.QueueMessageAsync(message);
            return View("Index", message);
        }

        [HttpPost]
        public async Task<IActionResult> GetMessageStateAsync(Guid id)
        {
            var state = await _messageService.GetStateAsync(id);
            ViewData["State"] = state;
            return View("Index");
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