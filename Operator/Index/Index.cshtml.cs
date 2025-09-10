using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Operator
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<OperatorInfo> ListOperator { get; set; } = new List<OperatorInfo>();

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

                    string sql = "SELECT Operator_id, Contact_number, Password, Address FROM Operator";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OperatorInfo operatorInfo = new OperatorInfo
                                {
                                    Operator_id = reader["Operator_id"].ToString(),
                                    Contact_number = reader["Contact_number"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Address = reader["Address"].ToString()
                                };

                                ListOperator.Add(OperatorInfo);
                                _logger.LogInformation("Added Operator: {Operator}", operatorInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Operator data");
            }
        }
    }

    public class OperatorInfo
    {
        public string Operator_id { get; set; }
        public string Contact_number { get; set; }
        public string Password { get; set; }
        public string Address{ get; set; }
    }
}
