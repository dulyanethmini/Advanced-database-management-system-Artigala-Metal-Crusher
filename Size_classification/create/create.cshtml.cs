using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Project.Pages.SizeClassification
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public SizeClassificationInfo SizeClassification { get; set; }

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
                    string sql = "INSERT INTO Size_classification (Size_id, size_description, Material_id) VALUES (@Size_id, @size_description, @Material_id)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Size_id", SizeClassification.Size_id);
                        command.Parameters.AddWithValue("@size_description", SizeClassification.size_description);
                        command.Parameters.AddWithValue("@Material_id", SizeClassification.Material_id);

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "New size classification added successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while creating the size classification: " + ex.Message;
                return Page();
            }
        }
    }

    public class SizeClassificationInfo
    {
        [Required]
        public int Size_id { get; set; }

        [Required]
        [StringLength(255)]
        public string size_description { get; set; }

        [Required]
        public int Material_id { get; set; }
    }
}
