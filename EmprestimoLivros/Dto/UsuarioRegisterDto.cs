using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Dto
{
    public class UsuarioRegisterDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmaSenha { get; set; }
    }
}
