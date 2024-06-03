using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace connection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateUserStatusController : ControllerBase
    {
        private readonly EmailService _emailService;

        public UpdateUserStatusController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUserStatus(int userId, [FromBody] StatusUpdateModel model)
        {
            string constr = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS; Initial Catalog=gestionachats; Trusted_Connection=True;";
            using (SqlConnection sqlConnection = new SqlConnection(constr))
            {
                try
                {
                    sqlConnection.Open();

                    // Retrieve the user's email
                    string emailQuery = "SELECT Email FROM Users WHERE UserId = @UserId";
                    SqlCommand emailCmd = new SqlCommand(emailQuery, sqlConnection);
                    emailCmd.Parameters.AddWithValue("@UserId", userId);

                    string userEmail = emailCmd.ExecuteScalar() as string;
                    Console.WriteLine($"Retrieved Email: {userEmail}");

                    if (string.IsNullOrEmpty(userEmail))
                    {
                        return NotFound(new { message = "User not found" });
                    }

                    if (model.StatusUserId == 2)
                    {
                        // Update the user's status
                        string updateQuery = "UPDATE Users SET StatusUserId = @StatusUserId WHERE UserId = @UserId";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, sqlConnection);
                        updateCmd.Parameters.AddWithValue("@StatusUserId", model.StatusUserId);
                        updateCmd.Parameters.AddWithValue("@UserId", userId);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Send email with the appropriate image attachment
                            string subject = "Regarding Registration as an Employee In STB PM";
                            string body = "Your registration status has been updated.";
                            string imagePath = model.StatusUserId == 2 ? Path.Combine(AppContext.BaseDirectory, "approved.jpg") : Path.Combine(AppContext.BaseDirectory, "declined.png");

                            Console.WriteLine($"Sending Email to: {userEmail} with Image: {imagePath}");

                            _emailService.SendEmail(userEmail, subject, body, imagePath);

                            return Ok(new { message = "User status updated and email sent successfully" });
                        }
                        else
                        {
                            return NotFound(new { message = "User not found" });
                        }
                    }
                    else if (model.StatusUserId == 3)
                    {
                        // Delete the user and related data in employee
                        string deleteEmployeeQuery = "DELETE FROM Employees WHERE UserId = @UserId";
                        SqlCommand deleteEmployeeCmd = new SqlCommand(deleteEmployeeQuery, sqlConnection);
                        deleteEmployeeCmd.Parameters.AddWithValue("@UserId", userId);
                        deleteEmployeeCmd.ExecuteNonQuery();

                        string deleteUserQuery = "DELETE FROM Users WHERE UserId = @UserId";
                        SqlCommand deleteUserCmd = new SqlCommand(deleteUserQuery, sqlConnection);
                        deleteUserCmd.Parameters.AddWithValue("@UserId", userId);
                        deleteUserCmd.ExecuteNonQuery();

                        return Ok(new { message = "User and related data deleted successfully" });
                    }

                    return BadRequest(new { message = "Invalid StatusUserId" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }
        }
    }

    public class StatusUpdateModel
    {
        public int StatusUserId { get; set; }
    }
}
