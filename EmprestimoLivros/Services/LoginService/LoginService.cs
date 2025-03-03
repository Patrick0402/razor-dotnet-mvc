using EmprestimoLivros.Data;
using EmprestimoLivros.Dto;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;
using EmprestimoLivros.Services.SessaoService;

namespace EmprestimoLivros.Services.LoginService
{
    public class LoginService : ILoginInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        private readonly ISessaoInterface _sessaoInterface;
        public LoginService(ApplicationDbContext context, ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _sessaoInterface = sessaoInterface;
        }

        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try 
            {
                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioLoginDto.Email);
                if (usuario == null)
                {
                    response.Mensagem = "Usuário não encontrado";
                    response.Status = false;
                    return response;
                }
                if (!_senhaInterface.VerificaSenha(usuarioLoginDto.Senha, usuario.senhaHash, usuario.senhaSalt)) 
                {
                    response.Mensagem = "Credenciais inválidas";
                    response.Status = false;
                    return response;
                }

                // Criar a sessão do usuário
                _sessaoInterface.CriarSessao(usuario);

                response.Mensagem = "Usuário logado com sucesso";
                return response; //Devolve true como padrão

            }
            catch (Exception e)
            {
                response.Mensagem = e.Message;
                response.Status = false;
                return response;
            }
        }

     
    }

}
