using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeetupApi.Models
{
  public class MeetupDto
  {
    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    [Required]
    public string Organizer { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public bool isPrivate { get; set; }
  }
}