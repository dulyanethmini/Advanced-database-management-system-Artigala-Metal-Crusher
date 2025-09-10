using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Quantity
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<QuantityInfo> ListQuantity { get; set; } = new List<QuantityInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Project;Integrated Security=True";
                _logger.LogInformation("Attempting to connect to the database");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    _logger.LogInformation("Database connection established");

                    string sql = "SELECT Material_id, Date_received, Remaining_quantity FROM Quantity";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                QuantityInfo quantityInfo = new QuantityInfo
                                {
                                    Material_id = reader["Material_id"].ToString(),
                                    Date_received = reader["Date_received"].ToString(),
                                    Remaining_quantity = reader["Remaining_quantity"].ToString(),
                                   
                                };

                                ListQuantity.Add(quantityInfo);
                                _logger.LogInformation("Added quantity: {Quantity}", quantityInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting quantity data");
            }
        }
    }

    public class QuantityInfo
    {
        public string Material_id { get; set; }
        public string Date_received { get; set; }
        public string Remaining_quantity { get; set; }
        
    }
}
