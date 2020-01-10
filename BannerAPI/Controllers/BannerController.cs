using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BannerAPI.Models;
using BannerAPI.Repositories;

namespace BannerAPI.Controllers
{
    [RoutePrefix("api/Banner")]
    public class BannerController : ApiController
    {
        private readonly BannerRepository _repo = new BannerRepository();

        private static HttpResponseMessage GetDefaultStatusCode(bool success)
        {
            return success ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public Banner Get(int id)
        {
            return _repo.ReadBanner(id);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Banner value)
        {
           return GetDefaultStatusCode(_repo.CreateBanner(value));
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            return GetDefaultStatusCode(_repo.DeleteBanner(id));
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]Banner value)
        {
            return GetDefaultStatusCode(_repo.UpdateBanner(value));
        }

        [HttpGet]
        [Route("Render/{id:int}")]
        public HttpResponseMessage Render(int id)
        {
            var html = _repo.GetBannerHtml(id);
            if (html == null)
            {
                return GetDefaultStatusCode(false);
            }
            var response = GetDefaultStatusCode(true);
            response.Content = new StringContent(html);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}