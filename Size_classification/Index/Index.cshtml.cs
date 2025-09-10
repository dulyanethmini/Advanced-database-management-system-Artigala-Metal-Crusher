using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Size_classification
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<Size_classificationInfo> ListSize_classification { get; set; } = new List<Size_classificationInfo>();

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

                    string sql = "SELECT Size_id, size_description, Material_id,  FROM Size_classification";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Size_classificationInfo size_classificationInfo = new Size_classificationInfo
                                {
                                    Size_id = reader["Size_id"].ToString(),
                                    size_description = reader["size_description"].ToString(),
                                    Material_id = reader["Material_id"].ToString(),
                                    
                                };

                                ListSize_classification.Add(size_classificationInfo);
                                _logger.LogInformation("Added size_classification: {Size_classification}", size_classificationInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting size_classification data");
            }
        }
    }

    public class Size_classificationInfo
    {
        public string Size_id { get; set; }
        public string size_description { get; set; }
        public string Material_id { get; set; }
        
    }
}
