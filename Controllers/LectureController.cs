using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeetupAPI.Entities;
using AutoMapper;
using MeetupApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupApi.Controllers
{
  [Route("api/meetup/{meetupName}/lecture")]
  public class LectureController : ControllerBase
  {
    private readonly MeetupContext _meetupContext;
    private readonly IMapper _mapper;

    public LectureController(MeetupContext meetupContext, IMapper mapper)
    {
      _meetupContext = meetupContext;
      _mapper = mapper;
    }

    [HttpGet]
    public ActionResult Get(string meetupName)
    {
      var meetup = _meetupContext.Meetups
        .Include(m => m.Lectures)
        .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

      if( meetup == null)
      {
        return NotFound();
      }

      var lectures = _mapper.Map<List<LectureDto>>(meetup.Lectures);

      if( lectures == null)
      {
        return NotFound();
      }

      return Ok(lectures);
    }

    [HttpPost]
    public ActionResult Post(string meetupName, [FromBody] LectureDto model)
    {
      if(!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var meetup = _meetupContext.Meetups
        .Include(m => m.Lectures)
        .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

      if( meetup == null)
      {
        return NotFound();
      }

      var lecture = _mapper.Map<Lecture>(model);
      meetup.Lectures.Add(lecture);
      _meetupContext.SaveChanges();

      return Created($"api/meetup/{meetupName}", null);
    }

    [HttpDelete]
    public ActionResult Delete(string meetupName)
    {
      var meetup = _meetupContext.Meetups
        .Include(m => m.Lectures)
        .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

      if( meetup == null)
      {
        return NotFound();
      }

      _meetupContext.Lectures.RemoveRange(meetup.Lectures);
      _meetupContext.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string meetupName, int id)
    {
      var meetup = _meetupContext.Meetups
        .Include(m => m.Lectures)
        .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

      if(meetup == null)
      {
        return NotFound();
      }

      var lecture = meetup.Lectures.FirstOrDefault(l => l.Id == id);

      if(lecture == null)
      {
        return NotFound();
      }

      _meetupContext.Lectures.Remove(lecture);
      _meetupContext.SaveChanges();

      return NoContent();
    }

  }
}