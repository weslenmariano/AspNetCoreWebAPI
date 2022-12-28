using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public interface IRepository
    {
         void Add<T>(T entity) where T : class;

         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         bool SaveChanges();

        //ALUNOS
         Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false);
         Aluno[] GetAllAlunos(bool includeProfessor);

         Aluno[] GetAllAlunosByDisciplinaId(int disciplinaId , bool includeProfessor);

         //Aluno GetAllAlunoById(int alunoId , bool includeProfessor = false);

         Aluno GetAlunoById(int alunoId , bool includeProfessor = false);
         
         //PROFESSORES
         Professor[] GetAllProfessores(bool includeAluno = false);

         Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId , bool includeAluno);

         //Professor GetAllProfessorById(int professorId ,bool includeAluno);
         Professor GetProfessorById(int professorId ,bool includeAluno);

    }
}