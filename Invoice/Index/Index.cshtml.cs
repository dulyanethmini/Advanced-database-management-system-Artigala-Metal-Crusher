using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.Pages.Invoice
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<InvoiceInfo> ListInvoice{ get; set; } = new List<InvoiceInfo>();

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

                    string sql = "SELECT Invoice_id, Price, Tax, Date, Due_date, total,Order_id FROM Invoice";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InvoiceInfo invoiceInfo = new InvoiceInfo
                                {
                                    Invoice_id = reader["Invoice_id"].ToString(),
                                    Price = reader["Price"].ToString(),
                                    Tax = reader["Tax"].ToString(),
                                    Date = reader["Date"].ToString()
				    Due_date = reader["Due_date"].ToString()
				    total = reader["total"].ToString()
				    Order_id = reader["Order_id"].ToString()
                                };

                                ListInvoice.Add(invoiceInfo);
                                _logger.LogInformation("Added invoice: {Invoice}", invoiceInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Invoice data");
            }
        }
    }

    public class InvoiceInfo
    {
        public string Invoice_id { get; set; }
        public string Price { get; set; }
        public string Tax { get; set; }
        public string Date { get; set; }
	public string Due_date { get; set; }
	public string total { get; set; }
	public string Order_id { get; set; }
    }
}
