using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<LoginInfo> ListLogin { get; set; } = new List<LoginInfo>();

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

                    string sql = "SELECT Operator_id, Login_date, operation FROM Login";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LoginInfo loginInfo = new LoginInfo
                                {
                                    Operator_id = reader["Operator_id"].ToString(),
                                    Login_date = reader["Login_date"].ToString(),
                                    operation = reader["operation"].ToString()                                };

                                ListLogin.Add(loginInfo);
                                _logger.LogInformation("Added login: {Login}", loginInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting login data");
            }
        }
    }

    public class MaterialInfo
    {
        public string Operator_id { get; set; }
        public string Login_date { get; set; }
        public string operation { get; set; }
        
    }
}
