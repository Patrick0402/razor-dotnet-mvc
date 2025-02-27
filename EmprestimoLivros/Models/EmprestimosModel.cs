using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Models
{
    public class EmprestimosModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do recebedor")]
        public string Recebedor { get; set; }

        [Required(ErrorMessage = "Digite o nome do fornecedor")]
        public string Fornecedor { get; set; }

        [Required(ErrorMessage = "Digite o nome do livro")]

        public string LivroEmprestado { get; set; }

        [Required(ErrorMessage = "O empréstimo deve ter uma data")]
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.Now;
    }
}
