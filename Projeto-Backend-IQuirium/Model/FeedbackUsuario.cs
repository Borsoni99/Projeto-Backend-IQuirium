namespace Projeto_Backend_IQuirium.Model
{
    public class FeedbackUsuario
    {
        public Guid Id { get; set; }
        public Guid RemetenteId { get; set; }
        public Guid DestinatarioId { get; set; }
        public TipoFeedbackEnum Tipo { get; set; } 
        public string ConteudoFeedback { get; set; }
        public DateTime DataHoraEnvio { get; set; }
        public StatusFeedbackEnum Status { get; set; } 
        public string Motivo { get; set; } // Motivo de report, se aplicável
        public string? ConteudoReport { get; set; }
        public Usuario Remetente { get; set; }
        public Usuario Destinatario { get; set; }
    }
}
