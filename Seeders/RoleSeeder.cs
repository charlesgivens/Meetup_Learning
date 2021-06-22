using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupAPI.Entities;

namespace MeetupApi
{
  public class RoleSeeder
  {
    private readonly MeetupContext _meetupContext;

    public RoleSeeder(MeetupContext meetupContext)
    {
      _meetupContext = meetupContext;
    }

    public void Seed()
    {
      if(_meetupContext.Database.CanConnect())
      {
        if(!_meetupContext.Roles.Any())
        {
          InsertSampleData();
        } else
        {
        }
      } else
      {
        Console.WriteLine("Error: Connecting to Db");
      }
    }

    private void InsertSampleData()
    {
      var roles = new List<Role>
      {
        new Role
        {
         RoleName = "User",
        },
        new Role
        {
         RoleName = "Moderator",
        },
        new Role
        {
         RoleName = "Admin",
        }
      };

      _meetupContext.AddRange(roles);
      _meetupContext.SaveChanges();

    }
  }
}