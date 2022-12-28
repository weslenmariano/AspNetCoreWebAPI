namespace SmartSchool.WebAPI.Helpers
{
    public class PageParams
    {
        public const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize 
        { 
            get{ return pageSize;}
            set{ pageSize = (value > MaxPageSize) ? MaxPageSize : value;} // garante q  page size nao seja maior do que o max page (se o value for maior que o maxpage retorna o maxpage, senao retorna o proprio value)
        }

        public int? Matricula { get; set; } = null;
        public string Nome { get; set; } = string.Empty;
        public int? Ativo { get; set; } = null;

        public int? OrderbyName { get; set; } = null;
        public int? OrderbyIdade { get; set; } = null;

    }
}