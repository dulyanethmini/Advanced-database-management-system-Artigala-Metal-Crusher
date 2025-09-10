using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Project.Pages.Invoice
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public InvoiceInfo Invoice { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Project;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Invoice (Invoice_id, Price, Tax, Date, Due_date, total, Order_id) VALUES (@Invoice_id, @Price, @Tax, @Date, @Due_date, @total, @Order_id);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Invoice_id", Invoice.Invoice_id);
                        command.Parameters.AddWithValue("@Price", Invoice.Price);
                        command.Parameters.AddWithValue("@Tax", Invoice.Tax);
                        command.Parameters.AddWithValue("@Date", Invoice.Date);
                        command.Parameters.AddWithValue("@Due_date", Invoice.Due_date);
                        command.Parameters.AddWithValue("@total", Invoice.total);
                        command.Parameters.AddWithValue("@Order_id", Invoice.Order_id);

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "New invoice added successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while creating the invoice: " + ex.Message;
                return Page();
            }
        }
    }

    public class InvoiceInfo
    {
        [Required]
        public int Invoice_id { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime Due_date { get; set; }
        [Required]
        public decimal total { get; set; }
        [Required]
        public int Order_id { get; set; }
    }
}
