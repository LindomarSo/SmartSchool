using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.V1.Dtos;
using SmartSchool.WebAPI.Models;
using System.Threading.Tasks;
using SmartSchool.WebAPI.Helpers;

namespace SmartSchool.WebAPI.V2.Controllers
{
    /// <summary>
    /// Versão 2
    /// </summary>
    [ApiController] // EVITA FAZER VALIDAÇÕES EM CADA ACTION
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlunoController : ControllerBase
    {
        /// <summary>
        /// Encasulamento
        /// </summary>
        public readonly IRepository _repo;

        private readonly IMapper _mapper;

        /// <summary>
        /// Método responsável por iniciar a classe
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public AlunoController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Método responsável por retornar uma instância de AlunoRegisterDto
        /// </summary>
        /// <returns></returns>
        [HttpGet("register")]
        public IActionResult GetRegister()
        {
            return Ok(new AlunoRegisterDto());
        }

        /// <summary>
        /// Método responsável por retornar um registro pelo ID via query string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId")]
        public IActionResult GetById(int id)
        {
            var aluno =  _repo.GetAlunoById(id);

            if(aluno == null) return BadRequest("Aluno não encontrado!");

            var alunoDto = _mapper.Map<IEnumerable<AlunoDto>>(aluno);

            return Ok(alunoDto);
        }

        /// <summary>
        /// Método responsável por retorna uma coleção completa do banco 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            var alunos = await  _repo.GetAllAlunosAsync(pageParams, false);

            var alunoDto=  _mapper.Map<IEnumerable<AlunoDto>>(alunos);

            // Retornar no Header
            Response.AddPagination(alunos.CurrentPage, alunos.PageSize, alunos.TotalCount, alunos.TotalPages);

            return Ok(alunoDto);
        }

        /// <summary>
        /// Método responsável por criar um novo aluno 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(AlunoRegisterDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            
            return BadRequest("Aluno não cadastrado");
        }

        /// <summary>
        /// Método responsável por atualizar parcialmente um AlunoRetisterDto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoRegisterDto model)
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

        /// <summary>
        /// Método responsável por deletar um registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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