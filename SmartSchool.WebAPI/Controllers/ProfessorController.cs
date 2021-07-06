using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly SmartContext _context;
        public ProfessorController() { }

        public ProfessorController(SmartContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Professores);
        }

        //api/aluno/byId/1
        [HttpGet("byId/{id}")]
        public IActionResult GetById(int id)
        {
            var professor = _context.Professores.FirstOrDefault(a => a.Id == id);
            if(professor == null) return BadRequest("O Professor nao foi encontrado!");
            return Ok(professor);
        }

        //api/professor/byName?nome=Marta&sobrenome=Kente
        [HttpGet("ByName")]
        public IActionResult GetByName(string nome)
        {
            var professor = _context.Professores.FirstOrDefault(a => a.Nome.Contains(nome));
            if(professor == null) return BadRequest("O professor nao foi encontrado!");
            return Ok(professor);
        }
        //api/professor
        [HttpPost]
        public IActionResult Post(Professor professor)
        {
            _context.Add(professor);
            _context.SaveChanges();
            return Ok(professor);
        }
        //atualiza registro
        //api/professor
        [HttpPut("{id}")]
        public IActionResult Put(int id, Professor professor)
        {
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if(prof == null) return BadRequest("O professor nao foi encontrado!");
            _context.Update(professor);
            _context.SaveChanges();
            return Ok(professor);
        }
        //atualizar parcialmente
        //api/professor
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if(prof == null) return BadRequest("O professor nao foi encontrado!");
            _context.Update(professor);
            _context.SaveChanges();
            return Ok(professor);
        }

        //api/aluno
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prof = _context.Professores.FirstOrDefault(a => a.Id == id);
            if(prof == null) return BadRequest("O professor nao foi encontrado!");
            _context.Remove(prof);
            _context.SaveChanges();
            return Ok();
        }
    }
}