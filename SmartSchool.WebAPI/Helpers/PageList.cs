using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartSchool.WebAPI.Helpers
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages =  (int)Math.Ceiling(count/(double)pageSize); // ceiling arredondar

            this.AddRange(items); // this é a propria clace, e como precisamos add um lista, usamos addrange, e não somente o add

        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1)*pageSize) // caso a pagina pedida for a segunda pula os primeiros registros
                                    .Take(pageSize) // pega os itens de acordo com o tamanho de pagina
                                    .ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}