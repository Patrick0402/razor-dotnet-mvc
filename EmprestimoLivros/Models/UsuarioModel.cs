using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email não é válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public byte[] senhaHash { get; set; }
        [Required(ErrorMessage = "O salt é obrigatório.")]
        public byte[] senhaSalt { get; set; }

    }
}
