using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        bool SaveChanges();

        // ALUNOS
        Aluno[] GetAllAlunos(bool includeProfessor);

        Aluno[] GetAllAlunosByDisciplinaId(int disciplinaId, bool includeProfessor =  false);

        Aluno GetAlunoById(int alunoId, bool includeProfessor = false);

        // PROFESSORES
        Professor[] GetAllProfessores(bool includeDisciplinas = false);

        Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, bool includeAlunos = false);

        Professor GetProfessorById(int professorId,  bool includeProfessor = false);
    }
}