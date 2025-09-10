using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Crushing_Process
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<Crushing_ProcessInfo> ListCrushing_Process { get; set; } = new List<Crushing_ProcessInfo>();

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

                    string sql = "SELECT Process_id, Operator_id, Material_id, Size_id, crushing_date FROM Crushing_Process";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Crushing_ProcessInfo crushing_ProcessInfo = new Crushing_ProcesslInfo
                                {
                                    Process_id = reader["Process_id"].ToString(),
                                    Operator_id = reader["Operator_id"].ToString(),
                                    Material_id = reader["Material_id"].ToString(),
                                    Size_id = reader["Size_id"].ToString(),
				     crushing_date = reader["crushing_date"].ToString()

                                };

                                ListCrushing_Process.Add(crushing_ProcessInfo);
                                _logger.LogInformation("Added crushing_Process: {Crushing_Process}", crushing_ProcessInfo);
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

    public class Crushing_ProcessInfo
    {
        public string Process_id { get; set; }
        public string Operator_id { get; set; }
        public string Material_id { get; set; }
        public string Size_id { get; set; }
	public string crushing_date { get; set; }
    }
}
