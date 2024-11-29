//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Xunit;
//using Projeto_Backend_IQuirium.Controllers;
//using Projeto_Backend_IQuirium.Model;
//using Projeto_Backend_IQuirium.Interfaces;
//using System.Linq.Expressions;

//namespace Projeto_Backend_IQuirium.Tests.Controllers
//{
//    public class FeedbacksUsuarioControllerTests
//    {
//        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
//        private readonly FeedbacksUsuarioController _controller;

//        public FeedbacksUsuarioControllerTests()
//        {
//            _mockUnitOfWork = new Mock<IUnitOfWork>();
//            _controller = new FeedbacksUsuarioController(_mockUnitOfWork.Object);
//        }

//        [Fact]
//        public async Task GetFeedback_WithValidId_ReturnsOkResult()
//        {
//            // Arrange
//            var feedbackId = Guid.NewGuid();
//            var feedback = new FeedbackUsuario { Id = feedbackId };
//            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.GetByIdAsync(feedbackId))
//                .ReturnsAsync(feedback);

//            // Act
//            var result = await _controller.GetFeedback(feedbackId.ToString());

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnValue = Assert.IsType<FeedbackUsuario>(okResult.Value);
//            Assert.Equal(feedbackId, returnValue.Id);
//        }

//        [Fact]
//        public async Task GetFeedback_WithInvalidId_ReturnsNotFound()
//        {
//            // Arrange
//            var feedbackId = Guid.NewGuid();
//            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.GetByIdAsync(feedbackId))
//                .ReturnsAsync((FeedbackUsuario)null);

//            // Act
//            var result = await _controller.GetFeedback(feedbackId.ToString());

//            // Assert
//            Assert.IsType<NotFoundObjectResult>(result);
//        }

//        [Fact]
//        public async Task PostFeedback_WithValidData_ReturnsOkResult()
//        {
//            var remetenteId = Guid.NewGuid();
//            var destinatarioId = Guid.NewGuid();

//            var feedbackDTO = new EnviarFeedbackUsuarioDTO
//            {
//                RemetenteId = remetenteId,
//                DestinatarioId = destinatarioId,
//                Tipo = TipoFeedbackEnum.Sugestao,
//                Conteudo = "Test feedback"
//            };

//            var mockUsuarios = new Mock<IRepository<Usuario>>();
//            mockUsuarios.Setup(r => r.GetByIdAsync(remetenteId)).ReturnsAsync(new Usuario { Id = remetenteId });
//            mockUsuarios.Setup(r => r.GetByIdAsync(destinatarioId)).ReturnsAsync(new Usuario { Id = destinatarioId });
//            _mockUnitOfWork.Setup(uow => uow.Usuarios).Returns(mockUsuarios.Object);

//            var mockFeedbacks = new Mock<IRepository<FeedbackUsuario>>();
//            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios).Returns(mockFeedbacks.Object);

//            var result = await _controller.PostFeedback(feedbackDTO);

//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnValue = Assert.IsType<FeedbackUsuario>(okResult.Value);
//            Assert.Equal(feedbackDTO.RemetenteId, returnValue.RemetenteId);
//        }

//        [Fact]
//        public async Task PostFeedback_WithInvalidRemetente_ReturnsBadRequest()
//        {
//            var feedbackDTO = new EnviarFeedbackUsuarioDTO
//            {
//                RemetenteId = Guid.NewGuid(),
//                DestinatarioId = Guid.NewGuid(),
//                Tipo = TipoFeedbackEnum.Sugestao,
//                Conteudo = "Test feedback"
//            };

//            var mockUsuarios = new Mock<IRepository<Usuario>>();
//            mockUsuarios.Setup(r => r.GetByIdAsync(feedbackDTO.RemetenteId)).ReturnsAsync((Usuario)null);
//            mockUsuarios.Setup(r => r.GetByIdAsync(feedbackDTO.DestinatarioId)).ReturnsAsync(new Usuario { Id = feedbackDTO.DestinatarioId });
//            _mockUnitOfWork.Setup(uow => uow.Usuarios).Returns(mockUsuarios.Object);

//            var result = await _controller.PostFeedback(feedbackDTO);

