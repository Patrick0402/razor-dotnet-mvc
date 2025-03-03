using EmprestimoLivros.Dto;
using EmprestimoLivros.Services.CadastroService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class CadastroController : Controller
    {
        private readonly ICadastroInterface _cadastroInterface;
        private readonly ISessaoInterface _sessaoInterface;
        public CadastroController(ICadastroInterface cadastroInterface, ISessaoInterface sessaoInterface)
        {
            _cadastroInterface = cadastroInterface;
            _sessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UsuarioRegisterDto usuarioRegisterDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _cadastroInterface.RegistrarUsuario(usuarioRegisterDto);
                if (usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(usuarioRegisterDto);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(usuarioRegisterDto);
            }
        }
    }
}
