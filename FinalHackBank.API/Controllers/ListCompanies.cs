using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListCompaniesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ListCompaniesController(IWebHostEnvironment env)
        {
            _env = env;
        }

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

                    // Construct the SQL query to join Company, Users tables
                    string query = @"
                        SELECT 
                            c.UserId,
                            u.Name,
                            c.PhoneNumber,
                            c.Address,
                            u.Email,
                            u.StatusUserId
                        FROM 
                            Company c
                        INNER JOIN 
                            Users u ON c.UserId = u.UserId";

                    // Create SQL command with the query and connection
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        // Execute the SQL command and retrieve the data
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            // Create a list to hold the company data
                            var companies = new List<CompanyDto>();

                            // Read the data and populate the list
                            while (reader.Read())
                            {
                                var company = new CompanyDto
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    StatusUserId = reader.GetInt32(reader.GetOrdinal("StatusUserId"))
                                };

                                companies.Add(company);
                            }

                            // Return the list of companies
                            return Ok(companies);
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

        [HttpPut("update-status/{UserId}")]
        public IActionResult UpdateStatus(int UserId, [FromBody] StatusUpdateModel model)
        {
            Console.WriteLine($"Received UserId: {UserId}");
            Console.WriteLine($"Received StatusUserId: {model.StatusUserId}");

            string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
            using (SqlConnection sqlConnection = new SqlConnection(constr))
            {
                try
                {
                    sqlConnection.Open();

                    // Update the company status
                    string updateQuery = "UPDATE Users SET StatusUserId = @StatusUserId WHERE UserId = @UserId";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, sqlConnection);
                    updateCmd.Parameters.AddWithValue("@StatusUserId", model.StatusUserId);
                    updateCmd.Parameters.AddWithValue("@UserId", UserId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Company status updated successfully" });
                    }
                    else
                    {
                        return NotFound(new { message = "Company not found" });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }
        }

        [HttpPut("update-company/{UserId}")]
        public IActionResult UpdateCompany(int UserId, [FromBody] CompanyDto companyDto)
        {
            Console.WriteLine($"Received UserId: {UserId}");
            Console.WriteLine($"Received Company Data: {companyDto.Name}, {companyDto.Email}, {companyDto.PhoneNumber}, {companyDto.Address}");

            string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
            using (SqlConnection sqlConnection = new SqlConnection(constr))
            {
                try
                {
                    sqlConnection.Open();

                    // Update Users table
                    string updateUsersQuery = "UPDATE Users SET Name = @Name, Email = @Email WHERE UserId = @UserId";
                    SqlCommand updateUsersCmd = new SqlCommand(updateUsersQuery, sqlConnection);
                    updateUsersCmd.Parameters.AddWithValue("@Name", companyDto.Name);
                    updateUsersCmd.Parameters.AddWithValue("@Email", companyDto.Email);
                    updateUsersCmd.Parameters.AddWithValue("@UserId", UserId);
                    updateUsersCmd.ExecuteNonQuery();

                    // Update Company table
                    string updateCompanyQuery = "UPDATE Company SET PhoneNumber = @PhoneNumber, Address = @Address WHERE UserId = @UserId";
                    SqlCommand updateCompanyCmd = new SqlCommand(updateCompanyQuery, sqlConnection);
                    updateCompanyCmd.Parameters.AddWithValue("@PhoneNumber", companyDto.PhoneNumber);
                    updateCompanyCmd.Parameters.AddWithValue("@Address", companyDto.Address);
                    updateCompanyCmd.Parameters.AddWithValue("@UserId", UserId);
                    updateCompanyCmd.ExecuteNonQuery();

                    return Ok(new { message = "Company updated successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string userId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolderPath = Path.Combine(_env.ContentRootPath, "wwwroot", "assets", "img");
            Directory.CreateDirectory(uploadsFolderPath);

            var filePath = Path.Combine(uploadsFolderPath, $"{userId}.jpg");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath = $"/assets/img/{userId}.jpg" });
        }

        [HttpDelete("{UserId}")]
        public IActionResult Delete(int UserId)
        {
            string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
            using (SqlConnection sqlConnection = new SqlConnection(constr))
            {
                try
                {
                    sqlConnection.Open();

                    // Delete related data from Bids table
                    string deleteBidsQuery = "DELETE FROM Bids WHERE CompanyId = @UserId";
                    SqlCommand deleteBidsCmd = new SqlCommand(deleteBidsQuery, sqlConnection);
                    deleteBidsCmd.Parameters.AddWithValue("@UserId", UserId);
                    deleteBidsCmd.ExecuteNonQuery();

                    // Delete company from Company table
                    string deleteCompanyQuery = "DELETE FROM Company WHERE UserId = @UserId";
                    SqlCommand deleteCompanyCmd = new SqlCommand(deleteCompanyQuery, sqlConnection);
                    deleteCompanyCmd.Parameters.AddWithValue("@UserId", UserId);
                    deleteCompanyCmd.ExecuteNonQuery();

                    return Ok(new { message = "Company and related data deleted successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }
        }
    }

    // Define a DTO class to represent the company data
    public class CompanyDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int StatusUserId { get; set; }
    }

    public class StatusUpdateModel
    {
        public int StatusUserId { get; set; }
    }
}
