using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Project.Models;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Project.Hubs
{
    // step1. notification hub
    public class NotificationHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;

        public NotificationHub(IUserRepository userRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        [Authorize(Policy = "Policy")]
        public async Task SendMessageToUser(string description, int questionId)
        {
           
            var user = await _userRepository.GetUserByUsernameAsync(Context.User.Identity.Name);
            if (user == null)
            {
                await Clients.Caller.SendAsync("Error", "User not found.");
                return;
            }

            // 3. Get the question by id
            var question = await _questionRepository.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                await Clients.Caller.SendAsync("Error", "Question not found.");
                return;
            }

            // 4. Create a new Answer
            var answer = new Answer
            {
                Description = description,
                CreatedAt = DateTime.Now,
                QuestionId = questionId,
                AppUser = user,   // Assuming navigation property exists
                UserId = user.Id,
                Question = question
            };

            // 5. Save the answer
            await _answerRepository.AddAnswerAsync(answer);
            await _userRepository.UpdateRatingAsync();

            // 6. Notify all clients about the new answer
            await Clients.All.SendAsync("ReceiveNotification", user.UserName, questionId, description, answer.Id);
        }

        public async Task NotifyQuestionDeleted(int questionId)
        {
            await Clients.All.SendAsync("QuestionDeleted", questionId);
        }

        public async Task DeleteAnswer(int answerId)
        {
            var answer = await _answerRepository.GetAnswerByIdAsync(answerId);
            if (answer == null)
            {
                await Clients.Caller.SendAsync("Error", "Answer not found.");
                return;
            }

            await _answerRepository.DeleteAnswerAsync(answerId);

            // Notify all clients to remove from DOM
            await Clients.All.SendAsync("AnswerDeleted", answerId);
        }

    }
}

