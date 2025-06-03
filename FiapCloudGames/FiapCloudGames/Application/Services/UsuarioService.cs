using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using FiapCloudGames.Infrastructure.Repository;
using FiapCloudGames.Application.DTOs;

namespace FiapCloudGames.Application.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenhaHasher _senhaHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, ISenhaHasher senhaHasher)
        {
            _usuarioRepository = usuarioRepository;
            _senhaHasher = senhaHasher;
        }

        public async Task CadastrarUsuarioAsync(string nome, string email, string senha)
        {
            if (await _usuarioRepository.EmailExisteAsync(email))
                throw new BusinessException("Já existe um usuário cadastrado com esse email.");

            if (!SenhaValida(senha))
                throw new BusinessException("A senha não atende aos critérios de segurança.");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Email = email,
                SenhaHash = _senhaHasher.Hash(senha),
                Perfil = Perfil.USUARIO
            };

            await _usuarioRepository.IncluirAsync(usuario);
        }

        public async Task<bool> VerificaEmailDupplicado(string email)
        {
            return await _usuarioRepository.EmailExisteAsync(email);

        }

        public bool SenhaValida(string senha)
        {
            if (senha.Length < 8)
                return false;

            bool temLetraMaiuscula = senha.Any(char.IsUpper);
            bool temLetraMinuscula = senha.Any(char.IsLower);
            bool temNumero = senha.Any(char.IsDigit);
            bool temCaractereEspecial = senha.Any(c => !char.IsLetterOrDigit(c));

            return temLetraMaiuscula && temLetraMinuscula && temNumero && temCaractereEspecial;
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return usuarios;
        }


        public async Task RemoverAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
            {
                throw new NotFoundException("Usuario não encontrado.");
            }

            await _usuarioRepository.RemoverAsync(id);
        }

        public async Task AtualizarAsync(AtualizarUsuarioDto usuarioAlterado)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioAlterado.Id);

            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            if (!SenhaValida(usuarioAlterado.SenhaHash))
                throw new BusinessException("A senha não atende aos critérios de segurança.");

           
            usuario.Nome = usuarioAlterado.Nome;
            usuario.Email = usuarioAlterado.Email;
            usuario.SenhaHash = _senhaHasher.Hash(usuarioAlterado.SenhaHash);
            usuario.Perfil = usuarioAlterado.Perfil;

            await _usuarioRepository.AtualizarAsync(usuario);


        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return usuario;

        }

        
    }
}
