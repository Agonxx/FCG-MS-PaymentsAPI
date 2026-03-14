using Moq;
using PaymentsAPI.Application.Services;
using PaymentsAPI.Domain.Entities;
using PaymentsAPI.Domain.Interfaces.Repositories;

namespace PaymentsAPI.Tests.Services
{
    public class PagamentoServiceTests
    {
        private readonly Mock<IPagamentoRepository> _repoMock;
        private readonly PagamentoService _service;

        public PagamentoServiceTests()
        {
            _repoMock = new Mock<IPagamentoRepository>();
            _service = new PagamentoService(_repoMock.Object);
        }

        [Fact]
        public async Task ProcessarAsync_DeveSempreRetornarApproved()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _repoMock.Setup(r => r.Registrar(It.IsAny<Pagamento>())).ReturnsAsync(true);

            // Act
            var resultado = await _service.ProcessarAsync(orderId, 1, 5, "Elden Ring", 249.90m);

            // Assert
            Assert.Equal("Approved", resultado.Status);
        }

        [Fact]
        public async Task ProcessarAsync_DeveRegistrarPagamentoComDadosCorretos()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = 2;
            var jogoId = 5;
            var titulo = "EA FC 26";
            var preco = 299.90m;

            Pagamento? pagamentoRegistrado = null;

            _repoMock.Setup(r => r.Registrar(It.IsAny<Pagamento>()))
                .Callback<Pagamento>(p => pagamentoRegistrado = p)
                .ReturnsAsync(true);

            // Act
            var resultado = await _service.ProcessarAsync(orderId, userId, jogoId, titulo, preco);

            // Assert
            Assert.NotNull(pagamentoRegistrado);
            Assert.Equal(orderId, pagamentoRegistrado.OrderId);
            Assert.Equal(userId, pagamentoRegistrado.UserId);
            Assert.Equal(jogoId, pagamentoRegistrado.JogoId);
            Assert.Equal(titulo, pagamentoRegistrado.Titulo);
            Assert.Equal(preco, pagamentoRegistrado.Preco);
            _repoMock.Verify(r => r.Registrar(It.IsAny<Pagamento>()), Times.Once);
        }

        [Fact]
        public async Task ProcessarAsync_DeveRetornarPagamentoComMesmoOrderId()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _repoMock.Setup(r => r.Registrar(It.IsAny<Pagamento>())).ReturnsAsync(true);

            // Act
            var resultado = await _service.ProcessarAsync(orderId, 1, 1, "Jogo Teste", 100m);

            // Assert
            Assert.Equal(orderId, resultado.OrderId);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDePagamentos()
        {
            // Arrange
            var pagamentos = new List<Pagamento>
            {
                new Pagamento { Id = 1, OrderId = Guid.NewGuid(), UserId = 1, JogoId = 1, Titulo = "Elden Ring", Preco = 249.90m, Status = "Approved" },
                new Pagamento { Id = 2, OrderId = Guid.NewGuid(), UserId = 2, JogoId = 2, Titulo = "EA FC 26", Preco = 299.90m, Status = "Approved" }
            };

            _repoMock.Setup(r => r.GetAll()).ReturnsAsync(pagamentos);

            // Act
            var resultado = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal("Approved", p.Status));
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoHaPagamentos()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Pagamento>());

            // Act
            var resultado = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
