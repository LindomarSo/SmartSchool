using System.Collections.Generic;

namespace SmartSchool.WebAPI.V1.Dtos
{
    public class AlunoDto
    {
        /// <summary>
        /// Id do aluno
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Matr�cula do aluno
        /// </summary>
        public int Matricula { get; set; }
        
        /// <summary>
        /// Nome do aluno
        /// </summary>
        public string Nome { get; set; }
        
        /// <summary>
        /// Telefone do aluno
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Idade do aluno
        /// </summary>
        public int Idade { get; set; }

        /// <summary>
        /// Data de in�cio do aluno
        /// </summary>
        public System.DateTime DataInicio { get; set; } 

        /// <summary>
        /// Se ainda esta ativo na escola
        /// </summary>
        public bool Ativo { get; set; }
    } 
}