using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using Newtonsoft.Json;
[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
public class OfferFormController : ControllerBase
{
    private readonly string connectionString = @"Data Source=DESKTOP-VGL0E1R\SQLEXPRESS;Initial Catalog=gestionachats;Trusted_Connection=True;";
    private readonly string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedDocuments");

    [HttpPost("UploadBid")]
    public async Task<IActionResult> UploadBid([FromForm] IFormFile file, [FromForm] FormDataDto formData)
    {
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        if (file != null && file.Length > 0)
        {
            var filePath = Path.Combine(uploadFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            formData.DocumentName = file.FileName; // Store the file name in formData
        }

        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            try
            {
                sqlConnection.Open();

                string checkQuery = $"SELECT COUNT(*) FROM bids WHERE CompanyId = @CompanyId AND Idcall = @Idcall";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, sqlConnection))
                {
                    checkCommand.Parameters.AddWithValue("@CompanyId", formData.CompanyId);
                    checkCommand.Parameters.AddWithValue("@Idcall", formData.Idcall);
                    int existingCount = (int)checkCommand.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        return BadRequest(new { message = "Bid already exists for this company and description." });
                    }
                }

                string insertQuery = @"INSERT INTO bids (CompanyId, AmountTTC, Idcall, DocumentName)
                                       VALUES (@CompanyId, @AmountTTC, @Idcall, @DocumentName)";
                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@CompanyId", formData.CompanyId);
                    sqlCommand.Parameters.AddWithValue("@AmountTTC", formData.AmountTTC);
                    sqlCommand.Parameters.AddWithValue("@Idcall", formData.Idcall);
                    sqlCommand.Parameters.AddWithValue("@DocumentName", formData.DocumentName);
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                Console.WriteLine($"Received Form Data: {JsonConvert.SerializeObject(formData)}");

                return Ok(new { message = "Bid uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}

public class FormDataDto
{
    public string BidId { get; set; }
    public string CompanyId { get; set; }
    public string AmountTTC { get; set; }
    public string Idcall { get; set; }
    public string DocumentName { get; set; }
}
