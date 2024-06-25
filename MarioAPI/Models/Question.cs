namespace MarioAPI.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string Title { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int? CorrectAnswerId { get; set; }

    public bool? HasAnswer { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