//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal("Usuário remetente não encontrado.", badRequestResult.Value);
//        }

//        [Fact]
//        public async Task ReportarFeedback_WithValidData_ReturnsOkResult()
//        {
//            var feedbackId = Guid.NewGuid();
//            var feedback = new FeedbackUsuario { Id = feedbackId };
//            var reportDTO = new ReportarFeedbackUsuarioDTO
//            {
//                Motivo = "Test motivo",
//                Conteudo = "Test conteudo"
//            };

//            var mockFeedbacks = new Mock<IRepository<FeedbackUsuario>>();
//            mockFeedbacks.Setup(r => r.GetByIdAsync(feedbackId)).ReturnsAsync(feedback);
//            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios).Returns(mockFeedbacks.Object);

//            var result = await _controller.ReportarFeedback(feedbackId, reportDTO);

//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var dictionary = Assert.IsType<Dictionary<string, string>>(okResult.Value);
//            Assert.Equal("Feedback reportado com sucesso.", dictionary["Message"]);
//        }

//        [Fact]
//        public async Task DeleteFeedback_WithValidId_ReturnsNoContent()
//        {
//            // Arrange
//            var feedbackId = Guid.NewGuid();
//            var feedback = new FeedbackUsuario { Id = feedbackId };
//            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.GetByIdAsync(feedbackId))
//                .ReturnsAsync(feedback);

//            // Act
//            var result = await _controller.DeleteFeedback(feedbackId);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//            _mockUnitOfWork.Verify(uow => uow.FeedbackUsuarios.Delete(feedback), Times.Once);
//            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//        }

//        [Fact]
//        public async Task DeleteFeedback_WithInvalidId_ReturnsBadRequest()
//        {
//            // Act
//            var result = await _controller.DeleteFeedback(Guid.Empty);

//            // Assert
//            Assert.IsType<BadRequestObjectResult>(result);
//        }

//        [Theory]
//        [InlineData("")]
//        [InlineData(" ")]
//        [InlineData(null)]
//        public async Task GetFeedback_WithInvalidIdOrNome_ReturnsBadRequest(string idOrNome)
//        {
//            // Act
//            var result = await _controller.GetFeedback(idOrNome);

//            // Assert
//            Assert.IsType<BadRequestObjectResult>(result);
//        }

//        [Fact]
//        public async Task GetFeedback_WithValidName_ReturnsOkResult()
//        {
//            // Arrange
//            var name = "TestUser";
//            var feedback = new FeedbackUsuario { Id = Guid.NewGuid() };
//            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.FindAsync(It.IsAny<Expression<Func<FeedbackUsuario, bool>>>()))
//                .ReturnsAsync(new List<FeedbackUsuario> { feedback });

//            // Act
//            var result = await _controller.GetFeedback(name);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.IsType<FeedbackUsuario>(okResult.Value);
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Projeto_Backend_IQuirium.Controllers;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Interfaces;
using System.Linq.Expressions;

namespace Projeto_Backend_IQuirium.Tests.Controllers
{
    public class FeedbacksUsuarioControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly FeedbacksUsuarioController _controller;

