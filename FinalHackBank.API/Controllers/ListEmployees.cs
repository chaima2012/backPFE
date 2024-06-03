using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
public class ListEmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult List()
    {
        // Construct the SQL connection string
        string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
        
        // Initialize SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(constr))
        {
            try
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Construct the SQL query to join Employees, Users, Role, and Department tables
                string query = @"
                    SELECT 
                        e.UserId,
                        u.Name,
                        u.Email,
                        u.StatusUserId,
                        r.Name AS Role,
                        d.Name AS Department
                    FROM 
                        Employees e
                    INNER JOIN 
                        Users u ON e.UserId = u.UserId
                    INNER JOIN 
                        Role r ON e.RoleId = r.Id
                    INNER JOIN 
                        Department d ON e.DepartmentId = d.Id";

                // Create SQL command with the query and connection
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    // Execute the SQL command and retrieve the data
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        // Create a list to hold the employee data
                        var employees = new List<EmployeeDto>();

                        // Read the data and populate the list
                        while (reader.Read())
                        {
                            var employee = new EmployeeDto
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                StatusUserId = reader.GetInt32(reader.GetOrdinal("StatusUserId")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                Department = reader.GetString(reader.GetOrdinal("Department"))
                            };

                            employees.Add(employee);
                        }

                        // Return the list of employees
                        return Ok(employees);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }[HttpPost("AddReq")]
public IActionResult AddRequest([FromBody] RequestDtoed requestDto)
{
    string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";

    using (SqlConnection sqlConnection = new SqlConnection(constr))
    {
        try
        {
            sqlConnection.Open();
            string query = @"INSERT INTO Requestt (Title, Description, EmployeeId, Daterequest, StatusId, DecisionId)
                             VALUES (@Title, @Description, @EmployeeId, @Daterequest, @StatusId, @DecisionId)";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@Description", requestDto.Description);
                sqlCommand.Parameters.AddWithValue("@Title", requestDto.Title); // Add Title parameter
                sqlCommand.Parameters.AddWithValue("@EmployeeId", requestDto.UserId); // Ensure correct casing
                sqlCommand.Parameters.AddWithValue("@Daterequest", requestDto.Deadline);
                sqlCommand.Parameters.AddWithValue("@StatusId", 1); // Assuming default status ID is 1
                sqlCommand.Parameters.AddWithValue("@DecisionId", 3);

                Console.WriteLine(requestDto.UserId);
                sqlCommand.ExecuteNonQuery();
            }

            return Ok(new { message = "Request added successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}
[HttpPut("editUpd/{userId}")]
public IActionResult UpdateEmployeeProfile(int userId, [FromBody] EmployeeDtoUpdt employeeDto)
{
        Console.WriteLine($"UpdateEmployeeProfile called with userId: {userId}");

    string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
    using (SqlConnection sqlConnection = new SqlConnection(constr))
    {
        try
        {
            sqlConnection.Open();

            // Update Users table
            string updateUsersQuery = @"
                UPDATE Users
                SET Name = @Name, Email = @Email
                WHERE UserId = @UserId";

            using (SqlCommand updateUsersCmd = new SqlCommand(updateUsersQuery, sqlConnection))
            {
                updateUsersCmd.Parameters.AddWithValue("@Name", employeeDto.Name);
                updateUsersCmd.Parameters.AddWithValue("@Email", employeeDto.Email);
                updateUsersCmd.Parameters.AddWithValue("@UserId", userId);
                updateUsersCmd.ExecuteNonQuery();
            }

            return Ok(new { message = "Employee updated successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}
[HttpPut("updatePassword/{userId}")]
    public IActionResult UpdatePassword(int userId, [FromBody] UpdatePasswordDto updatePasswordDto)
    {
        string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
        using (SqlConnection sqlConnection = new SqlConnection(constr))
        {
            try
            {
                sqlConnection.Open();

                // Fetch the current password from the database
                string fetchPasswordQuery = @"
                    SELECT Password
                    FROM Users
                    WHERE UserId = @UserId";

                using (SqlCommand fetchPasswordCmd = new SqlCommand(fetchPasswordQuery, sqlConnection))
                {
                    fetchPasswordCmd.Parameters.AddWithValue("@UserId", userId);
                    var currentPassword = (string)fetchPasswordCmd.ExecuteScalar();

                    // Verify the current password
                    if (updatePasswordDto.CurrentPassword != currentPassword)
                    {
                        return BadRequest(new { message = "Current password is incorrect" });
                    }
                }

                // Update password in Users table
                string updatePasswordQuery = @"
                    UPDATE Users
                    SET Password = @Password
                    WHERE UserId = @UserId";

                using (SqlCommand updatePasswordCmd = new SqlCommand(updatePasswordQuery, sqlConnection))
                {
                    updatePasswordCmd.Parameters.AddWithValue("@Password", updatePasswordDto.NewPassword);
                    updatePasswordCmd.Parameters.AddWithValue("@UserId", userId);
                    updatePasswordCmd.ExecuteNonQuery();
                }

                return Ok(new { message = "Password updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
public class UpdatePasswordDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}

[HttpGet("ListReq/{statusId}")]
public IActionResult ListRequests(int statusId)
{
    string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";

    using (SqlConnection sqlConnection = new SqlConnection(constr))
    {
        try
        {
            sqlConnection.Open();
            string query = @"
                SELECT 
                    req.RequestId, 
                    req.Title,
                    req.Description, 
                    req.EmployeeId, 
                    req.Daterequest, 
                    req.StatusId, 
                    req.DecisionId, 
                    usr.Name AS UserName,
                    usr.Email AS UserEmail,
                    emp.RoleId,
                    emp.DepartmentId,
                    rol.Name AS RoleName,
                    dep.Name AS DepartmentName
                FROM 
                    Requestt req
                INNER JOIN 
                    Users usr ON req.EmployeeId = usr.UserId
                INNER JOIN 
                    Employees emp ON usr.UserId = emp.UserId
                INNER JOIN 
                    Role rol ON emp.RoleId = rol.Id
                INNER JOIN 
                    Department dep ON emp.DepartmentId = dep.Id
                WHERE 
                    req.StatusId = @StatusId";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@StatusId", statusId); // Use parameterized query

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    var requests = new List<RequestDto>();
                    while (reader.Read())
                    {
                        var request = new RequestDto
                        {
                            RequestId = reader.GetInt32(reader.GetOrdinal("RequestId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),                
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                            Deadline = reader.GetString(reader.GetOrdinal("Daterequest")),
                            StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                            DecisionId = reader.GetInt32(reader.GetOrdinal("DecisionId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            UserEmail = reader.GetString(reader.GetOrdinal("UserEmail")),
                            RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                            DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
                        };
                        requests.Add(request);
                    }
                    return Ok(requests);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}
[HttpPut("EditRequest/{requestId}")]
public IActionResult EditRequest(int requestId, [FromBody] RequestDtoEdit requestDto)
{
string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";

    using (SqlConnection sqlConnection = new SqlConnection(constr))
    {
        try
        {
            sqlConnection.Open();
            string query = @"
                UPDATE Requestt 
                SET Title = @Title, Description = @Description, Daterequest = @Deadline 
                WHERE RequestId = @RequestId";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@RequestId", requestId);
                sqlCommand.Parameters.AddWithValue("@Title", requestDto.Title);
                sqlCommand.Parameters.AddWithValue("@Description", requestDto.Description);
                sqlCommand.Parameters.AddWithValue("@Deadline", requestDto.Deadline);
                sqlCommand.ExecuteNonQuery();
            }

            return Ok(new { message = "Request updated successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}

[HttpPut("UpdateRequest/{requestId}")]
public IActionResult UpdateRequest(int requestId)
{
    string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
    using (SqlConnection sqlConnection = new SqlConnection(constr))
    {
        try
        {
            sqlConnection.Open();
            string query = @"
                UPDATE Requestt 
                SET StatusId = 2, DecisionId = 1 
                WHERE RequestId = @RequestId";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@RequestId", requestId);
                sqlCommand.ExecuteNonQuery();
            }

            return Ok(new { message = "Request updated successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}

    [HttpDelete("{userId}")]
    public IActionResult Delete(int userId)
    {
        string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
        using (SqlConnection sqlConnection = new SqlConnection(constr))
        {
            try
            {
                sqlConnection.Open();

                // Delete related data from Requestt table
                string deleteRequesttQuery = "DELETE FROM Requestt WHERE EmployeeId = @UserId";
                SqlCommand deleteRequesttCmd = new SqlCommand(deleteRequesttQuery, sqlConnection);
                deleteRequesttCmd.Parameters.AddWithValue("@UserId", userId);
                deleteRequesttCmd.ExecuteNonQuery();

                // Delete related data from Employees table
                string deleteEmployeeQuery = "DELETE FROM Employees WHERE UserId = @UserId";
                SqlCommand deleteEmployeeCmd = new SqlCommand(deleteEmployeeQuery, sqlConnection);
                deleteEmployeeCmd.Parameters.AddWithValue("@UserId", userId);
                deleteEmployeeCmd.ExecuteNonQuery();

                // Delete user from Users table
                string deleteUserQuery = "DELETE FROM Users WHERE UserId = @UserId";
                SqlCommand deleteUserCmd = new SqlCommand(deleteUserQuery, sqlConnection);
                deleteUserCmd.Parameters.AddWithValue("@UserId", userId);
                deleteUserCmd.ExecuteNonQuery();

                return Ok(new { message = "User and related data deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }

    
[HttpPut("edit/{userId}")]
public IActionResult UpdateEmployee(int userId, [FromBody] EmployeeDtoUp employeeDto)
{
    string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
    using (SqlConnection sqlConnection = new SqlConnection(constr))
    {
        try
        {
            sqlConnection.Open();

            // Update Users table
            string updateUsersQuery = @"
                UPDATE Users
                SET Name = @Name, Email = @Email, StatusUserId = @StatusUserId
                WHERE UserId = @UserId";

            using (SqlCommand updateUsersCmd = new SqlCommand(updateUsersQuery, sqlConnection))
            {
                updateUsersCmd.Parameters.AddWithValue("@Name", employeeDto.Name);
                updateUsersCmd.Parameters.AddWithValue("@Email", employeeDto.Email);
                updateUsersCmd.Parameters.AddWithValue("@StatusUserId", employeeDto.StatusUserId);
                updateUsersCmd.Parameters.AddWithValue("@UserId", userId);
                updateUsersCmd.ExecuteNonQuery();
            }

            // Update Employees table
            string updateEmployeesQuery = @"
                UPDATE Employees
                SET RoleId = (SELECT Id FROM Role WHERE Name = @Role),
                    DepartmentId = (SELECT Id FROM Department WHERE Name = @Department)
                WHERE UserId = @UserId";

            using (SqlCommand updateEmployeesCmd = new SqlCommand(updateEmployeesQuery, sqlConnection))
            {
                updateEmployeesCmd.Parameters.AddWithValue("@Role", employeeDto.Role);
                updateEmployeesCmd.Parameters.AddWithValue("@Department", employeeDto.Department);
                updateEmployeesCmd.Parameters.AddWithValue("@UserId", userId);
                updateEmployeesCmd.ExecuteNonQuery();
            }

            // Log the updated user details
            Console.WriteLine("Updated User Details:");
            Console.WriteLine($"UserId: {userId}");
            Console.WriteLine($"Name: {employeeDto.Name}");
            Console.WriteLine($"Email: {employeeDto.Email}");
            Console.WriteLine($"StatusUserId: {employeeDto.StatusUserId}");
            Console.WriteLine($"Role: {employeeDto.Role}");
            Console.WriteLine($"Department: {employeeDto.Department}");

            return Ok(new { message = "Employee updated successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}

    
}
// Define a DTO class to represent the employee data
public class EmployeeDto
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int StatusUserId { get; set; }
    public string Role { get; set; }
    public string Department { get; set; }
}
public class RequestDto
{
    public int RequestId { get; set; }
    public string Description { get; set; }
    public int EmployeeId { get; set; }
    public int UserId { get; set; }
    public string Deadline { get; set; }  
    public string Title { get; set; } // Add the Title field
    public int StatusId { get; set; }
    public int DecisionId { get; set; }
    public string UserName { get; set; } // Name of the user associated with the request
    public string UserEmail { get; set; } // Email of the user associated with the request
    public int RoleId { get; set; } // Role ID of the user associated with the request
    public int DepartmentId { get; set; } // Department ID of the user associated with the request
    public string RoleName { get; set; } // Name of the role associated with the user
    public string DepartmentName { get; set; } // Name of the department associated with the user
}
public class RequestDtoed
{
    public int UserId { get; set; } // Correct the naming to match the backend
    public int RequestId { get; set; }
    public string Description { get; set; }
    public string Deadline { get; set; }
    public string Title { get; set; }
    public int StatusId { get; set; }
    public int DecisionId { get; set; }
}

public class RequestDtoEdit
{
    public int RequestId { get; set; }
    public string Description { get; set; }
    public int EmployeeId { get; set; }
    public int UserId { get; set; }
    public string Deadline { get; set; }  
    public string Title { get; set; } // Add the Title field
    public int StatusId { get; set; }
    public int DecisionId { get; set; }
    public int RoleId { get; set; } // Role ID of the user associated with the request
    public int DepartmentId { get; set; } // Department ID of the user associated with the request
}
public class EmployeeDtoUp
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int StatusUserId { get; set; }
    public string Role { get; set; }
    public string Department { get; set; }
}
public class EmployeeDtoUpdt
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}