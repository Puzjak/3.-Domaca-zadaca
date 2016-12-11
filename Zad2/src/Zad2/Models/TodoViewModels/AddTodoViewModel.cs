using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zad2.Models.TodoViewModels
{
    public class AddTodoViewModel
    {
        [Required, MinLength(1)]
        [DataType(DataType.Text)]
        [Display(Name = "Text")]
        public string Text { get; set; }
    }
}
