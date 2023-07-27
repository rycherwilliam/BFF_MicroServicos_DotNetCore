namespace Testes { 
    [TestClass]
    public class PagamentoServiceTests
    {
        private IPagamentoService _pagamentoService;
        private Mock<IPagamentoRepository> _mockPagamentoRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockPagamentoRepository = new Mock<IPagamentoRepository>();
            _pagamentoService = new PagamentoService(_mockPagamentoRepository.Object);
        }

        [TestMethod]
        public async Task GetPagamentosAsync_ShouldReturnPagamentos_WhenPagamentosExist()
        {
            // Arrange
            var pagamentos = new List<Pagamento>
            {
                new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago },
                new Pagamento { Id = 2, NumeroDoContrato = 1001, Parcela = 2, Valor = 500, EstadoPagamento = EstadoPagamento.Atrasado }
            };
            _mockPagamentoRepository.Setup(r => r.GetPagamentosAsync()).ReturnsAsync(pagamentos);

            // Act
            var result = await _pagamentoService.GetPagamentosAsync();

            // Assert
            Assert.AreEqual(pagamentos.Count, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
            public async Task GetPagamentosAsync_ShouldThrowException_WhenPagamentosDoNotExist()
        {
            // Arrange
            _mockPagamentoRepository.Setup(r => r.GetPagamentosAsync()).ReturnsAsync((IEnumerable<Pagamento>)null);

            // Act
            await _pagamentoService.GetPagamentosAsync();
        }

        [TestMethod]
        public async Task GetPagamentoByIdAsync_ShouldReturnPagamento_WhenPagamentoExists()
        {
            // Arrange
            var pagamento = new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago };
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(1)).ReturnsAsync(pagamento);

            // Act
            var result = await _pagamentoService.GetPagamentoByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1001, result.NumeroDoContrato);
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
        public async Task GetPagamentoByIdAsync_ShouldThrowException_WhenPagamentoDoesNotExist()
        {
            // Arrange
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(It.IsAny<int>())).ReturnsAsync((Pagamento)null);

            // Act
            await _pagamentoService.GetPagamentoByIdAsync(1);
        }

        [TestMethod]
        public async Task AddPagamentoAsync_ShouldAddPagamento_WhenPagamentoIsValid()
        {
            // Arrange
            var pagamento = new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago };
            _mockPagamentoRepository.Setup(r => r.AddPagamentoAsync(pagamento)).Returns(Task.CompletedTask);

            // Act
            await _pagamentoService.AddPagamentoAsync(pagamento);

            // Assert
            _mockPagamentoRepository.Verify(r => r.AddPagamentoAsync(pagamento), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
        public async Task AddPagamentoAsync_ShouldThrowException_WhenPagamentoRepositoryThrowsException()
        {
            // Arrange
            var pagamento = new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago };
            _mockPagamentoRepository.Setup(r => r.AddPagamentoAsync(pagamento)).ThrowsAsync(new Exception());

            // Act
            await _pagamentoService.AddPagamentoAsync(pagamento);
        }

        [TestMethod]
        public async Task UpdatePagamentoAsync_ShouldUpdatePagamento_WhenPagamentoIsValid()
        {
            // Arrange
            var pagamento = new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago };
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(1)).ReturnsAsync(pagamento);
            _mockPagamentoRepository.Setup(r => r.UpdatePagamentoAsync(pagamento)).Returns(Task.CompletedTask);

            // Act
            await _pagamentoService.UpdatePagamentoAsync(1, pagamento);

            // Assert
            _mockPagamentoRepository.Verify(r => r.UpdatePagamentoAsync(pagamento), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
        public async Task UpdatePagamentoAsync_ShouldThrowException_WhenPagamentoDoesNotExist()
        {
            // Arrange
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(It.IsAny<int>())).ReturnsAsync((Pagamento)null);

            // Act
            await _pagamentoService.UpdatePagamentoAsync(1, new Pagamento());
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
        public async Task UpdatePagamentoAsync_ShouldThrowException_WhenPagamentoRepositoryThrowsException()
        {
            // Arrange
            var pagamento = new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago };
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(1)).ReturnsAsync(pagamento);
            _mockPagamentoRepository.Setup(r => r.UpdatePagamentoAsync(pagamento)).ThrowsAsync(new Exception());

            // Act
            await _pagamentoService.UpdatePagamentoAsync(1, pagamento);
        }

        [TestMethod]
        public async Task DeletePagamentoAsync_ShouldDeletePagamento_WhenPagamentoExists()
        {
            // Arrange
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(1)).ReturnsAsync(new Pagamento());
            _mockPagamentoRepository.Setup(r => r.DeletePagamentoAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _pagamentoService.DeletePagamentoAsync(1);

            // Assert
            _mockPagamentoRepository.Verify(r => r.DeletePagamentoAsync(1), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
        public async Task DeletePagamentoAsync_ShouldThrowException_WhenPagamentoDoesNotExist()
        {
            // Arrange
            _mockPagamentoRepository.Setup(r => r.GetPagamentoByIdAsync(It.IsAny<int>())).ReturnsAsync((Pagamento)null);

            // Act
            await _pagamentoService.DeletePagamentoAsync(1);
        }

        [TestMethod]
        public async Task GetPagamentosDoCliente_ShouldReturnPagamentos_WhenPagamentosExistForCliente()
        {
            // Arrange
            var pagamentos = new List<Pagamento>
            {
                new Pagamento { Id = 1, NumeroDoContrato = 1001, Parcela = 1, Valor = 500, EstadoPagamento = EstadoPagamento.Pago, CpfCnpjCliente = "12345678900" },
                new Pagamento { Id = 2, NumeroDoContrato = 1002, Parcela = 1, Valor = 600, EstadoPagamento = EstadoPagamento.A_Vencer, CpfCnpjCliente = "98765432100" },
                new Pagamento { Id = 3, NumeroDoContrato = 1001, Parcela = 2, Valor = 500, EstadoPagamento = EstadoPagamento.Atrasado, CpfCnpjCliente = "12345678900" }
            };
            _mockPagamentoRepository.Setup(r => r.GetPagamentosDoCliente("12345678900")).ReturnsAsync(pagamentos.Where(p => p.CpfCnpjCliente == "12345678900"));

            // Act
            var result = await _pagamentoService.GetPagamentosDoCliente("12345678900");

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(PagamentosAPI.Utils.HttpResponseException))]
        public async Task GetPagamentosDoCliente_ShouldThrowException_WhenNoPagamentosExistForCliente()
        {
            // Arrange
            _mockPagamentoRepository.Setup(r => r.GetPagamentosDoCliente(It.IsAny<string>())).ReturnsAsync((IEnumerable<Pagamento>)null);

            // Act
            await _pagamentoService.GetPagamentosDoCliente("12345678900");
        }
    }
}