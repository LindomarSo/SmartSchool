using AutoMapper;
using SmartSchool.WebAPI.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Helpers
{
    public class SmartSchoolProfile : Profile
    {
        public SmartSchoolProfile()
        {
            // ESTOU FALANDO PARA O MEU AUTOMAPPER QUE TODA VEZ QUE EU TIVER O ALUNO 
            // COMO PARÂMETRO EU ESTOU MAPEANDO PARA O ALUNO DTO
            // O FATO DAS PROPRIEDADES TEREM O MESMO NOME ELE CONSEGUE FAZER O MAPEAMENTO
            // CORRETO
            CreateMap<Aluno, AlunoDto>()
                .ForMember(
                    dest => dest.Nome,
                    opt => opt.MapFrom(src => $"{src.Nome} {src.Sobrenome}")
                )
                .ForMember(
                    dest => dest.Idade,
                    opt => opt.MapFrom(src => src.DataNascimento.GetCurrentAge()) 
                )
            ;
        }
    }
}