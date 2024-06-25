using MarioAPI.Models;
using MarioAPI.Requests;
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

        [HttpPost]
        public async Task<IActionResult> Check(EvaluateQuestionRequest request)
        {
            try
            {
                var question = await _context.Questions
                    .FirstOrDefaultAsync(x => x.QuestionId == request.QuestionId && x.CorrectAnswerId == request.AnswerId);
                if (question != null)
                {
                    question.HasAnswer = true;
                    _context.Entry(question).State = EntityState.Modified;
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
            var question = await _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.HasAnswer == false);

            if (question?.QuestionId == _context.Questions.Count()) await Reset();

            return Ok(question);
        }

        private async Task Reset()
        {

            var questions = await _context.Questions.ToListAsync();
            foreach (var item in questions)
            {
                item.HasAnswer = false;
                _context.Entry(item).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

        }
    }
}
