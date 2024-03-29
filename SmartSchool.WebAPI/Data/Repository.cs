using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public class Repository : IRepository
    {
        private readonly SmartContext _context;
        public Repository(SmartContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
                _context.Add(entity);

        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() > 0); 
        }

        //ALUNOS
        public async Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.Disciplina) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(d => d.Professor); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);

            if (!string.IsNullOrEmpty(pageParams.Nome)){
                query = query.Where(aluno => aluno.Nome
                                                  .ToUpper()
                                                  .Contains(pageParams.Nome.ToUpper()) ||
                                                  aluno.Sobrenome
                                                  .ToUpper()
                                                  .Contains(pageParams.Nome.ToUpper()));
            }

            if (pageParams.Matricula > 0){
                query = query.Where(aluno => aluno.Matricula == pageParams.Matricula);
            }

            if (pageParams.Ativo != null){
                query = query.Where(aluno => aluno.Ativo == (pageParams.Ativo != 0));
            }

            if (pageParams.OrderbyName != null){
                query = query.OrderBy(aluno => aluno.Nome);
            }

            if (pageParams.OrderbyIdade != null){
                query = query.OrderByDescending(aluno => aluno.DataNasc);
            }
            //return await query.ToListAsync();
            return await PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public Aluno[] GetAllAlunos(bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.Disciplina) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(d => d.Professor); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return query.ToArray();
        }

        public Aluno[] GetAllAlunosByDisciplinaId(int disciplinaId , bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.Disciplina) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(d => d.Professor); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking()
            .OrderBy(a => a.Id)
            .Where(aluno => aluno.AlunosDisciplinas.Any(ad => ad.DisciplinaId == disciplinaId));

            return query.ToArray();
        }

        public Aluno GetAlunoById(int alunoId , bool includeProfessor = false )
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.Disciplina) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(d => d.Professor); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking()
            .OrderBy(a => a.Id)
            .Where(aluno => aluno.Id == alunoId);

            return query.FirstOrDefault();
        }

        //PROFESSORES
        public Professor[] GetAllProfessores(bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAluno)
            {
                query = query.Include(p => p.Disciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.AlunosDisciplinas) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(a => a.Aluno); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking().OrderBy(p => p.Id);

            return query.ToArray();
        }
 
        public Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId , bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;
 
            if(includeAluno)
            {
                query = query.Include(p => p.Disciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.AlunosDisciplinas) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(a => a.Aluno); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking()
            .OrderBy(p => p.Id)
            .Where(d => d.Disciplinas.Any(ad => ad.AlunosDisciplinas.Any(d => d.DisciplinaId == disciplinaId)));

            return query.ToArray();
        }

        public Professor GetProfessorById(int professorId ,bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAluno)
            {
                query = query.Include(p => p.Disciplinas) // INCLUI ALUNO DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(ad => ad.AlunosDisciplinas) // INCLUI A DISCIPLINA (MODEL) COMO SE FOSSE UM JOIN
                .ThenInclude(a => a.Aluno); // INCLUI PROFESSOR (MODEL) COMO SE FOSSE UM JOIN
            }

            query = query.AsNoTracking()
            .OrderBy(p => p.Id)
            .Where(professor => professor.Id == professorId);

            return query.FirstOrDefault();
        }
    }
}