using InterviewTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public List<Employee> Get()
        {
            var employees = new List<Employee>();
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees";
                
                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        });
                    }
                }
            }

            return employees;
        }
        
        
        [HttpPost]
        public IActionResult Add(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
                return new ContentResult
                {
                    Content = "Invalid employee list supplied", 
                    StatusCode = StatusCodes.Status400BadRequest 
                };
            
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                
                using (var transaction = connection.BeginTransaction())
                {
                    try 
                    { 
                        var insertCmd = connection.CreateCommand();
                        insertCmd.CommandText = @"INSERT INTO Employees (Name, Value) VALUES ($name, $value)";
                
                        foreach (var employee in employees)
                        {
                            insertCmd.Parameters.AddWithValue("$name", employee.Name);
                            insertCmd.Parameters.AddWithValue("$value", employee.Value);
                        }
                        
                        insertCmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            return Ok();
        }
        
        [HttpPut]
        public IActionResult Update(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
                return new ContentResult
                {
                    Content = "Invalid employee list supplied", 
                    StatusCode = StatusCodes.Status400BadRequest 
                };
            
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                
                using (var transaction = connection.BeginTransaction())
                {
                    try 
                    { 
                        var updateCmd = connection.CreateCommand();
                        updateCmd.CommandText = @"UPDATE Employees SET Value = $value WHERE Name = $name";
                
                        foreach (var employee in employees)
                        {
                            updateCmd.Parameters.AddWithValue("$name", employee.Name);
                            updateCmd.Parameters.AddWithValue("$value", employee.Value);
                        }
                        
                        updateCmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            return Ok();
        }
        
        [HttpDelete]
        public IActionResult Delete(List<string> employeeNames)
        {
            if (employeeNames == null || !employeeNames.Any())
                return new ContentResult
                {
                    Content = "Invalid employee name list supplied", 
                    StatusCode = StatusCodes.Status400BadRequest 
                };
            
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                
                using (var transaction = connection.BeginTransaction())
                {
                    try 
                    { 
                        var deleteCmd = connection.CreateCommand();
                        deleteCmd.CommandText = @"DELETE FROM Employees WHERE Name = $name";
                
                        foreach (var employeeName in employeeNames)
                            deleteCmd.Parameters.AddWithValue("$name", employeeName);

                        deleteCmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            return Ok();
        }
    }
}
