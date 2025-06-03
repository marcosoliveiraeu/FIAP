using FiapCloudGames.Domain.Enuns;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.DTOs
{
    public class AtualizarUsuarioDto 
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(200, ErrorMessage = "O hash da senha deve ter no máximo 200 caracteres.")]
        public string SenhaHash { get; set; }

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        public Perfil Perfil { get; set; }
    }
}
