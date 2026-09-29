public class Question
{

    public int QuestionId { get; set; } // PRIMARY_KEY
    public string Prompt { get; set; }
    public string Options { get; set; } // poss used for text of questions (contains multiple paragraphs with labeling)
    public int CorrectAnswer { get; set; } // poss used to ID correct answer of questions (contains single-char/num label to match the correct option)
    public int PassageId { get; set; } // FOREIGN_KEY(1 or more questions per passage)
}
