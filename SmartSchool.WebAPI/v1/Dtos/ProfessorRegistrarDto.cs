using System;

namespace SmartSchool.WebAPI.v1.Dtos
{
    public class ProfessorRegistrarDto
    {
        public int Id { get; set; }
        public int Registro { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataIni { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Ativo { get; set; } = true;
    }
}