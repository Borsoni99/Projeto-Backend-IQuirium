namespace Projeto_Backend_IQuirium.Model
{
    public class StatusFeedback
    {
        public Guid Id { get; set; }
        public Guid Id_feedback { get; set; }
        public string Status { get; set; }
        public DateTime Atualizado_em { get; set; }
        public Feedback Feedback { get; set; }

    }
}
