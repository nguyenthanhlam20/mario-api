namespace MarioAPI.Models;

public partial class Answer
{
    public int AnswerId { get; set; }

    public string Content { get; set; } = null!;

    public int? QuestionId { get; set; }

    public virtual Question? Question { get; set; }
}
