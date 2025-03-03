using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;

namespace EmprestimoLivros.Services.EmprestimosService
{
    public class EmprestimosService : IEmprestimosInterface
    {
        private readonly ApplicationDbContext _context;
        public EmprestimosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<EmprestimosModel>>> BuscarEmprestimos()
        {
            var response = new ResponseModel<List<EmprestimosModel>>();
            try
            {
                var emprestimos = await _context.Emprestimos.ToListAsync();
                response.Dados = emprestimos;
                response.Mensagem = "Emprestimos encontrados com sucesso";
                return response;
            }
            catch (Exception e)
            {
                response.Mensagem = e.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<EmprestimosModel>> BuscarEmprestimoPorId(int? id)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                if (id == null)
                {
                    response.Mensagem = "Empréstimo não localizado";
                    response.Status = false;
                    return response;
                }

                var emprestimo = await _context.Emprestimos.FirstOrDefaultAsync(e => e.Id == id);

                if (emprestimo == null)
                {
                    response.Mensagem = "Empréstimo não localizado";
                    response.Status = false;
                    return response;
                }

                response.Dados = emprestimo;
                response.Mensagem = "Empréstimo encontrado com sucesso";
                return response;
            }
            catch (Exception e)
            {
                response.Mensagem = e.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<DataTable> BuscaEmprestimosExportacao()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Dados dos Empréstimos";

            dt.Columns.Add("Recebedor", typeof(string));
            dt.Columns.Add("Fornecedor", typeof(string));
            dt.Columns.Add("Livro emprestado", typeof(string));
            dt.Columns.Add("Data do empréstimo", typeof(DateTime));
            dt.Columns.Add("Data da última atualizacao", typeof(DateTime));

            var emprestimos = await BuscarEmprestimos();

            if (emprestimos.Dados.Count() > 0)
            {
                emprestimos.Dados.ForEach(emprestimo =>
                {
                    dt.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataEmprestimo, emprestimo.DataUltimaAtualizacao);
                });
            }

            return dt;
        }

        public async Task<ResponseModel<EmprestimosModel>> CadastrarEmprestimo(EmprestimosModel emprestimo)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                emprestimo.DataEmprestimo = DateTime.Now;
                _context.Emprestimos.Add(emprestimo);
                await _context.SaveChangesAsync();
                response.Mensagem = "O empréstimo foi cadastrado com sucesso!";
                return response;
            }
            catch (Exception e)
            {
                response.Mensagem = e.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<EmprestimosModel>> EditarEmprestimo(EmprestimosModel emprestimo)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                var emprestimoDb = await BuscarEmprestimoPorId(emprestimo.Id);

                if (emprestimoDb.Dados == null)
                {
                    return emprestimoDb;
                }
                else
                {
                    if (emprestimoDb.Dados.Recebedor != emprestimo.Recebedor ||
                        emprestimoDb.Dados.Fornecedor != emprestimo.Fornecedor ||
                        emprestimoDb.Dados.LivroEmprestado != emprestimo.LivroEmprestado)
                    {
                        emprestimoDb.Dados.Recebedor = emprestimo.Recebedor;
                        emprestimoDb.Dados.Fornecedor = emprestimo.Fornecedor;
                        emprestimoDb.Dados.LivroEmprestado = emprestimo.LivroEmprestado;
                        emprestimoDb.Dados.DataUltimaAtualizacao = DateTime.Now;

                        _context.Update(emprestimoDb.Dados);
                        await _context.SaveChangesAsync();
                        response.Mensagem = "O empréstimo foi editado com sucesso!";
                        return response;
                    }
                    else
                    {
                        response.Mensagem = "Não houve alterações no empréstimo";
                        return response;
                    }
                }

            }
            catch (Exception e)
            {
                response.Mensagem = e.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<EmprestimosModel>> ExcluirEmprestimo(int? id)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                var emprestimo = _context.Emprestimos.FirstOrDefault(e => e.Id == id);
                if (emprestimo == null)
                {
                    response.Mensagem = "Empréstimo não localizado";
                    response.Status = false;
                    return response;
                }
                _context.Emprestimos.Remove(emprestimo);
                await _context.SaveChangesAsync();
                response.Mensagem = "Empréstimo excluído com sucesso";
                return response;
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