        public FeedbacksUsuarioControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new FeedbacksUsuarioController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetFeedback_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedback = new FeedbackUsuario { Id = feedbackId };
            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.GetByIdAsync(feedbackId))
                .ReturnsAsync(feedback);

            // Act
            var result = await _controller.GetFeedback(feedbackId.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeedbackUsuarioResponseDTO>(okResult.Value);
            Assert.Equal(feedbackId, returnValue.Id);
        }

        [Fact]
        public async Task GetFeedback_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.GetByIdAsync(feedbackId))
                .ReturnsAsync((FeedbackUsuario)null);

            // Act
            var result = await _controller.GetFeedback(feedbackId.ToString());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PostFeedback_WithValidData_ReturnsOkResult()
        {
            var remetenteId = Guid.NewGuid();
            var destinatarioId = Guid.NewGuid();

            var feedbackDTO = new EnviarFeedbackUsuarioDTO
            {
                RemetenteId = remetenteId,
                DestinatarioId = destinatarioId,
                Tipo = TipoFeedbackEnum.Sugestao,
                Conteudo = "Test feedback"
            };

            var mockUsuarios = new Mock<IRepository<Usuario>>();
            mockUsuarios.Setup(r => r.GetByIdAsync(remetenteId))
                .ReturnsAsync(new Usuario { Id = remetenteId, Nome = "Remetente Test" });
            mockUsuarios.Setup(r => r.GetByIdAsync(destinatarioId))
                .ReturnsAsync(new Usuario { Id = destinatarioId, Nome = "Destinatario Test" });
            _mockUnitOfWork.Setup(uow => uow.Usuarios).Returns(mockUsuarios.Object);

            var mockFeedbacks = new Mock<IRepository<FeedbackUsuario>>();
            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios).Returns(mockFeedbacks.Object);

            var result = await _controller.PostFeedback(feedbackDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeedbackUsuarioResponseDTO>(okResult.Value);
            Assert.Equal(feedbackDTO.RemetenteId, returnValue.RemetenteId);
        }

        [Fact]
        public async Task PostFeedback_WithInvalidRemetente_ReturnsBadRequest()
        {
            var feedbackDTO = new EnviarFeedbackUsuarioDTO
            {
                RemetenteId = Guid.NewGuid(),
                DestinatarioId = Guid.NewGuid(),
                Tipo = TipoFeedbackEnum.Sugestao,
                Conteudo = "Test feedback"
            };

            var mockUsuarios = new Mock<IRepository<Usuario>>();
            mockUsuarios.Setup(r => r.GetByIdAsync(feedbackDTO.RemetenteId)).ReturnsAsync((Usuario)null);
            mockUsuarios.Setup(r => r.GetByIdAsync(feedbackDTO.DestinatarioId))
                .ReturnsAsync(new Usuario { Id = feedbackDTO.DestinatarioId });
            _mockUnitOfWork.Setup(uow => uow.Usuarios).Returns(mockUsuarios.Object);

            var result = await _controller.PostFeedback(feedbackDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário remetente não encontrado.", badRequestResult.Value);
        }

        [Fact]
        public async Task ReportarFeedback_WithValidData_ReturnsOkResult()
        {
            var feedbackId = Guid.NewGuid();
            var feedback = new FeedbackUsuario { Id = feedbackId };
            var reportDTO = new ReportarFeedbackUsuarioDTO
            {
                Motivo = "Test motivo",
                Conteudo = "Test conteudo"
            };

            var mockFeedbacks = new Mock<IRepository<FeedbackUsuario>>();
            mockFeedbacks.Setup(r => r.GetByIdAsync(feedbackId)).ReturnsAsync(feedback);
            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios).Returns(mockFeedbacks.Object);

            var result = await _controller.ReportarFeedback(feedbackId, reportDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeedbackUsuarioResponseDTO>(okResult.Value);
            Assert.Equal(feedbackId, returnValue.Id);
        }

        [Fact]
        public async Task DeleteFeedback_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedback = new FeedbackUsuario { Id = feedbackId };
            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.GetByIdAsync(feedbackId))
                .ReturnsAsync(feedback);

            // Act
            var result = await _controller.DeleteFeedback(feedbackId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockUnitOfWork.Verify(uow => uow.FeedbackUsuarios.Delete(feedback), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteFeedback_WithInvalidId_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.DeleteFeedback(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetFeedback_WithInvalidIdOrNome_ReturnsBadRequest(string idOrNome)
        {
            // Act
            var result = await _controller.GetFeedback(idOrNome);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetFeedback_WithValidName_ReturnsOkResult()
        {
            // Arrange
            var name = "TestUser";
            var feedback = new FeedbackUsuario { Id = Guid.NewGuid() };
            _mockUnitOfWork.Setup(uow => uow.FeedbackUsuarios.FindAsync(It.IsAny<Expression<Func<FeedbackUsuario, bool>>>()))
                .ReturnsAsync(new List<FeedbackUsuario> { feedback });

            // Act
            var result = await _controller.GetFeedback(name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeedbackUsuarioResponseDTO>(okResult.Value);
            Assert.NotNull(returnValue);
        }
    }
}