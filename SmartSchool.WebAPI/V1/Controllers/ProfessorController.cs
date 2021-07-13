using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.V1.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.V1.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repo;

        private readonly IMapper _mapper;

        /// <summary>
        /// Método responsável por iniciar a classe 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public ProfessorController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Método responsável por retornar uma instância de ProfessorRegisterDto
        /// </summary>
        /// <returns></returns>
        [HttpGet("register")]
        public IActionResult GetRegister()
        {
            return Ok(new ProfessorRegisterDto());
        }

        /// <summary>
        /// Método responsável por retornar uma coleção de professores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {   
            var professor = _repo.GetAllProfessores();

            var teacher = _mapper.Map<IEnumerable<ProfessorDto>>(professor);
            
            return Ok(teacher);
        }

        /// <summary>
        /// Método responsável por retornar um professor pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var professor = _repo.GetAlunoById(id, true);

            if(professor == null) return BadRequest("Aluno não pode ser encontrado!");

            return Ok(professor);
        }

        /// <summary>
        /// Método responsáve por retornar via query string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId")]
        public IActionResult GetById(int id)
        {
            var aluno =  _repo.GetProfessorById(id);

            if(aluno == null) return BadRequest("Aluno não encontrado!");

            return Ok(aluno);
        }

        /// <summary>
        /// Método responsável por criar um novo professor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(ProfessorRegisterDto model)
        {
            var professor = _mapper.Map<Professor>(model);

            _repo.Add(professor);

            if(_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }

            return BadRequest("Professor não cadastrado");
        }

        /// <summary>
        /// Método responsável por atualizar parcialmente um registro
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegisterDto model)
        {
            var teacher = this._repo.GetProfessorById(id);

            if(teacher == null) return BadRequest("Professor não encontrado!");

            var professor = _mapper.Map<Professor>(model);

            _repo.Update(professor);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }

            return BadRequest("Professor não atualizado");
        }

        /// <summary>
        /// Método responsável por atualizar um registro 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id,  ProfessorRegisterDto model)
        {
            var teacher = _repo.GetProfessorById(id);

            if(teacher == null) return BadRequest("Professor não encontrado!");

            var professor = _mapper.Map<Professor>(model);

            _repo.Update(professor);

            if(_repo.SaveChanges())
            {
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }

            return BadRequest("O professor não foi atualizado");
        }

        /// <summary>
        /// Método responsáve por deletar um registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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