using EmprestimoLivros.Models;
using Newtonsoft.Json;

namespace EmprestimoLivros.Services.SessaoService
{
    public class SessaoService : ISessaoInterface
    {
        private readonly IHttpContextAccessor _contextAcessor;
        public SessaoService(IHttpContextAccessor contextAcessor)
        {
            _contextAcessor = contextAcessor;
        }

        public UsuarioModel BuscarSessao()
        {
            var sessaoUsuario = _contextAcessor.HttpContext.Session.GetString("sessaoUsuario");
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessao(UsuarioModel usuarioModel)
        {
            var usuarioJson = JsonConvert.SerializeObject(usuarioModel);
            _contextAcessor.HttpContext.Session.SetString("sessaoUsuario", usuarioJson);
        }

        public void RemoveSessao()
        {
            _contextAcessor.HttpContext.Session.Remove("sessaoUsuario");
        }
    }
}
