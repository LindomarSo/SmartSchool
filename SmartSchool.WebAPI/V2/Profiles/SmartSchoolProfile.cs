using AutoMapper;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V2.Dtos;

namespace SmartSchool.WebAPI.V2.Profiles
{
    public class SmartSchoolProfile : Profile
    {
        public SmartSchoolProfile()
        {
            CreateMap<Aluno, AlunoDto>()
                    .ForMember(
                        opt => opt.Nome,
                        opt => opt.MapFrom(src => $"{src.Nome} {src.Sobrenome}")
                    ).ForMember(
                        dest => dest.Idade,
                        opt => opt.MapFrom(src => src.DataNascimento.GetCurrentAge())
                    );
            
            CreateMap<Aluno, AlunoRegisterDto>().ReverseMap();
        }
    }
}