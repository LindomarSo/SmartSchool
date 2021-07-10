using System;

namespace SmartSchool.WebAPI.Models
{
    public class AlunoDisciplina
    {
        public AlunoDisciplina(){}

        public AlunoDisciplina(int alunoId,  int disciplinaId)
        {
            this.AlunoId =  alunoId;
            DisciplinaId = disciplinaId;
        }

        public DateTime DataInicio { get; set; } = DateTime.Now;

        public DateTime? DataFim { get; set; } = null;

        public int? Nota { get; set; } = null; 

        public int AlunoId { get; set; }
        
        // Propriedade que retorna a classe aluno populada
        public Aluno Aluno { get; set; }
        
        public int DisciplinaId { get; set; }

        // Propriedade que retorna a classe Disciplina populada
        public Disciplina Disciplina { get; set; }
    }
}