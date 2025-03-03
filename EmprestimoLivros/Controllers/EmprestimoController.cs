using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        readonly private ApplicationDbContext _db;
        readonly private ISessaoInterface _sessaoInterface;

        public EmprestimoController(ApplicationDbContext db, ISessaoInterface sessaoInterface) 
        {
            _db = db;
            _sessaoInterface = sessaoInterface;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;
            return View(emprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel emprestimo)
        {
            if (ModelState.IsValid)
            {
                emprestimo.DataEmprestimo = DateTime.Now;
                emprestimo.DataUltimaAtualizacao = DateTime.Now;

                _db.Emprestimos.Add(emprestimo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "O empréstimo foi cadastrado com sucesso!";

                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Houve um erro ao cadastrar o empréstimo";

            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || id == 0)
            {
                TempData["MensagemErro"] = "O empréstimo não foi encontrado";
                return NotFound();
            }

            EmprestimosModel? emprestimos = _db.Emprestimos.Find(id);

            if (emprestimos == null)
            {
                TempData["MensagemErro"] = "O empréstimo não foi encontrado";
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
                        TempData["MensagemSucesso"] = "O empréstimo foi editado com sucesso!";

                    }
                }

                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Houve um erro ao editar o empréstimo";

            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["MensagemErro"] = "O empréstimo não foi encontrado";
                return NotFound();
            }

            EmprestimosModel? emprestimos = _db.Emprestimos.Find(id);

            if (emprestimos == null)
            {
                TempData["MensagemErro"] = "O empréstimo não foi encontrado";
                return NotFound();
            }

            return View(emprestimos);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel emprestimo)
        {
            if (emprestimo == null)
            {
                TempData["MensagemErro"] = "O empréstimo não foi encontrado";
                return NotFound();
            }

            _db.Emprestimos.Remove(emprestimo);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "O empréstimo foi removido com sucesso!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Exportar()
        {
            var dados = GetDados();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados, "Dados dos Empréstimos");

                using(MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Emprestimos.xlsx");
                }
            }
        }

        private DataTable GetDados()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Dados dos Empréstimos";

            dt.Columns.Add("Recebedor", typeof(string));
            dt.Columns.Add("Fornecedor", typeof(string));
            dt.Columns.Add("Livro emprestado", typeof(string));
            dt.Columns.Add("Data do empréstimo", typeof(DateTime));
            dt.Columns.Add("Data da última atualizacao", typeof(DateTime));

            var dados = _db.Emprestimos.ToList();

            if (dados.Count() > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    dt.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataEmprestimo, emprestimo.DataUltimaAtualizacao);
                });
            }

            return dt;
        }
    }
}

