using System;
using System.Collections.Generic;
using System.Linq;
using InterviewTest.Model;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace InterviewTest.Repository;

public class EmployeeRepository : BaseRepository<Employee>
{
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(ILogger<EmployeeRepository> logger) => _logger = logger;

    public override List<Employee> Get()
    {
        var employees = new List<Employee>();
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = DataSource };
            
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

    public Employee GetByName(string name)
    {
        var employee = Get().FirstOrDefault(x => x.Name == name);
        return employee;
    }

    public override bool Add(List<Employee> employees)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
                
            using (var transaction = connection.BeginTransaction())
            {
                try 
                { 
                    foreach (var employee in employees)
                    {
                        var insertCmd = connection.CreateCommand();
                        insertCmd.CommandText = @"INSERT INTO Employees (Name, Value) VALUES ($name, $value)";
                        insertCmd.Parameters.AddWithValue("$name", employee.Name);
                        insertCmd.Parameters.AddWithValue("$value", employee.Value);
                        insertCmd.ExecuteNonQuery();
                    }
                        
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    _logger.Log(LogLevel.Error, exception, $"Failed to add employees: {string.Join(',', employees.Select(e => e.ToKeyValuePair))}");
                    return false;
                }
            }
        }

        return true;
    }

    public override List<Employee> Update(List<Employee> employees)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
                
            using (var transaction = connection.BeginTransaction())
            {
                try 
                { 
                    foreach (var employee in employees)
                    {
                        var updateCmd = connection.CreateCommand();
                        updateCmd.CommandText = @"UPDATE Employees SET Value = $value WHERE Name = $name";
                        updateCmd.Parameters.AddWithValue("$name", employee.Name);
                        updateCmd.Parameters.AddWithValue("$value", employee.Value);
                        updateCmd.ExecuteNonQuery();
                    }
                    
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    _logger.Log(LogLevel.Error, exception, $"Failed to update employees: {string.Join(',', employees.Select(e => e.ToKeyValuePair))}");
                    return new List<Employee>();
                }
            }
        }

        //Return new list of employees to ensure database is updated
        var employeeNames = employees.Select(e => e.Name);
        var updatedEmployees = Get().Where(x => employeeNames.Contains(x.Name)).ToList();
        return updatedEmployees;
    }

    public override bool Delete(List<string> employeeNames)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "./SqliteDB.db" };
            
        using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
                
            using (var transaction = connection.BeginTransaction())
            {
                try 
                { 
                    foreach (var employeeName in employeeNames)
                    {
                        var deleteCmd = connection.CreateCommand();
                        deleteCmd.CommandText = @"DELETE FROM Employees WHERE Name = $name";
                        deleteCmd.Parameters.AddWithValue("$name", employeeName);
                        deleteCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    _logger.Log(LogLevel.Error, exception, $"Failed to delete employees: {string.Join(',', employeeNames)}");
                    return false;
                }
            }
        }

        return true;
    }
}