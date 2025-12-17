using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class CreateCommentDto
    {
        
        [Required]
        [MinLength(5, ErrorMessage ="Title must be min 5 characters")]
        [MaxLength(300, ErrorMessage ="Title cannot be over 300 characters")]
        public string Title { get; set; }= string.Empty;
        [Required]
        [MinLength(5, ErrorMessage ="content must be min 5 characters")]
        [MaxLength(700, ErrorMessage ="content cannot be over 700 characters")]

        public string Content { get; set; }=string.Empty;
        
    }
}