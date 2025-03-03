using EmprestimoLivros.Dto;
using EmprestimoLivros.Models;

namespace EmprestimoLivros.Services.CadastroService
{
    public interface ICadastroInterface
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuario);
    }
}
