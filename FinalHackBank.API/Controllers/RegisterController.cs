using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RegistrationModel model)
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

                // Check if email already exists
                string emailCheckQuery = @"SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (SqlCommand emailCheckCommand = new SqlCommand(emailCheckQuery, sqlConnection))
                {
                    emailCheckCommand.Parameters.AddWithValue("@Email", model.Email);
                    int existingEmailCount = (int)emailCheckCommand.ExecuteScalar();

                    if (existingEmailCount > 0)
                    {
                        return BadRequest(new { message = "Email already exists" });
                    }
                }

                // Retrieve the role ID based on the Position
                string roleQuery = @"SELECT Id FROM Role WHERE Name = @Position";
                int wposition;
                using (SqlCommand roleCommand = new SqlCommand(roleQuery, sqlConnection))
                {
                    roleCommand.Parameters.AddWithValue("@Position", model.Position);
                    wposition = (int)roleCommand.ExecuteScalar();
                }

                // Construct the SQL query for registration
                string query = @"INSERT INTO Users (Name, Email, Password, StatusUserId, Role) 
                                 VALUES (@Name, @Email, @Password, @StatusUserId, @Role);
                                 SELECT SCOPE_IDENTITY();";

                // Create SQL command with the query and connection
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.AddWithValue("@Name", model.Name);
                    sqlCommand.Parameters.AddWithValue("@Email", model.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", model.Password);
                    sqlCommand.Parameters.AddWithValue("@StatusUserId", 1);
                    sqlCommand.Parameters.AddWithValue("@Role", wposition);

                    // Execute the SQL command and retrieve the UserId
                    int userId = Convert.ToInt32(sqlCommand.ExecuteScalar());

                    // Check if any rows were affected
                    if (userId > 0)
                    {
                        string depQuery = @"SELECT Id FROM Department WHERE Name = @Department";
                        int ndep;
                        using (SqlCommand depCommand = new SqlCommand(depQuery, sqlConnection))
                        {
                            depCommand.Parameters.AddWithValue("@Department", model.Department);
                            ndep = (int)depCommand.ExecuteScalar();
                        }

                        string employeeInsertQuery = @"INSERT INTO Employees (UserId, RoleId, DepartmentId) 
                                                       VALUES (@UserId, @RoleId, @DepartmentId)";
                        using (SqlCommand empInsertCommand = new SqlCommand(employeeInsertQuery, sqlConnection))
                        {
                            empInsertCommand.Parameters.AddWithValue("@UserId", userId);
                            empInsertCommand.Parameters.AddWithValue("@RoleId", wposition);
                            empInsertCommand.Parameters.AddWithValue("@DepartmentId", ndep);

                            empInsertCommand.ExecuteNonQuery();
                        }

                        return Ok(new { message = "Registration successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to register user" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}

// Define a model class to represent the registration data
public class RegistrationModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Position { get; set; }
    public string Department { get; set; }
}
