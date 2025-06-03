using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Moq;
using FluentAssertions;

namespace FiapCloudGames.Tests
{
    public class JogosUsuarioServiceTests
    {
        private readonly Mock<IJogosUsuarioRepository> _jogosUsuarioRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IJogoRepository> _jogoRepositoryMock;
        private readonly Mock<IPromocaoJogoRepository> _promocaoJogoRepositoryMock;

        private readonly IJogosUsuarioService _jogosUsuarioService;

        public JogosUsuarioServiceTests()
        {
            _jogosUsuarioRepositoryMock = new Mock<IJogosUsuarioRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _jogoRepositoryMock = new Mock<IJogoRepository>();
            _promocaoJogoRepositoryMock = new Mock<IPromocaoJogoRepository>();

            _jogosUsuarioService = new JogosUsuarioService(
                _jogosUsuarioRepositoryMock.Object,
                _usuarioRepositoryMock.Object,
                _jogoRepositoryMock.Object,
                _promocaoJogoRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Deve_Lancar_Erro_Se_Usuario_Ja_Possuir_Jogo_Ao_Inserir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            _jogoRepositoryMock.Setup(r => r.ObterPorIdAsync(jogoId))
                               .ReturnsAsync(new Jogo { Id = jogoId });

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(usuarioId))
                                   .ReturnsAsync(new Usuario { Id = usuarioId });

            _jogosUsuarioRepositoryMock.Setup(r => r.ExisteAsync(usuarioId, jogoId))
                                       .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _jogosUsuarioService.AdicionarAsync(usuarioId, jogoId);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("O usuário já possui este jogo.");
        }
        
        [Fact]
        public async Task Deve_Lancar_Erro_Ao_Inserir_Se_Jogo_Nao_Existir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            _jogoRepositoryMock.Setup(r => r.ObterPorIdAsync(jogoId))
                               .ReturnsAsync((Jogo)null);

            // Act
            Func<Task> act = async () => await _jogosUsuarioService.AdicionarAsync(usuarioId, jogoId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Jogo não encontrado.");
        }

        [Fact]
        public async Task Deve_Lancar_Erro_Ao_Inserir_Se_Usuario_Nao_Existir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            _jogoRepositoryMock.Setup(r => r.ObterPorIdAsync(jogoId))
                               .ReturnsAsync(new Jogo());

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(usuarioId))
                                  .ReturnsAsync((Usuario)null);

            // Act
            Func<Task> act = async () => await _jogosUsuarioService.AdicionarAsync(usuarioId, jogoId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado.");
        }

        [Fact]
        public async Task Deve_Aplicar_Preco_Com_Desconto_Se_Estiver_Em_Promocao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();
            var jogo = new Jogo { Id = jogoId, Preco = 100 };

