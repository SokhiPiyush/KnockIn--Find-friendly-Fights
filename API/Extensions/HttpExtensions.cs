using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions//this is an extension method therefeore the class needs to be static
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
           var jsonnOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; //it needs to be serialized into json format and not c# foramt//default method is pascal casing therefore we need to convert it to CamelCase

           response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonnOptions));
           
           //pagination is a custom header therefore we need to explicitly allow CORS policy
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");

        }
    }
}