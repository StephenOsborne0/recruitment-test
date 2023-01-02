using InterviewTest.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using InterviewTest.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly ILogger<EmployeesController> _logger;
        
        public EmployeesController(IRepository<Employee> employeeRepository, ILogger<EmployeesController> logger)
        {
            _repository = employeeRepository;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("/Get")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            var employees = _repository.Get();
            return new JsonResult(employees);
        }
        
        [HttpPost]
        [Route("/Add")]
        public IActionResult Add(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
                return new ContentResult
                {
                    Content = "Invalid employee list supplied", 
                    StatusCode = StatusCodes.Status400BadRequest 
                };
            
            return _repository.Add(employees) ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        [HttpPut]
        [Route("/Update")]
        public IActionResult Update(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
                return new ContentResult
                {
                    Content = "Invalid employee list supplied", 
                    StatusCode = StatusCodes.Status400BadRequest 
                };

            return new JsonResult(_repository.Update(employees));
        }
        
        [HttpDelete]
        [Route("/Delete")]
        public IActionResult Delete(List<string> employeeNames)
        {
            if (employeeNames == null || !employeeNames.Any())
                return new ContentResult
                {
                    Content = "Invalid employee name list supplied", 
                    StatusCode = StatusCodes.Status400BadRequest 
                };
            
            return _repository.Delete(employeeNames) ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
