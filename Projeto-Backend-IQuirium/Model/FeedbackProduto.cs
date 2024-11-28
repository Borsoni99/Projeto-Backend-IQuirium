namespace Projeto_Backend_IQuirium.Model
{
    public class FeedbackProduto
    {
        public Guid Id { get; set; }
        public Guid Id_usuario { get; set; }
        public TipoFeedbackEnum Tipo_feedback { get; set; }
        public string Conteudo { get; set; }
        public DateTime Criado_em { get; set; }
        public Usuario Usuario { get; set; }
    }
}
