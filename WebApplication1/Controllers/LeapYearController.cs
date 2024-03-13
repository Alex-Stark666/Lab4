using System;
using Microsoft.AspNetCore.Mvc;
using LeapYearCalculator;

namespace LeapYearCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeapYearController : ControllerBase
    {
        [HttpGet("check/{year}")]
        public ActionResult<bool> CheckLeapYear(int year)
        {
            if (DateTime.IsLeapYear(year))
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpGet("interval")]
        public ActionResult<int> CalculateDateInterval(DateTime date1, DateTime date2)
        {
            TimeSpan interval = date2 - date1;
            return Ok((int)interval.TotalDays);
        }

        [HttpGet("weekday")]
        public ActionResult<string> OutputDayOfWeek(DateTime date)
        {
            return Ok(date.DayOfWeek.ToString());
        }
    }
}