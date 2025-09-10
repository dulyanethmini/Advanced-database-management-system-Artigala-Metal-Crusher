using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Order_table
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<Order_tableInfo> ListOrder_table { get; set; } = new List<Order_tableInfo>();

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

                    string sql = "SELECT Order_id, Size_id, Order_quantity, Order_type, Material_id  FROM Order_table";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Order_tableInfo order_tableInfo = new Order_tableInfo
                                {
                                    Order_id = reader["Order_id"].ToString(),
                                    Size_id = reader["Size_id"].ToString(),
                                    Order_quantity = reader["Order_quantity"].ToString(),
                                    Order_type = reader["Order_type"].ToString(),
Material_id = reader["Material_id"].ToString()
                                };

                                ListOrder_table.Add(order_tableInfo);
                                _logger.LogInformation("Added order_table: {Order_table}", order_tableInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting order_table data");
            }
        }
    }

    public class Order_tableInfo
    {
        public string Order_id { get; set; }
        public string Size_id { get; set; }
        public string Order_quantity { get; set; }
        public string Order_type { get; set; }
        public string Material_id { get; set; }
    }
}
