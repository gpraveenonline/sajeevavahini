using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SV.DataAccess;
using SV.DataAccess.Models;

namespace SV.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private TestDAL testDAL;

        private string ConnectionString;

        private readonly ILogger testLogger;

        public TestController(IConfiguration configuration, ILoggerFactory logger)
        {
            testLogger = logger.CreateLogger<TestController>();

            ConnectionString = configuration.GetConnectionString("SampleDb");

            testDAL = new TestDAL(ConnectionString);
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            testLogger.LogInformation("Entering into Test/Get");

            var testList = await testDAL.GetTestList();

            if (testList == null)
                return NotFound();

            testLogger.LogInformation("Exiting Test/Get");

            return Ok(testList);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var test = await testDAL.GetTestById(id);

            if (test == null)
                return NotFound();

            return Ok(test);
        }

        [HttpGet]
        [Route("GetBySP/{id}")]
        public async Task<IActionResult> GetBySP(int id)
        {
            var test = await testDAL.GetTestBySp(id);

            if (test == null)
                return NotFound();

            return Ok(test);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Test test)
        {
            var addTest = await testDAL.AddTest(test);

            if (addTest <= 0)
                return NotFound();

            return Ok(addTest);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Test test)
        {
            var updateTest = await testDAL.UpdateTest(test);

            if (updateTest <= 0)
                return NotFound();

            return Ok(updateTest);
        }

        // DELETE api/values/5
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromBody]Test test)
        {
            var deleteTest = await testDAL.DeleteTest(test);

            if (deleteTest <= 0)
                return NotFound();

            return Ok(deleteTest);
        }
    }
}
