using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace web_team3_assignment.Models
{
    public class StudentSkillSetViewModel
    {
        public int StudentID { get; set; }

        public int SkillSetID { get; set; }

        public string SkillSetName { get; set; }

        public bool IsChecked { get; set; }
    }

    public class CheckBoxList
    {
        public List<StudentSkillSetViewModel> CheckBoxOptions { get; set; }
    }
}
