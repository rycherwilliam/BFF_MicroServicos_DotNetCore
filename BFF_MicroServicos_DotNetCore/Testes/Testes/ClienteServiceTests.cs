namespace Testes
{
    [TestClass]
    public class ClienteServiceTests
    {
        private IClienteService _clienteService;
        private Mock<IClienteRepository> _mockClienteRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockClienteRepository = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_mockClienteRepository.Object);
        }

        [TestMethod]
        public async Task GetClientesAsync_ShouldReturnClientes_WhenClientesExist()
        {
            // Arrange
            var clientes = new List<Cliente>
        {
            new Cliente { CpfOuCnpj = "12345678900", Nome = "Cliente 1", Estado = "SP", RendaBruta = 1000 },
            new Cliente { CpfOuCnpj = "98765432100", Nome = "Cliente 2", Estado = "RJ", RendaBruta = 1500 }
        };
            _mockClienteRepository.Setup(r => r.GetClientesAsync()).ReturnsAsync(clientes);

            // Act
            var result = await _clienteService.GetClientesAsync();

            // Assert
            Assert.AreEqual(clientes.Count, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ClientesAPI.Utils.HttpResponseException))]
        public async Task GetClientesAsync_ShouldThrowException_WhenClientesDoNotExist()
        {
            // Arrange
            _mockClienteRepository.Setup(r => r.GetClientesAsync()).ReturnsAsync((IEnumerable<Cliente>)null);

            // Act
            await _clienteService.GetClientesAsync();
        }

        [TestMethod]
        public async Task GetClienteByCpfOuCnpjAsync_ShouldReturnCliente_WhenClienteExists()
        {
            // Arrange
            var cliente = new Cliente { CpfOuCnpj = "12345678900", Nome = "Cliente 1", Estado = "SP", RendaBruta = 1000 };
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync(It.IsAny<string>())).ReturnsAsync(cliente);

            // Act
            var result = await _clienteService.GetClienteByCpfOuCnpjAsync("12345678900");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Cliente 1", result.Nome);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientesAPI.Utils.HttpResponseException))]
        public async Task GetClienteByCpfOuCnpjAsync_ShouldThrowException_WhenClienteDoesNotExist()
        {
            // Arrange
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync(It.IsAny<string>())).ReturnsAsync((Cliente)null);

            // Act
            await _clienteService.GetClienteByCpfOuCnpjAsync("12345678900");
        }

        [TestMethod]
        public async Task AddClienteAsync_ShouldAddCliente_WhenClienteIsValid()
        {
            // Arrange
            var cliente = new Cliente { CpfOuCnpj = "12345678900", Nome = "Cliente 1", Estado = "SP", RendaBruta = 1000 };
            _mockClienteRepository.Setup(r => r.AddClienteAsync(cliente)).Returns(Task.CompletedTask);

            // Act
            await _clienteService.AddClienteAsync(cliente);

            // Assert
            _mockClienteRepository.Verify(r => r.AddClienteAsync(cliente), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientesAPI.Utils.HttpResponseException))]
        public async Task AddClienteAsync_ShouldThrowException_WhenClienteRepositoryThrowsException()
        {
            // Arrange
            var cliente = new Cliente { CpfOuCnpj = "12345678900", Nome = "Cliente 1", Estado = "SP", RendaBruta = 1000 };
            _mockClienteRepository.Setup(r => r.AddClienteAsync(cliente)).ThrowsAsync(new Exception());

            // Act
            await _clienteService.AddClienteAsync(cliente);
        }

        [TestMethod]
        public async Task UpdateClienteAsync_ShouldUpdateCliente_WhenClienteIsValid()
        {
            // Arrange
            var cliente = new Cliente { CpfOuCnpj = "12345678900", Nome = "Cliente 1", Estado = "SP", RendaBruta = 1000 };
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync("12345678900")).ReturnsAsync(cliente);
            _mockClienteRepository.Setup(r => r.UpdateClienteAsync(cliente)).Returns(Task.CompletedTask);

            // Act
            await _clienteService.UpdateClienteAsync("12345678900", cliente);

            // Assert
            _mockClienteRepository.Verify(r => r.UpdateClienteAsync(cliente), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientesAPI.Utils.HttpResponseException))]
        public async Task UpdateClienteAsync_ShouldThrowException_WhenClienteDoesNotExist()
        {
            // Arrange
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync(It.IsAny<string>())).ReturnsAsync((Cliente)null);

            // Act
            await _clienteService.UpdateClienteAsync("12345678900", new Cliente());
        }

        [TestMethod]
        [ExpectedException(typeof(ClientesAPI.Utils.HttpResponseException))]
        public async Task UpdateClienteAsync_ShouldThrowException_WhenClienteRepositoryThrowsException()
        {
            // Arrange
            var cliente = new Cliente { CpfOuCnpj = "12345678900", Nome = "Cliente 1", Estado = "SP", RendaBruta = 1000 };
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync("12345678900")).ReturnsAsync(cliente);
            _mockClienteRepository.Setup(r => r.UpdateClienteAsync(cliente)).ThrowsAsync(new Exception());

            // Act
            await _clienteService.UpdateClienteAsync("12345678900", cliente);
        }

        [TestMethod]
        public async Task DeleteClienteAsync_ShouldDeleteCliente_WhenClienteExists()
        {
            // Arrange
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync("12345678900")).ReturnsAsync(new Cliente());
            _mockClienteRepository.Setup(r => r.DeleteClienteAsync("12345678900")).Returns(Task.CompletedTask);

            // Act
            await _clienteService.DeleteClienteAsync("12345678900");

            // Assert
            _mockClienteRepository.Verify(r => r.DeleteClienteAsync("12345678900"), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientesAPI.Utils.HttpResponseException))]
        public async Task DeleteClienteAsync_ShouldThrowException_WhenClienteDoesNotExist()
        {
            // Arrange
            _mockClienteRepository.Setup(r => r.GetClienteByCpfOuCnpjAsync(It.IsAny<string>())).ReturnsAsync((Cliente)null);

            // Act
            await _clienteService.DeleteClienteAsync("12345678900");
        }
    }
}