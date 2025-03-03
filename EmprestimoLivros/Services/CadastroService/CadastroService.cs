using EmprestimoLivros.Data;
using EmprestimoLivros.Dto;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;

namespace EmprestimoLivros.Services.CadastroService
{
    public class CadastroService : ICadastroInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        public CadastroService(ApplicationDbContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }
        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                if (VerificarSeEmailExisteu(usuarioRegisterDto))
                {
                    response.Status = false;
                    response.Mensagem = "Email já cadastrado.";
                    return response;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegisterDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var usuario = new UsuarioModel
                {
                    Nome = usuarioRegisterDto.Nome,
                    Sobrenome = usuarioRegisterDto.Sobrenome,
                    Email = usuarioRegisterDto.Email,
                    senhaHash = senhaHash,
                    senhaSalt = senhaSalt
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                response.Mensagem = "Usuário cadastrado com sucesso.";
                response.Status = true;
                return response;
            }
            catch (Exception e)
            {
                response.Status = false;
                response.Mensagem = "Erro ao cadastrar usuário." + e.Message;
                return response;
            }
        }

        private bool VerificarSeEmailExisteu(UsuarioRegisterDto usuario)
        {
            return _context.Usuarios.Any(x => x.Email == usuario.Email);
        }
    }
}
