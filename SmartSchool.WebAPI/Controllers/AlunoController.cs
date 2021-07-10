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
    [Route("api/[controller]")]
    [ApiController] // EVITA FAZER VALIDAÇÕES EM CADA ACTION
    public class AlunoController : ControllerBase
    {
        public readonly IRepository _repo;

        private readonly IMapper _mapper;

        // Construtor da classe AlunoController
        public AlunoController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // QUERY STRING byId?id=2
        [HttpGet("byId")]
        public IActionResult GetById(int id)
        {
            var aluno =  _repo.GetAlunoById(id);

            if(aluno == null) return BadRequest("Aluno não encontrado!");

            var alunoDto = _mapper.Map<IEnumerable<AlunoDto>>(aluno);

            return Ok(alunoDto);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var alunos =  _repo.GetAllAlunos(false);

            var alunoDto=  _mapper.Map<IEnumerable<AlunoDto>>(alunos);

            return Ok(alunoDto);
        }

        // Rota de post 
        [HttpPost]
        public IActionResult Post(AlunoDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            
            return BadRequest("Aluno não cadastrado");
        }

        // Atualizar um registro no banco
        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoDto model)
        {
            var aluno =  _repo.GetAlunoById(id);

            if(aluno == null) return BadRequest("O aluno não pode ser encontrado");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("O aluno foi atualizado com sucesso");
        }

        // ROTA DE PATCH Edita um recurso sem a necessidade de enviar todos os atributos
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoDto model)
        {
            var aluno =  _repo.GetAlunoById(id);

            if(aluno == null) return BadRequest("O aluno não pode ser encontrado");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("O aluno foi atualizado com sucesso");
        }

        // ROTA DE DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno  =  _repo.GetAlunoById(id);

            if(aluno == null) return BadRequest("Não foi possível deletar o Aluno!");

            _repo.Delete(aluno);
            if(_repo.SaveChanges())
            {
                return Ok("Aluno deletado com sucesso");
            }

            return BadRequest("Aluno não encontrado");
        }
    }
}