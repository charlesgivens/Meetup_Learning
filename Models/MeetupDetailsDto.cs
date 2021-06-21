using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Models
{
  public class MeetupDetailsDto
  {
    public string Name { get; set; }
    public string Organizer { get; set; }
    public DateTime Date { get; set; }
    public bool isPrivate { get; set; }

    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }

    public List<LectureDto> Lectures { get; set; }
  }
}