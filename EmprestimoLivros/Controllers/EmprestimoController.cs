using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        readonly private ApplicationDbContext _db;

        public EmprestimoController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;
            return View(emprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel emprestimo)
        {
            if (ModelState.IsValid)
            {
                _db.Emprestimos.Add(emprestimo);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]

        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel? emprestimos = _db.Emprestimos.Find(id);

            if (emprestimos == null)
            {
                return NotFound();
            }

            return View(emprestimos);
        }

        [HttpPost]
        public IActionResult Editar(EmprestimosModel emprestimo)
        {
            if (ModelState.IsValid)
            {
                var emprestimoDb = _db.Emprestimos.Find(emprestimo.Id);
                if (emprestimoDb != null)
                {
                    if (emprestimoDb.Recebedor != emprestimo.Recebedor ||
                        emprestimoDb.Fornecedor != emprestimo.Fornecedor ||
                        emprestimoDb.LivroEmprestado != emprestimo.LivroEmprestado)
                    {
                        emprestimoDb.Recebedor = emprestimo.Recebedor;
                        emprestimoDb.Fornecedor = emprestimo.Fornecedor;
                        emprestimoDb.LivroEmprestado = emprestimo.LivroEmprestado;
                        emprestimoDb.DataUltimaAtualizacao = DateTime.Now;

                        _db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(emprestimo);
        }


        [HttpGet]

        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel? emprestimos = _db.Emprestimos.Find(id);

            if (emprestimos == null)
            {
                return NotFound();
            }

            return View(emprestimos);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel emprestimo)
        {
            if (emprestimo == null)
            {
                return NotFound();
            }

            _db.Emprestimos.Remove(emprestimo);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
