using Hangfire.Logging;
using Hangfire.Model;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        public static List<Employee> employees = new List<Employee>();


        private readonly IEmployeeMethods _iemployeemethods;

        public EmployeeController(IEmployeeMethods iemployeemethods)
        {
            _iemployeemethods = iemployeemethods;
        }

        [HttpPost]
        public IActionResult AddPilot(Employee emp)
        {
            if (ModelState.IsValid)
            {
                employees.Add(emp);
                _iemployeemethods.AddEmployee(emp);
                BackgroundJob.Enqueue<IEmployeeMethods>(x => x.SendEmail());
                return CreatedAtAction("GetEmployee", new { emp.Id }, emp);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetEmployee(int id)
        {
            var emps = _iemployeemethods.GetEmployeeList();

            var emp = emps.FirstOrDefault(x => x.Id == id);
            if (emp == null)
                return NotFound();
            BackgroundJob.Enqueue<IEmployeeMethods>(x => x.SyncData());
            return Ok(emp);
        }


    }
}
