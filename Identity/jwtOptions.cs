using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeetupApi.Identity
{
  public class JwtOptions
  {
    public string JwtKey { get; set; }
    public string JwtIssuer { get; set; }
    public int JwtExpireDays { get; set; }
  }
}