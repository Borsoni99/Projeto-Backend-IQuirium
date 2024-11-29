using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Projeto_Backend_IQuirium.Tests.Repository
{
    public class FeedbackUsuarioRepositoryTests
    {
        private readonly ProjetoBackendIQuiriumContext _context;
        private readonly FeedbackUsuarioRepository _repository;

        public FeedbackUsuarioRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProjetoBackendIQuiriumContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProjetoBackendIQuiriumContext(options);
            _repository = new FeedbackUsuarioRepository(_context);
        }

        [Fact]
        public async Task GetByIdAsync_RetornaFeedbackUsuario_QuandoIdExiste()
        {
            // Arrange
            var feedback = new FeedbackUsuario
            {
                Id = Guid.NewGuid(),
                // Inicialize outras propriedades conforme necessário
            };
            _context.FeedbacksUsuarios.Add(feedback);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repository.GetByIdAsync(feedback.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(feedback.Id, resultado.Id);
        }

        [Fact]
        public async Task GetByIdAsync_RetornaNull_QuandoIdNaoExiste()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var resultado = await _repository.GetByIdAsync(idInexistente);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task GetAllAsync_RetornaTodosFeedbacksUsuarios()
        {
            // Arrange
            var feedback1 = new FeedbackUsuario { Id = Guid.NewGuid() /*, outras propriedades */ };
            var feedback2 = new FeedbackUsuario { Id = Guid.NewGuid() /*, outras propriedades */ };
            _context.FeedbacksUsuarios.AddRange(feedback1, feedback2);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task FindAsync_RetornaFeedbacksQueSatisfazemPredicado()
        {
            // Arrange
            var feedback1 = new FeedbackUsuario { Id = Guid.NewGuid(), /* outras propriedades */ };
            var feedback2 = new FeedbackUsuario { Id = Guid.NewGuid(), /* outras propriedades */ };
            _context.FeedbacksUsuarios.AddRange(feedback1, feedback2);
            await _context.SaveChangesAsync();

            // Define um predicado que corresponde a feedback1
            Expression<Func<FeedbackUsuario, bool>> predicado = f => f.Id == feedback1.Id;

            // Act
            var resultado = await _repository.FindAsync(predicado);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(feedback1.Id, resultado.First().Id);
        }
    }
}
