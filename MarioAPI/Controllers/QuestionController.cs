using MarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarioAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class QuestionController : ControllerBase
    {
        private readonly MarioContext _context;
        public QuestionController(MarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Questions.Include(x => x.Answers).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Reset()
        {
            try
            {
                var questions = await _context.Questions.ToListAsync();
                foreach (var item in questions)
                {
                    item.HasAnswer = false;
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Check(int questionId, int selectedAnswer)
        {
            try
            {
                var question = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == questionId && x.CorrectAnswerId == selectedAnswer);
                if (question != null)
                {
                    question.HasAnswer = true;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception) { }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Random()
        {
            Question? question = null;
            var maximum = _context.Questions.Count();
            do
            {
                var random = new Random(maximum).Next();
                question = await _context.Questions
                    .Include(x => x.Answers)
                    .Where(x => x.QuestionId == random)
                    .FirstOrDefaultAsync();
            } while (question == null);

            return Ok(question);
        }
    }
}
