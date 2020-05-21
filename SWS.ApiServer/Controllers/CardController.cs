using Microsoft.AspNetCore.Mvc;
using SWS.ApiServer.Models;

namespace SWS.ApiServer.Controllers
{
    [Route("/Card")]
    public class CardController : Controller
    {
        [HttpPost]
        [Route("AttachCard")]
        public void AttachCard([FromBody] CardRequest request)
        {
            var request2 = request;
        }

        [HttpGet]
        public string AttachCard()
        {
            return "dsds";
        }
    }
}