using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCloudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TMController : ControllerBase
    {

        // GET api/<TMController>/5
        [HttpGet("{Queue}")]
        [HttpGet(Name = "GetQueues")]
        public List<string> Get(string Queue)
        {
           
            return TMInitialize.GetQueues(Queue);
        }


    }
}
