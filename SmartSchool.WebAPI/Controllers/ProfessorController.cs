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
    public class ProfessorController : ControllerBase
    {
        public List<Aluno> Alunos =  new List<Aluno>() {
            new Aluno(){
                Id = 1,
                Nome = "Lindomar",
                Sobrenome = "Souza Dias",
                Telefone = "(61) 9 9999-9999"
            },
            new Aluno(){
                Id = 2,
                Nome = "Pedro",
                Sobrenome = "Barbosa",
                Telefone = "(61) 9 9999-9999"
            },
            new Aluno(){
                Id = 3,
                Nome = "Joao",
                Sobrenome = "Marcos",
                Telefone = "(61) 9 9999-9999"
            }
        };

        private readonly IRepository _repo;

        public ProfessorController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.GetAllProfessores(true));
        }

        // api/professor/1
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var professor = _repo.GetAlunoById(id, true);

            if(professor == null) return BadRequest("Aluno não pode ser encontrado!");

            return Ok(professor);
        }

        // Por meio de query strings não posso passar as chaves
        [HttpGet("byId")]
        public IActionResult GetById(int id)
        {
            var aluno =  _repo.GetProfessorById(id);

            if(aluno == null) return BadRequest("Aluno não encontrado!");

            return Ok(aluno);
        }

        // ROTA DE POST PARA ADICIONAR UM NOVO PROFESSOR
        [HttpPost]
        public IActionResult Post(Professor professor)
        {
            _repo.Add(professor);
            if(_repo.SaveChanges())
            {
                return Ok(professor);
            }

            return BadRequest("Professor não cadastrado");
        }

        // ROTA DE PATCH ATUALIZA UM DADO SEM A NECESSIDADE DE PASSAR TODOS OS ATRIBUTOS
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var teacher = this._repo.GetProfessorById(id);

            if(teacher == null) return BadRequest("Professor não encontrado!");

            _repo.Update(professor);
            
            if(_repo.SaveChanges())
            {
                return Ok(professor);
            }

            return BadRequest("Professor não atualizado");
        }

        // ROTA DE PUT 
        [HttpPut("{id}")]
        public IActionResult Put(int id,  Professor professor)
        {
            var teacher = _repo.GetProfessorById(id);

            if(teacher == null) return BadRequest("Professor não encontrado!");

            _repo.Update(professor);
            if(_repo.SaveChanges())
            {
                return Ok(professor);
            }

            return BadRequest("O professor não foi atualizado");
        }

        // ROTA DE DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var teacher =  _repo.GetProfessorById(id);

            if(teacher == null) return BadRequest("Professor não encontrado!");

            _repo.Delete(teacher);
            
            if(_repo.SaveChanges())
            {
                return Ok("Professor deletado com sucesso!");
            }

            return BadRequest("Professor não encontrado");
        }
    }
}