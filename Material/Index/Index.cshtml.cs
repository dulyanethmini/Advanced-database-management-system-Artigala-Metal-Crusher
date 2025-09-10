using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Material
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<MaterialInfo> ListMaterial { get; set; } = new List<MaterialInfo>();

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

                    string sql = "SELECT Material_id, Date_received, Material_Name, Loaded_Quantity FROM Material";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MaterialInfo materialInfo = new MaterialInfo
                                {
                                    Material_id = reader["Material_id"].ToString(),
                                    Date_received = reader["Date_received"].ToString(),
                                    Material_Name = reader["Material_Name"].ToString(),
                                    Loaded_Quantity = reader["Loaded_Quantity"].ToString()
                                };

                                ListMaterial.Add(materialInfo);
                                _logger.LogInformation("Added material: {Material}", materialInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting material data");
            }
        }
    }

    public class MaterialInfo
    {
        public string Material_id { get; set; }
        public string Date_received { get; set; }
        public string Material_Name { get; set; }
        public string Loaded_Quantity { get; set; }
    }
}
