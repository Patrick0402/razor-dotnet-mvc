using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SessaoService;

namespace EmprestimoLivros.Controllers;

public class HomeController : Controller
{
    private readonly ISessaoInterface _sessaoInterface;

    public HomeController(ISessaoInterface sessaoInterface)
    {
        _sessaoInterface = sessaoInterface;
    }

    public IActionResult Index()
    {
        var usuario = _sessaoInterface.BuscarSessao();
        if (usuario == null)
        {
            return RedirectToAction("Index", "Login");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
