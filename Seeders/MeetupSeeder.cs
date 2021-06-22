using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupAPI.Entities;

namespace MeetupApi
{
  public class MeetupSeeder
  {
    private readonly MeetupContext _meetupContext;

    public MeetupSeeder(MeetupContext meetupContext)
    {
      _meetupContext = meetupContext;
    }

    public void Seed()
    {
      if(_meetupContext.Database.CanConnect())
      {
        if(!_meetupContext.Meetups.Any())
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
      var meetups = new List<Meetup>
      {
        new Meetup
        {
          Name = "Web Summit",
          Date = DateTime.Now.AddDays(7),
          IsPrivate = false,
          Organizer = "Microsoft",
          Location = new Location
          {
            City = "Krakow",
            Street = "Szeroka 33/5",
            PostCode = "33-337"
          },
          Lectures = new List<Lecture>
          {
            new Lecture
            {
              Author = "Bob Clark",
              Topic = "Modern Browsers",
              Description = "Deep Dive Into V8"
            }
          }
        },
        new Meetup
        {
          Name = "4Devs",
          Date = DateTime.Now.AddDays(7),
          IsPrivate = false,
          Organizer = "Microsoft2",
          Location = new Location
          {
            City = "Krakow2",
            Street = "Szeroka2 33/5",
            PostCode = "33-336"
          },
          Lectures = new List<Lecture>
          {
            new Lecture
            {
              Author = "Jan Doe",
              Topic = "Modern React",
              Description = "Deep Dive Into react-query"
            },
            new Lecture
            {
              Author = "Charles Givens",
              Topic = "Styled Components",
              Description = "Deep Dive Into Styled-Components"
            }
          }
        }
      };

      _meetupContext.AddRange(meetups);
      _meetupContext.SaveChanges();

    }
  }
}