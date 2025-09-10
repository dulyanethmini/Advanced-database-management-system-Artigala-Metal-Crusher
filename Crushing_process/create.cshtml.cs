using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Project.Pages.CrushingProcess
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CrushingProcessInfo CrushingProcess { get; set; }

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
                    string sql = "INSERT INTO Crushing_Process (Process_id, Operator_id, Material_id, Size_id, crushing_date) VALUES (@Process_id, @Operator_id, @Material_id, @Size_id, @crushing_date);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Process_id", CrushingProcess.Process_id);
                        command.Parameters.AddWithValue("@Operator_id", CrushingProcess.Operator_id);
                        command.Parameters.AddWithValue("@Material_id", CrushingProcess.Material_id);
                        command.Parameters.AddWithValue("@Size_id", CrushingProcess.Size_id);
                        command.Parameters.AddWithValue("@crushing_date", CrushingProcess.crushing_date);

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "New crushing process added successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while creating the crushing process: " + ex.Message;
                return Page();
            }
        }
    }

    public class CrushingProcessInfo
    {
        [Required]
        public int Process_id { get; set; }
        
        [Required]
        public int Operator_id { get; set; }
        
        [Required]
        public int Material_id { get; set; }
        
        [Required]
        public int Size_id { get; set; }
        
        [Required]
        public DateTime crushing_date { get; set; }
    }
}
