﻿namespace Projeto_Backend_IQuirium.Model
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime Criado_em { get; set; }


        // Propriedades de navegação
        public ICollection<FeedbackUsuario> FeedbacksEnviados { get; set; }
        public ICollection<FeedbackUsuario> FeedbacksRecebidos { get; set; }

    }
}
