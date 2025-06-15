using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Data;
using Microsoft.EntityFrameworkCore;
using Project.Models.Repositories;
using Project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Project.Models.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;

namespace Project.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository qr;
        private readonly IAnswerRepository ar;
        private readonly IUserRepository ur;

        // step4.  hub context
        private readonly IHubContext<NotificationHub> _hubContext;

        public QuestionController(IQuestionRepository q, IAnswerRepository ar, IUserRepository u, IHubContext<NotificationHub> hubContext)
        {
            qr = q;
            this.ar = ar;
            ur = u;
            _hubContext = hubContext;
        }


        public async Task<IActionResult> Index(string? tagName, string? user, string? ansUser)
        {
            List<Question> questions;

            if (!string.IsNullOrEmpty(tagName))
            {
                questions = await qr.GetQuestionsByTagNameAsync(tagName); // You filter here
            }
            else if(!string.IsNullOrEmpty(user))
            {
                questions = await qr.GetQuestionsByUserAsync(user);
            }
            else if (!string.IsNullOrEmpty(ansUser))
            {
                questions = await qr.GetQuestionsByAnsUserAsync(ansUser);
            }
            else
            {
                questions = await qr.GetAllQuestionsAsync(); // Show all if no tag
            }

            return View(questions);
        }



        [Authorize(Policy = "Policy")]
        public async Task<IActionResult> Ask()
        {
            var t = await qr.getAllTagsAsync();
            return View(t);
        }

        [HttpPost]
        [Authorize(Policy = "Policy")]
        public async Task<IActionResult> Asked(string description, List<string> tags)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError("Description", "Description is required.");
                var t = qr.getAllTagsAsync();
                return View("Ask", t);
            }

            var AppUser = await ur.GetUserByUsernameAsync(User.Identity.Name);
            if (AppUser == null)
            {
                return RedirectToPage("/Account/Login", "Identity");
            }

            // Fetch Tag entities from DB matching selected tags
            var tagEntities = await qr.GetTagsByNameAsync(tags);

            var question = new Question
            {
                Description = description,
                UserId = AppUser.Id,
                CreatedAt = DateTime.Now,
                Tags = tagEntities
            };

            await qr.AddQuestionAsync(question);
            await ur.UpdateRatingAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Show(int id) 
        {
            var question = await qr.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await qr.DeleteQuestionAsync(id);

            await _hubContext.Clients.All.SendAsync("QuestionDeleted", id);

            return RedirectToAction("Index");
        }

    }
}
