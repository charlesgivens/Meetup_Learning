using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeetupApi.Models
{
  public class LectureDto
  {
    [Required]
    [MinLength(5)]
    public string Author { get; set; }
    [Required]
    [MinLength(5)]
    public string Topic { get; set; }
    public string Description { get; set; }
  }
}