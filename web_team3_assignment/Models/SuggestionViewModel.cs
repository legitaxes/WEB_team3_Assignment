using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_team3_assignment.Models
{
    public class SuggestionViewModel
    {
        [Display(Name = "Suggestion ID")]
        public int SuggestionId { get; set; }

        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Display(Name = "Student ID")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Fill in the Description!")]
        [StringLength(3000, ErrorMessage = "Description Cannot Exceed 3000 Characters!")]
        public string Description { get; set; }

        public string Status { get; set; }
        public IEnumerable<SelectListItem> StatusList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Acknowledge", Value = "Y"},
                    new SelectListItem() { Text = "Not Acknowledged", Value = "N" }
                };
                return list.Select(l => new SelectListItem { Selected = (l.Value == Status.ToString()), Text = l.Text, Value = l.Value });
            }
        }
        public SelectListItem SelectedStatusItem { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
    }
}
