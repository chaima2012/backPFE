using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
public class RegisterCompanyController : ControllerBase
{
    [HttpPost]
    public IActionResult RegisterCompany([FromBody] CompanyRegistrationModel model)
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

                // Construct the SQL query for registration
                string query = @"INSERT INTO Users (Name, Email, Password, StatusUserId, Role) 
                                 VALUES (@Name, @Email, @Password, @StatusUserId, NULL);
                                 SELECT SCOPE_IDENTITY();";

                // Create SQL command with the query and connection
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.AddWithValue("@Name", model.CompanyName);
                    sqlCommand.Parameters.AddWithValue("@Email", model.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", model.Password);
                    sqlCommand.Parameters.AddWithValue("@StatusUserId", 1);

                    // Execute the SQL command and retrieve the UserId
                    int userId = Convert.ToInt32(sqlCommand.ExecuteScalar());

                    // Check if any rows were affected
                    if (userId > 0)
                    {
                        // Insert company data
                        string companyInsertQuery = @"INSERT INTO Company (UserId, PhoneNumber, Address) 
                                                      VALUES (@UserId, @PhoneNumber, @Address)";
                        using (SqlCommand companyInsertCommand = new SqlCommand(companyInsertQuery, sqlConnection))
                        {
                            companyInsertCommand.Parameters.AddWithValue("@UserId", userId);
                            companyInsertCommand.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                            companyInsertCommand.Parameters.AddWithValue("@Address", model.Address);

                            companyInsertCommand.ExecuteNonQuery();
                        }

                        return Ok(new { message = "Company registration successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to register company" });
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

// Define a model class to represent the company registration data
public class CompanyRegistrationModel
{
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
