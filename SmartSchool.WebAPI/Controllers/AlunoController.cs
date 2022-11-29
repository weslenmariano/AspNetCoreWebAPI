using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
       // private readonly SmartContext _context;
        public readonly IRepository _repo;
        private readonly IMapper _mapper;

        //v2 do desenvolvimento
        /*
        public AlunoController(SmartContext context,
                                IRepository repo)
        {
            _repo = repo;
            _context = context;
        }
        */

        public AlunoController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        
        [HttpGet]
        public IActionResult Get()
        {
            //return Ok(_context.Alunos);
            var alunos = _repo.GetAllAlunos(true);
            /* // substituido pelo auto mapper...
            var alunosRetorno = new List<AlunoDto>();

            foreach (var aluno in alunos)
            {
                alunosRetorno.Add(new AlunoDto(){
                    Id = aluno.Id,
                    Matricula = aluno.Matricula,
                    Nome = $"{aluno.Nome} {aluno.Sobrenome}",
                    Telefone = aluno.Telefone,
                    DataNasc = aluno.DataNasc,
                    DataIni = aluno.DataIni,
                    Ativo = aluno.Ativo
                });
            }
             return Ok(alunosRetorno);
            */



            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
        }

        [HttpGet("getRegister")]
        public IActionResult GetRegister()
        {
            return Ok(new AlunoRegistrarDto());
        }


        //api/aluno/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
            var aluno = _repo.GetAlunoById(id, false);
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");

            var alunoDto = _mapper.Map<AlunoDto>(aluno);

            return Ok(alunoDto);
        }

        //api/aluno/byName?nome=Marta&sobrenome=Kente
        //v2 desenvolvimento
        /*
        [HttpGet("ByName")]
        public IActionResult GetByName(string nome, string sobrenome)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");
            return Ok(aluno);
        } */
        //api/aluno
        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto alunoModel)
        {
            var aluno = _mapper.Map<Aluno>(alunoModel);

            _repo.Add(aluno);
            if(_repo.SaveChanges())
            {
                //return Ok(aluno);
                return Created($"/api/aluno/{alunoModel.Id}", _mapper.Map<AlunoDto>(aluno));
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
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {
            //var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if(_repo.SaveChanges())
            {
                //return Ok(aluno);
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
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
        public IActionResult Patch(int id, AlunoRegistrarDto model)
        {
            //var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("O aluno nao foi encontrado!");

             _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if(_repo.SaveChanges())
            {
                //return Ok(aluno);
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
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
            //var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
            var aluno = _repo.GetAlunoById(id);
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