            _jogoRepositoryMock.Setup(r => r.ObterPorIdAsync(jogoId))
                               .ReturnsAsync(jogo);

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(usuarioId))
                                  .ReturnsAsync(new Usuario());

            _promocaoJogoRepositoryMock.Setup(r => r.ObterPromocaoAtivaPorJogoIdAsync(jogoId))
                                       .ReturnsAsync(new PromocaoJogo
                                       {
                                           ValorComDesconto = 70
                                       });

            // Act
            await _jogosUsuarioService.AdicionarAsync(usuarioId, jogoId);

            // Assert
            _jogosUsuarioRepositoryMock.Verify(r => r.AdicionarAsync(It.Is<JogosUsuario>(j =>
                j.PrecoPago == 70
            )), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_JogoUsuario_Por_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            var jogosUsuario = new JogosUsuario { Id = id };

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(id))
                                       .ReturnsAsync(jogosUsuario);

            // Act
            var resultado = await _jogosUsuarioService.ObterPorIdAsync(id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(id);
        }


        [Fact]
        public async Task Deve_Retornar_Todos_JogosUsuario()
        {
            // Arrange
            var lista = new List<JogosUsuario> { new JogosUsuario(), new JogosUsuario() };

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterTodosAsync())
                                       .ReturnsAsync(lista);

            // Act
            var resultado = await _jogosUsuarioService.ObterTodosAsync();

            // Assert
            resultado.Should().HaveCount(2);
        }

        [Fact]
        public async Task Deve_Retornar_JogosUsuario_Por_UsuarioId()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var usuario = new Usuario { Id = usuarioId };

            var lista = new List<JogosUsuario>
    {
        new JogosUsuario { UsuarioId = usuarioId }
    };

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(usuarioId))
                                   .ReturnsAsync(usuario); //  Mock do usuário para não dar NotFound

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorUsuarioIdAsync(usuarioId))
                                       .ReturnsAsync(lista); //  Mock dos JogosUsuario

            // Act
            var resultado = await _jogosUsuarioService.ObterPorUsuarioIdAsync(usuarioId);

            // Assert
            resultado.Should().NotBeEmpty();
            resultado.All(j => j.UsuarioId == usuarioId).Should().BeTrue();
        }


        [Fact]
        public async Task Deve_Retornar_JogosUsuario_Por_JogoId()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var jogo = new Jogo { Id = jogoId };

            var lista = new List<JogosUsuario>
            {
                new JogosUsuario { JogoId = jogoId }
            };

            _jogoRepositoryMock.Setup(r => r.ObterPorIdAsync(jogoId))
                               .ReturnsAsync(jogo); //  Mock para garantir que o jogo existe

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorJogoIdAsync(jogoId))
                                       .ReturnsAsync(lista); //  Mock dos JogosUsuario

            // Act
            var resultado = await _jogosUsuarioService.ObterPorJogoIdAsync(jogoId);

            // Assert
            resultado.Should().NotBeEmpty();
            resultado.All(j => j.JogoId == jogoId).Should().BeTrue();
        }

        [Fact]
        public async Task Deve_Remover_JogoUsuario()
        {
            // Arrange
            var id = Guid.NewGuid();
            var jogoUsuario = new JogosUsuario();


            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(id))
                               .ReturnsAsync(jogoUsuario); // Mock da verificação de existência

            _jogosUsuarioRepositoryMock.Setup(r => r.RemoverAsync(jogoUsuario))
                                       .Returns(Task.CompletedTask); // Mock da remoção

            // Act
            await _jogosUsuarioService.RemoverAsync(id);

            // Assert
            _jogosUsuarioRepositoryMock.Verify(r => r.RemoverAsync(jogoUsuario), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Erro_Se_Tentar_Remover_E_Id_Nao_Existe()
        {
            // Arrange
            var id = Guid.NewGuid();

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(id))
                                       .ReturnsAsync((JogosUsuario)null);

            // Act
            Func<Task> act = async () => await _jogosUsuarioService.RemoverAsync(id);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Registro de jogo do usuário não encontrado.");
        }

        [Fact]
        public async Task Deve_Remover_Jogo_Do_Usuario_Quando_Existe()
        {
            // Arrange
            var id = Guid.NewGuid();
            var jogoUsuario = new JogosUsuario { Id = id};

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(id))
                                       .ReturnsAsync(jogoUsuario);

            // Act
            await _jogosUsuarioService.RemoverAsync(id);

            // Assert
            _jogosUsuarioRepositoryMock.Verify(r => r.RemoverAsync(jogoUsuario), Times.Once);
        }

       

        [Fact]
        public async Task Deve_Adicionar_Jogo_Se_Usuario_Nao_Possuir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            var jogo = new Jogo { Id = jogoId, Preco = 100 };
            var usuario = new Usuario { Id = usuarioId };

            _jogoRepositoryMock.Setup(r => r.ObterPorIdAsync(jogoId))
                               .ReturnsAsync(jogo);

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(usuarioId))
                                   .ReturnsAsync(usuario);

            _jogosUsuarioRepositoryMock.Setup(r => r.ExisteAsync(usuarioId, jogoId))
                                       .ReturnsAsync(false);

            _promocaoJogoRepositoryMock.Setup(r => r.ObterPromocaoAtivaPorJogoIdAsync(jogoId))
                                       .ReturnsAsync((PromocaoJogo)null);

            // Act
            await _jogosUsuarioService.AdicionarAsync(usuarioId, jogoId);

            // Assert
            _jogosUsuarioRepositoryMock.Verify(r => r.AdicionarAsync(It.Is<JogosUsuario>(j =>
                j.UsuarioId == usuarioId &&
                j.JogoId == jogoId &&
                j.PrecoPago == 100
            )), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Erro_Se_JogoUsuario_Nao_Existir_Por_Id()
        {
            // Arrange
            var id = Guid.NewGuid();

            _jogosUsuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(id))
                                       .ReturnsAsync((JogosUsuario)null);

            // Act
            Func<Task> act = async () => await _jogosUsuarioService.ObterPorIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Registro de jogo do usuário não encontrado.");
        }

        [Fact]
        public async Task Deve_Lancar_Erro_Se_Usuario_Nao_Existir_Em_ObterPorUsuarioId()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(usuarioId))
                                   .ReturnsAsync((Usuario)null);

            // Act
            Func<Task> act = async () => await _jogosUsuarioService.ObterPorUsuarioIdAsync(usuarioId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado.");
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Se_Nao_Houver_JogosUsuario()
        {
            // Arrange
            _jogosUsuarioRepositoryMock.Setup(r => r.ObterTodosAsync())
                                       .ReturnsAsync(new List<JogosUsuario>());

            // Act
            var resultado = await _jogosUsuarioService.ObterTodosAsync();

            // Assert
            resultado.Should().BeEmpty();
        }

    }

}