using AutoMapper;
using SmartSchool.WebAPI.v2.Dtos;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.Helpers;

namespace SmartSchool.WebAPI.v2.Profiles
{
    public class SmartSchoolProfile : Profile
    {
        public SmartSchoolProfile()
        {
            //MAPEAMENTO DE/PARA para campos q não possuem o mesmo nome de atributo.
            //-------origem , destino
            CreateMap<Aluno, AlunoDto>()
                .ForMember(
                    destino => destino.Nome,
                    opt => opt.MapFrom(origem => $"{origem.Nome} {origem.Sobrenome}")
                )
                .ForMember(
                    destino => destino.Idade,
                    opt => opt.MapFrom(origem => origem.DataNasc.GetCurrentAge())
                );  
            //
            CreateMap<AlunoDto, Aluno>();
            // Para gravacao de um novo aluno, com mapeamento bi-direcional 
            CreateMap<Aluno, AlunoRegistrarDto>().ReverseMap();

            
        }
    }
}