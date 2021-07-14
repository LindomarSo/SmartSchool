using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartSchool.WebAPI.Helpers
{
    public static class Extensions
    {
        public static void AddPagination(
            this HttpResponse response,
            int currentPage, 
            int itemsPerPage,
            int totalItems, 
            int totalPages 
        )
        {
            var paginationHeader = new PaginationHeader( 
                currentPage, itemsPerPage,totalItems, totalPages);

            var camelCasingFormatter =  new JsonSerializerSettings();

            camelCasingFormatter.ContractResolver =  new CamelCasePropertyNamesContractResolver();

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCasingFormatter));
            response.Headers.Add("Access-Control-Expose-Header", "Pagination");
        }
    }
}