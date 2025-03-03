using EmprestimoLivros.Models;
using System.Data;

namespace EmprestimoLivros.Services.EmprestimosService
{
    public interface IEmprestimosInterface
    {
        Task<ResponseModel<List<EmprestimosModel>>> BuscarEmprestimos();
        Task<ResponseModel<EmprestimosModel>> BuscarEmprestimoPorId(int? id);
        Task<ResponseModel<EmprestimosModel>> CadastrarEmprestimo(EmprestimosModel emprestimo);
        Task<ResponseModel<EmprestimosModel>> EditarEmprestimo(EmprestimosModel emprestimo);
        Task<ResponseModel<EmprestimosModel>> ExcluirEmprestimo(int? id);
        Task<DataTable> BuscaEmprestimosExportacao();
    }
}
