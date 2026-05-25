using mabyWorking.Data;
using mabyWorking.Data.Identity;
using mabyWorking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace mabyWorking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrgTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ILogger<OrgTestController> _logger;

        public OrgTestController(ApplicationDbContext context, UserManager<ApplicationIdentityUser> userManager, ILogger<OrgTestController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("can-start")]
        public async Task<IActionResult> CanStartTest()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(new { CanStart = false, Message = "Пользователь не найден" });

            var stats = await _context.Stats.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (stats == null) return Unauthorized(new { CanStart = false, Message = "Статистика пользователя не найдена" });

            if (stats.QuizLimit <= stats.QuizPassed)
            {
                return Ok(new { CanStart = false, Message = "Лимит попыток на сегодня исчерпан." });
            }

            return Ok(new { CanStart = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetTests()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized("Пользователь не найден");

            var userStatus = await _context.Stats.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (userStatus == null) return NotFound("Статус пользователя не найден");

            var tests = await _context.OrgTests.ToListAsync();

            var result = tests.Select(t => new
            {
                name = t.Name,
                isCompleted = _context.OrgTestStats.Any(ts =>
                    ts.StatsId == userStatus.Id &&
                    ts.OrgTestId == t.Id &&
                    ts.IsPassed)
            }).ToList();

            return Ok(result);
        }

    [HttpGet("{testName}")]
        public async Task<IActionResult> GetTest(string testName)
        {
            var test = await _context.OrgTests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(t => t.Name == testName);

            if (test == null) return NotFound("Тест не найден.");

            // Берем 3 случайных вопроса
            var response = test.Questions.OrderBy(_ => Guid.NewGuid()).Take(3).Select(q => new
            {
                id = q.Id,
                description = q.Description,
                explanation = q.Explanation,
                rewardRings = q.RewardRings,
                rewardXp = q.RewardXp,
                answers = q.Answers.Select(a => new
                {
                    id = a.Id,
                    text = a.Text
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpPost("check-answer")]
        public async Task<IActionResult> CheckAnswer([FromBody] CheckAnswerDto request)
        {
            var answerIds = request.AnswerIds;
            if (answerIds == null || !answerIds.Any()) return BadRequest("Пустой ответ.");

            var question = await _context.OrgTestQuestions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == request.QuestionId);

            if (question == null) return NotFound("Вопрос не найден.");

            var correctAnswers = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToList();

            var isCorrect = correctAnswers.Count == answerIds.Count && answerIds.All(id => correctAnswers.Contains(id));

            return Ok(new { IsCorrect = isCorrect, Explanation = isCorrect ? question.Explanation : null });
        }

        [HttpPost("complete-test")]
        public async Task<IActionResult> CompleteTest([FromBody] CompleteTestDto request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var userStatus = await _context.Stats.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (userStatus == null) return NotFound("Статус пользователя не найден");

            var test = await _context.OrgTests.FirstOrDefaultAsync(t => t.Name == request.TestName);
            if (test == null) return NotFound("Тест не найден");

            var testStats = await _context.OrgTestStats.FirstOrDefaultAsync(ts => ts.StatsId == userStatus.Id && ts.OrgTestId == test.Id);
            if (testStats == null)
            {
                testStats = new OrgTestStats { StatsId = userStatus.Id, OrgTestId = test.Id, IsPassed = false };
                _context.OrgTestStats.Add(testStats);
            }

            var lastQuestion = await _context.OrgTestQuestions
                .Where(q => q.OrgTestId == test.Id)
                .OrderByDescending(q => q.Id)
                .FirstOrDefaultAsync();

            int rewardXp = lastQuestion?.RewardXp ?? 10;
            int rewardRings = lastQuestion?.RewardRings ?? 5;

            // Как в квизах: для прохождения нужно >= 2 ответов (т.к. 3 вопроса всего)
            bool isPassed = request.CorrectAnswersCount >= 2;
            bool isFstTime = !testStats.IsPassed && isPassed;
            
            double bonus = isFstTime ? 1.5 : 1.0;
            
            int totalXP = (int)(rewardXp * request.CorrectAnswersCount * bonus);
            int totalRings = (int)(rewardRings * request.CorrectAnswersCount * bonus);

            userStatus.Xp += totalXP;
            userStatus.Balance += totalRings;

            if (isFstTime)
            {
                testStats.IsPassed = true;
            }

            userStatus.QuizPassed++;

            var newStatus = await _context.Statuses
                .Where(s => s.MinXp <= userStatus.Xp)
                .OrderByDescending(s => s.MinXp)
                .FirstOrDefaultAsync();

            if (newStatus != null && userStatus.StatusId != newStatus.Id)
                userStatus.StatusId = newStatus.Id;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Name = request.TestName,
                isFstTime = isFstTime,
                isPassed = isPassed,
                totalXP = totalXP,
                totalRings = totalRings
            });
        }
    }

    public class CheckAnswerDto
    {
        public long QuestionId { get; set; }
        public List<long> AnswerIds { get; set; } = new List<long>();
    }

    public class CompleteTestDto
    {
        public string TestName { get; set; } = string.Empty;
        public int CorrectAnswersCount { get; set; }
    }
}
