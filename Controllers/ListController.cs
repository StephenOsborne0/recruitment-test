using System;
using System.Linq;
using InterviewTest.Model;
using InterviewTest.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListController : ControllerBase
    {
        private readonly EmployeeRepository _repository;
        private readonly ILogger<ListController> _logger;
        
        public ListController(EmployeeRepository employeeRepository, ILogger<ListController> logger)
        {
            _repository = employeeRepository;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Increment()
        {
            var employees = _repository.Get();

            //Increment the field Value by 1 where the field Name begins with ‘E’ and by 10 where Name begins with ‘G’ and all others by 100
            foreach (Employee employee in employees)
            {
                if (employee.Name.ToUpper().StartsWith("E"))
                    employee.Value++;
                else if (employee.Name.ToUpper().StartsWith("G"))
                    employee.Value += 10;
                else
                    employee.Value += 100;
            }
            
            return new JsonResult(_repository.Update(employees));
        }
        
        [HttpGet]
        [Produces("application/json")]
        public IActionResult SumOfValuesList()
        {
            var employees = _repository.Get();

            if (!employees.Any())
                return new ContentResult { Content = "No employees to sum" };

            var employeesToSum = employees.Where(e => e.Name.ToUpper().StartsWith("A")
                                                            || e.Name.ToUpper().StartsWith("B")
                                                            || e.Name.ToUpper().StartsWith("C"))
                                                      .ToList();
            
            //List the sum of all Values for all Names that begin with A, B or C
            var abcSum = employeesToSum.Select(e => e.Value).Sum();
            Console.WriteLine($"ABC sum = {abcSum}");
            
            //But only present the data where the summed values are greater than or equal to 11171
            var employeesToReturn = employeesToSum.Select((e, index) => new
                                                  {
                                                      e,
                                                      index
                                                  })
                                                  .Where(pair => employeesToSum
                                                                 .Take(pair.index + 1)
                                                                 .Sum(employee => employee.Value) >= 11171)
                                                  .Select(pair => pair.e).ToList();
            
            return new JsonResult(employeesToReturn);
        }
    }
}
