using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly SmartContext _context;
        public readonly IRepository _repo;

        public AlunoController(SmartContext context,
                                IRepository repo)
        {
            _repo = repo;
            _context = context;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Alunos);
        }

        //api/aluno/byId/1
        [HttpGet("byId/{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");
            return Ok(aluno);
        }

        //api/aluno/byName?nome=Marta&sobrenome=Kente
        [HttpGet("ByName")]
        public IActionResult GetByName(string nome, string sobrenome)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");
            return Ok(aluno);
        }
        //api/aluno
        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            _repo.Add(aluno);
            if(_repo.SaveChanges())
            {
                return Ok(aluno);
            }
            
            return BadRequest("Aluno não cadastrado");

            /* VERSAO 1 DO DESENVOLVIMENTO, SEM INTERFACE E REPOSITORIO (USANDO O CONTEXTO DIRETAMENTE)
            _context.Add(aluno);
            _context.SaveChanges();
            return Ok(aluno);
            */
        }
        //atualiza registro
        //api/aluno
        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (alu == null) return BadRequest("O aluno nao foi encontrado!");

            _repo.Update(aluno);
            if(_repo.SaveChanges())
            {
                return Ok(aluno);
            }
            
            return BadRequest("Aluno não atualizado.");
            /* VERSAO 1 DO DESENVOLVIMENTO, SEM INTERFACE E REPOSITORIO (USANDO O CONTEXTO DIRETAMENTE)
            _context.Update(aluno);
            _context.SaveChanges();
            return Ok(aluno);
            */
        }
        //atualizar parcialmente
        //api/aluno
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (alu == null) return BadRequest("O aluno nao foi encontrado!");

            _repo.Update(aluno);
            if(_repo.SaveChanges())
            {
                return Ok(aluno);
            }
            
            return BadRequest("Aluno não atualizado.");
            /* VERSAO 1 DO DESENVOLVIMENTO, SEM INTERFACE E REPOSITORIO (USANDO O CONTEXTO DIRETAMENTE)
            _context.Update(aluno);
            _context.SaveChanges();
            return Ok(aluno);
            */
        }

        //api/aluno
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");

            _repo.Delete(aluno);
            if(_repo.SaveChanges())
            {
                return Ok("Aluno deletado");
            }
            
            return BadRequest("Aluno não deletado.");
            /* VERSAO 1 DO DESENVOLVIMENTO, SEM INTERFACE E REPOSITORIO (USANDO O CONTEXTO DIRETAMENTE)
            _context.Remove(aluno);
            _context.SaveChanges();
            return Ok();
            */
        }
    }
}