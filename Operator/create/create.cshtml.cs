using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Project.Pages.Operator
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public OperatorInfo Operator { get; set; }

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
                    string sql = "INSERT INTO Operator (Operator_id, Contact_number, Password, Address) VALUES (@Operator_id, @Contact_number, @Password, @Address); SELECT SCOPE_IDENTITY()";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Operator_id", Operator.Operator_id);
                        command.Parameters.AddWithValue("@Contact_number", Operator.Contact_number);
                        command.Parameters.AddWithValue("@Password", Operator.Password);
                        command.Parameters.AddWithValue("@Address", Operator.Address);

                        // ExecuteScalar to get the generated ID
                        //int newOperatorId = Convert.ToInt32(command.ExecuteScalar());
                        //Operator.Operator_id = newOperatorId;
                    }
                }

                SuccessMessage = "New operator added successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while creating the operator details: " + ex.Message;
                return Page();
            }
        }
    }

    public class OperatorInfo
    {
        [Key]
        public int Operator_id { get; set; }

        [Required]
        [StringLength(20)]
        public string Contact_number { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }
    }
}
