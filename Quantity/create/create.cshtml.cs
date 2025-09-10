using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Project.Pages.Quantity
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public QuantityInfo Quantity { get; set; }

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
                    string sql = "INSERT INTO Quantity (Material_id, Date_received, Remaining_quantity) VALUES (@Material_id, @Date_received, @Remaining_quantity); SELECT SCOPE_IDENTITY()";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Material_id", Quantity.Material_id);
                        command.Parameters.AddWithValue("@Date_received", Quantity.Date_received);
                        command.Parameters.AddWithValue("@Remaining_quantity", Quantity.Remaining_quantity);

                        // ExecuteScalar to get the generated ID
                        //int newQuantityId = Convert.ToInt32(command.ExecuteScalar());
                        //Quantity.Quantity_id = newQuantityId; // Uncomment if Quantity table has a Quantity_id auto-generated column
                    }
                }

                SuccessMessage = "New quantity record added successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while creating the quantity record: " + ex.Message;
                return Page();
            }
        }
    }

    public class QuantityInfo
    {
        [Required]
        public int Material_id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date_received { get; set; }

        [Required]
        public int Remaining_quantity { get; set; }
    }
}
