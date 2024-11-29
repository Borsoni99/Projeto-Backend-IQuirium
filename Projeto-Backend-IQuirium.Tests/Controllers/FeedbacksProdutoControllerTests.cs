using Microsoft.AspNetCore.Mvc;
using Moq;
using Projeto_Backend_IQuirium.Controllers;
using Projeto_Backend_IQuirium.Interfaces;
using Projeto_Backend_IQuirium.Model;
using System;
using System.Net;
using System.Threading.Tasks;


namespace Projeto_Backend_IQuirium.Tests.Controllers
{
    public class FeedbacksProdutoControllerTests
    {
        [Fact]
        public async Task ShouldGetFeedbackByIdSuccessfully()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var id = Guid.NewGuid();

            mockUnitOfWork.Setup(x => x.FeedbackProdutos.GetByIdAsync(id)).ReturnsAsync(
                new FeedbackProduto
                {
                    Id = id,
                    Id_usuario = Guid.NewGuid(),
                    Tipo_feedback = TipoFeedbackEnum.Comentario,
                    Conteudo = "Test feedback",
                    Criado_em = DateTime.UtcNow
                });

            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);

            // Act
            var result = await controller.GetFeedback(id.ToString());

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeedbackProduto>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public static async Task ShouldReturnNotFoundWhenFeedbackIdDoesNotExist()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var id = Guid.NewGuid();
        
            mockUnitOfWork.Setup(x => x.FeedbackProdutos.GetByIdAsync(id))
                .ReturnsAsync((FeedbackProduto?)null);
        
            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);
        
            var result = await controller.GetFeedback(id.ToString());
        
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task ShouldPostFeedbackSuccessfully()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var feedbackDto = new EnviarFeedbackDTO
            {
                Id_usuario = Guid.NewGuid(),
                Tipo_feedback = TipoFeedbackEnum.Sugestao,
                Conteudo = "Test feedback content"
            };

            mockUnitOfWork.Setup(x => x.FeedbackProdutos.AddAsync(It.IsAny<FeedbackProduto>()))
                .Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);

            // Act
            var result = await controller.PostFeedback(feedbackDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeedbackProduto>(okResult.Value);
            Assert.Equal(feedbackDto.Id_usuario, returnValue.Id_usuario);
            Assert.Equal(feedbackDto.Tipo_feedback, returnValue.Tipo_feedback);
            Assert.Equal(feedbackDto.Conteudo, returnValue.Conteudo);
        }

        [Fact]
        public async Task ShouldDeleteFeedbackSuccessfully()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var id = Guid.NewGuid();

            mockUnitOfWork.Setup(x => x.FeedbackProdutos.GetByIdAsync(id))
                .ReturnsAsync(new FeedbackProduto
                {
                    Id = id,
                    Id_usuario = Guid.NewGuid(),
                    Tipo_feedback = TipoFeedbackEnum.Comentario,
                    Conteudo = "Test feedback",
                    Criado_em = DateTime.UtcNow
                });

            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);

            // Act
            var result = await controller.DeleteFeedback(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentFeedback()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var id = Guid.NewGuid();

            mockUnitOfWork.Setup(x => x.FeedbackProdutos.GetByIdAsync(id))
                .ReturnsAsync((FeedbackProduto?)null);

            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);

            var result = await controller.DeleteFeedback(id);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task ShouldReturnBadRequestWhenDeletingWithEmptyGuid()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);

            // Act
            var result = await controller.DeleteFeedback(Guid.Empty);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenGettingFeedbackWithEmptyIdOrName()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var controller = new FeedbacksProdutoController(mockUnitOfWork.Object);

            // Act
            var result = await controller.GetFeedback("");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }
    }
}
