using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.v1.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.v1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfessorController : ControllerBase
    {
        //private readonly SmartContext _context;
        public readonly IRepository _repo;
        private readonly IMapper _mapper;

        //public ProfessorController() { }

        /*
        public ProfessorController(SmartContext context, IRepository repo)
        {
            _context = context;
            _repo = repo;
        }
        */
        public ProfessorController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var professores = _repo.GetAllProfessores(true);
            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(professores));
            //return Ok(result);

        }

        //api/professor/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //var professor = _context.Professores.FirstOrDefault(a => a.Id == id);
            var professor = _repo.GetProfessorById(id, false);
            if(professor == null) return BadRequest("O Professor nao foi encontrado!");

            var professorDto = _mapper.Map<ProfessorDto>(professor);

            return Ok(professorDto);
        }

        //api/professor/byName?nome=Marta&sobrenome=Kente
        /*
        [HttpGet("ByName")]
        public IActionResult GetByName(string nome)
        {
            var professor = _context.Professores.FirstOrDefault(a => a.Nome.Contains(nome));
            if(professor == null) return BadRequest("O professor nao foi encontrado!");
            return Ok(professor);
        }
        */
        //api/professor
        [HttpPost]
        public IActionResult Post(ProfessorRegistrarDto model)
        {
            var professor = _mapper.Map<Professor>(model);

             _repo.Add(professor);
            if(_repo.SaveChanges())
            {
                //return Ok(professor);
                 return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }
            
            return BadRequest("Professor não cadastrado");

             /* VERSAO 1 DO DESENVOLVIMENTO, SEM INTERFACE E REPOSITORIO (USANDO O CONTEXTO DIRETAMENTE)
            _context.Add(professor);
            _context.SaveChanges();
            return Ok(professor);
            */
        }
        //atualiza registro
        //api/professor
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProfessorRegistrarDto model)
        {
            //var prof = _context.Professores.AsNoTracking().FirstOrDefault(a => a.Id == id);
            var professor = _repo.GetProfessorById(id, false);
            if(professor == null) return BadRequest("O professor nao foi encontrado!");

            _mapper.Map(model, professor);

            _repo.Update(professor);
            if(_repo.SaveChanges())
            {
                //return Ok(professor);
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }

            return BadRequest("Professor não Atualizado!");;
        }
        //atualizar parcialmente
        //api/professor
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegistrarDto model)
        {
            //var prof = _context.Professores.AsNoTracking().FirstOrDefault(a => a.Id == id);
            var professor = _repo.GetProfessorById(id, false);
            if(professor == null) return BadRequest("O professor nao foi encontrado!");
            _repo.Update(professor);

            _mapper.Map(model, professor);

            if(_repo.SaveChanges())
            {
                //return Ok(professor);
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }

            return BadRequest("Professor não Atualizado!");
            
        }

        //api/aluno
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var prof = _context.Professores.FirstOrDefault(a => a.Id == id);
            var prof = _repo.GetProfessorById(id, false);
            if(prof == null) return BadRequest("O professor nao foi encontrado!");

            _repo.Delete(prof);
            
            if(_repo.SaveChanges())
            {
                return Ok("Professo deletado.");
            }

            return BadRequest("Professor não deletado!");
        }
    }
}