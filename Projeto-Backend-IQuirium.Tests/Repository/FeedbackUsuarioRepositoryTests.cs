using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;
using Xunit;

namespace Projeto_Backend_IQuirium.Tests
{
    public class FeedbackUsuarioRepositoryTests
    {
        private DbContextOptions<ProjetoBackendIQuiriumContext> _options;

        public FeedbackUsuarioRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ProjetoBackendIQuiriumContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usa um novo banco para cada teste
                .Options;
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarFeedbackComIdEspecifico()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var remetente = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Remetente",
                Email = "remetente@example.com"
            };
            var destinatario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Destinatário",
                Email = "destinatario@example.com"
            };

            var feedback = new FeedbackUsuario
            {
                Id = feedbackId,
                Remetente = remetente,
                Destinatario = destinatario,
                ConteudoFeedback = "Ótimo trabalho!",
                ConteudoReport = "Nenhum problema.",
                Motivo = "Avaliação de desempenho"
            };

            using (var context = new ProjetoBackendIQuiriumContext(_options))
            {
                context.Usuarios.Add(remetente);
                context.Usuarios.Add(destinatario);
                context.FeedbacksUsuarios.Add(feedback);
                await context.SaveChangesAsync();
            }

            // Act
            FeedbackUsuario resultado;
            using (var context = new ProjetoBackendIQuiriumContext(_options))
            {
                var repository = new FeedbackUsuarioRepository(context);
                resultado = await repository.GetByIdAsync(feedbackId);
            }

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(feedbackId, resultado.Id);
            Assert.Equal("Remetente", resultado.Remetente.Nome);
            Assert.Equal("Destinatário", resultado.Destinatario.Nome);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosOsFeedbacks()
        {
            // Arrange
            var remetente1 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Remetente1",
                Email = "remetente1@example.com"
            };
            var destinatario1 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Destinatário1",
                Email = "destinatario1@example.com"
            };
            var remetente2 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Remetente2",
                Email = "remetente2@example.com"
            };
            var destinatario2 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Destinatário2",
                Email = "destinatario2@example.com"
            };

            var feedbacks = new List<FeedbackUsuario>
            {
                new FeedbackUsuario
                {
                    Id = Guid.NewGuid(),
                    Remetente = remetente1,
                    Destinatario = destinatario1,
                    ConteudoFeedback = "Bom trabalho.",
                    ConteudoReport = "Sem problemas.",
                    Motivo = "Revisão mensal"
                },
                new FeedbackUsuario
                {
                    Id = Guid.NewGuid(),
                    Remetente = remetente2,
                    Destinatario = destinatario2,
                    ConteudoFeedback = "Precisa melhorar.",
                    ConteudoReport = "Atrasos frequentes.",
                    Motivo = "Feedback trimestral"
                }
            };

            using (var context = new ProjetoBackendIQuiriumContext(_options))
            {
                context.Usuarios.AddRange(remetente1, destinatario1, remetente2, destinatario2);
                context.FeedbacksUsuarios.AddRange(feedbacks);
                await context.SaveChangesAsync();
            }

            // Act
            IEnumerable<FeedbackUsuario> resultado;
            using (var context = new ProjetoBackendIQuiriumContext(_options))
            {
                var repository = new FeedbackUsuarioRepository(context);
                resultado = await repository.GetAllAsync();
            }

            // Assert
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task FindAsync_DeveRetornarFeedbacksQueAtendemAoPredicado()
        {
            // Arrange
            var destinatarioId = Guid.NewGuid();
            var remetente1 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Remetente1",
                Email = "remetente1@example.com"
            };
            var destinatario1 = new Usuario
            {
                Id = destinatarioId,
                Nome = "Destinatário1",
                Email = "destinatario1@example.com"
            };
            var remetente2 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Remetente2",
                Email = "remetente2@example.com"
            };
            var destinatario2 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Destinatário2",
                Email = "destinatario2@example.com"
            };

            var feedbacks = new List<FeedbackUsuario>
            {
                new FeedbackUsuario
                {
                    Id = Guid.NewGuid(),
                    Remetente = remetente1,
                    Destinatario = destinatario1,
                    ConteudoFeedback = "Excelente!",
                    ConteudoReport = "Nenhum problema.",
                    Motivo = "Feedback anual"
                },
                new FeedbackUsuario
                {
                    Id = Guid.NewGuid(),
                    Remetente = remetente2,
                    Destinatario = destinatario2,
                    ConteudoFeedback = "Bom desempenho.",
                    ConteudoReport = "Alguns atrasos.",
                    Motivo = "Revisão semestral"
                }
            };

            using (var context = new ProjetoBackendIQuiriumContext(_options))
            {
                context.Usuarios.AddRange(remetente1, destinatario1, remetente2, destinatario2);
                context.FeedbacksUsuarios.AddRange(feedbacks);
                await context.SaveChangesAsync();
            }

            // Act
            IEnumerable<FeedbackUsuario> resultado;
            using (var context = new ProjetoBackendIQuiriumContext(_options))
            {
                var repository = new FeedbackUsuarioRepository(context);
                resultado = await repository.FindAsync(f => f.Destinatario.Id == destinatarioId);
            }

            // Assert
            Assert.Single(resultado);
            Assert.Equal(destinatarioId, resultado.First().Destinatario.Id);
        }
    }
}
