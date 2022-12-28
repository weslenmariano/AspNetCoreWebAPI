using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartSchool.WebAPI.Helpers
{
    public static class Extensions
    {
        public static void AddPatination( this HttpResponse response, int currentPage, int itensPerPage, int totalItems, int totalPages) 
        {
            var paginationHeader = new PaginationHeader(currentPage, itensPerPage, totalItems, totalPages);
            //response.AddPatination

            var camelCaseFormatter =  new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Header", "Pagination");

        }
    }
}