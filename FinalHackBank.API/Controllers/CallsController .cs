using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
public class CallsController : ControllerBase
{
    private readonly string connectionString = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";

    [HttpGet("List")]
    public IActionResult List()
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT CallId, Descriptions, RequiredNumber, DeadLine, Budget, StatusId FROM Calls";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        var calls = new List<CallDto>();
                        while (reader.Read())
                        {
                            var call = new CallDto
                            {
                                CallId = reader.GetInt32(reader.GetOrdinal("CallId")),
                                Description = reader.GetString(reader.GetOrdinal("Descriptions")), // Correct column name
                                RequiredNumber = reader.GetInt32(reader.GetOrdinal("RequiredNumber")),
                                Deadline = reader.GetDateTime(reader.GetOrdinal("DeadLine")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget")),
                                StatusUserId = reader.GetInt32(reader.GetOrdinal("StatusId"))
                            };
                            calls.Add(call);

                            // Log each call's details
                            Console.WriteLine($"Retrieved Call - Description: {call.Description}, RequiredNumber: {call.RequiredNumber}, Deadline: {call.Deadline}, Budget: {call.Budget}, StatusUserId: {call.StatusUserId}");
                        }
                        return Ok(calls);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}"); // Log the error message
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }

[HttpPut("UpdateStatus/{callId}")]
public IActionResult UpdateStatus(int callId, [FromBody] StatusUpdateModel model)
{
    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    {
        try
        {
            sqlConnection.Open();

            // Begin a transaction
            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    // Step 1: Retrieve the Idcall from the Bids table using the provided callId (BidId)
                    string getIdcallQuery = "SELECT Idcall FROM Bids WHERE BidId = @CallId";
                    int idCall = 0;
                    using (SqlCommand getIdcallCommand = new SqlCommand(getIdcallQuery, sqlConnection, transaction))
                    {
                        getIdcallCommand.Parameters.AddWithValue("@CallId", callId);
                        using (SqlDataReader reader = getIdcallCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                idCall = reader.GetInt32(reader.GetOrdinal("Idcall"));
                            }
                            else
                            {
                                // Rollback transaction if no matching BidId found
                                transaction.Rollback();
                                return NotFound(new { message = "Bid not found" });
                            }
                        }
                    }

                    // Step 2: Update the StatusId of the corresponding Idcall in the Calls table
                    string updateStatusQuery = "UPDATE Calls SET StatusId = @StatusId WHERE CallId = @Idcall";
                    using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, sqlConnection, transaction))
                    {
                        updateStatusCommand.Parameters.AddWithValue("@Idcall", idCall);
                        updateStatusCommand.Parameters.AddWithValue("@StatusId", model.StatusId);
                        int rowsAffected = updateStatusCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            return Ok(new { message = "Status updated successfully" });
                        }
                        else
                        {
                            transaction.Rollback();
                            return NotFound(new { message = "Call not found" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
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


public class StatusUpdateModel
{
    public int StatusId { get; set; }
}

[HttpPut("Update/{callId}")]
public IActionResult Update(int callId, [FromBody] callUpdate model)
{
    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    {
        try
        {
            sqlConnection.Open();
            
            // Begin a transaction
            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    // Update the Call details in the Calls table
                    string updateCallQuery = @"UPDATE Calls 
                                               SET Descriptions = @Description, 
                                                   RequiredNumber = @RequiredNumber, 
                                                   DeadLine = @Deadline, 
                                                   Budget = @Budget 
                                               WHERE CallId = @CallId";
                    using (SqlCommand updateCallCommand = new SqlCommand(updateCallQuery, sqlConnection, transaction))
                    {
                        updateCallCommand.Parameters.AddWithValue("@CallId", callId);
                        updateCallCommand.Parameters.AddWithValue("@Description", model.Description);
                        updateCallCommand.Parameters.AddWithValue("@RequiredNumber", model.RequiredNumber);
                        updateCallCommand.Parameters.AddWithValue("@Deadline", model.Deadline);
                        updateCallCommand.Parameters.AddWithValue("@Budget", model.Budget);
                        updateCallCommand.ExecuteNonQuery();
                    }

                    // Update the corresponding Bids with the same CallId in the Bids table
                    string updateBidsQuery = @"UPDATE Bids 
                                               SET  
                                                   AmountTTC= @Budget 
                                               WHERE Idcall = @CallId";
                    using (SqlCommand updateBidsCommand = new SqlCommand(updateBidsQuery, sqlConnection, transaction))
                    {
                        updateBidsCommand.Parameters.AddWithValue("@CallId", callId);
                        updateBidsCommand.Parameters.AddWithValue("@Description", model.Description);
                        updateBidsCommand.Parameters.AddWithValue("@Budget", model.Budget);
                        updateBidsCommand.ExecuteNonQuery();
                    }

                    // Commit the transaction if both updates were successful
                    transaction.Commit();
                    return Ok(new { message = "Call and associated Bids updated successfully" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback transaction if an error occurs
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
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

    private readonly string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedDocuments");

    [HttpGet("DownloadDocument")]
    public IActionResult DownloadDocument(string fileName)
    {
        try
        {
            var filePath = Path.Combine(uploadFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found" });
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    private string GetContentType(string path)
    {
        var types = GetMimeTypes();
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types[ext];
    }

    private Dictionary<string, string> GetMimeTypes()
    {
        return new Dictionary<string, string>
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats.officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
        };
    }
[HttpDelete("DeleteBid/{callId}")]
public IActionResult DeleteBid(int callId)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            
            // Begin a transaction
            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    string deleteCallsQuery = "DELETE FROM Calls WHERE CallId = @CallId";

                    // Delete from Bids table
                    string deleteBidsQuery = "DELETE FROM Bids WHERE Idcall = @CallId";
                    using (SqlCommand deleteBidsCommand = new SqlCommand(deleteBidsQuery, sqlConnection, transaction))
                    {
                        deleteBidsCommand.Parameters.AddWithValue("@CallId", callId);
                        int bidsRowsAffected = deleteBidsCommand.ExecuteNonQuery();
                        if (bidsRowsAffected <= 0)
                        {
                    using (SqlCommand deleteCallsCommand = new SqlCommand(deleteCallsQuery, sqlConnection, transaction))
                    {
                        deleteCallsCommand.Parameters.AddWithValue("@CallId", callId);
                        int callsRowsAffected = deleteCallsCommand.ExecuteNonQuery();
                        if (callsRowsAffected <= 0)
                        {
                            transaction.Rollback(); // Rollback transaction if no rows were affected
                            return NotFound(new { message = "Call not found" });
                        }
                    }
                                        transaction.Commit();

                            return Ok(new { message = "Bid not found but Call was Deleted" });
                        }
                    }

                    // Delete from Calls table
                    using (SqlCommand deleteCallsCommand = new SqlCommand(deleteCallsQuery, sqlConnection, transaction))
                    {
                        deleteCallsCommand.Parameters.AddWithValue("@CallId", callId);
                        int callsRowsAffected = deleteCallsCommand.ExecuteNonQuery();
                        if (callsRowsAffected <= 0)
                        {
                            transaction.Rollback(); // Rollback transaction if no rows were affected
                            return NotFound(new { message = "Call not found" });
                        }
                    }

                    // Commit transaction if both deletes were successful
                    transaction.Commit();
                    return Ok(new { message = "Bid and associated Call deleted successfully" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback transaction if an error occurs
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
    }
}


[HttpGet("BidsCount")]
public IActionResult GetBidsCount()
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            string query = @"SELECT Idcall, COUNT(*) AS BidCount FROM Bids GROUP BY Idcall";
            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    var bidsCount = new List<BidsCountDto>();
                    while (reader.Read())
                    {
                        var bidCount = new BidsCountDto
                        {
                            CallId = reader.GetInt32(reader.GetOrdinal("Idcall")),
                            Count = reader.GetInt32(reader.GetOrdinal("BidCount"))
                        };
                        bidsCount.Add(bidCount);
                    }
                    return Ok(bidsCount);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
    }
}
[HttpGet("ListBids")]
public IActionResult ListBids()
{
    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    {
        try
        {
            sqlConnection.Open();
            string query = @"SELECT b.BidId, ca.Descriptions, ca.StatusId, b.CompanyId, c.Address AS CompanyOffice, u.Name AS CompanyName, b.DocumentName AS Document
                             FROM Bids b
                             INNER JOIN Calls ca ON b.Idcall = ca.CallId
                             INNER JOIN Users u ON b.CompanyId = u.UserId
                             INNER JOIN Company c ON u.UserId = c.UserId";
            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    var calls = new List<CallDto>();
                    while (reader.Read())
                    {
                        var call = new CallDto
                        {
                            CallId = reader.GetInt32(reader.GetOrdinal("BidId")), // Assuming BidId is the CallId
                            Description = reader.GetString(reader.GetOrdinal("Descriptions")),
                            CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                            CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                            CompanyOffice = reader.IsDBNull(reader.GetOrdinal("CompanyOffice")) ? null : reader.GetString(reader.GetOrdinal("CompanyOffice")),
                            Document = reader.IsDBNull(reader.GetOrdinal("Document")) ? null : reader.GetString(reader.GetOrdinal("Document")),
                            StatusUserId = reader.GetInt32(reader.GetOrdinal("StatusId")) // Read the StatusId
                        };
                        calls.Add(call);
                    }
                    return Ok(calls);
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

    [HttpPost("Create")]
    public IActionResult Create([FromBody] CallDtoN model)
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            try
            {
                sqlConnection.Open();
                string query = "INSERT INTO Calls (Descriptions, RequiredNumber, DeadLine, Budget, StatusId) VALUES (@Description, @RequiredNumber, @Deadline, @Budget, @StatusId)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Description", model.Description);
                    sqlCommand.Parameters.AddWithValue("@RequiredNumber", model.RequiredNumber);
                    sqlCommand.Parameters.AddWithValue("@Deadline", model.Deadline);
                    sqlCommand.Parameters.AddWithValue("@Budget", model.Budget);
                    sqlCommand.Parameters.AddWithValue("@StatusId", 1);
                    sqlCommand.ExecuteNonQuery();

                    // Log the inserted call details
                    Console.WriteLine($"Inserted Call - Description: {model.Description}, RequiredNumber: {model.RequiredNumber}, Deadline: {model.Deadline}, Budget: {model.Budget}, StatusUserId: {model.StatusUserId}");

                    return Ok(new { message = "Call added successfully" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}"); // Log the error message
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}

public class CallDto
{
    public int CallId { get; set; }
    public string Description { get; set; }
    public int RequiredNumber { get; set; }
    public DateTime Deadline { get; set; }
    public int Budget { get; set; }
    public int StatusUserId { get; set; } // Add this property to include StatusId

    // Additional properties
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyOffice { get; set; }
    public string Document { get; set; }
}
public class CallDtoN
{
    public int CallId { get; set; }
    public string Description { get; set; }
    public int RequiredNumber { get; set; }
    public DateTime Deadline { get; set; }
    public int Budget { get; set; }
    public int StatusUserId { get; set; } // Add this property to include StatusId

    // Additional properties
    public int CompanyId { get; set; }
}
public class BidsCountDto
{
    public int CallId { get; set; }
    public int Count { get; set; }
}

 public class callUpdate
{
    public string Description { get; set; }
    public int RequiredNumber { get; set; }
    public DateTime Deadline { get; set; }
    public int Budget { get; set; }

}