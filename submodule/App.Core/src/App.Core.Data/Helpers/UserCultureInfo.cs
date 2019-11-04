using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Core.Data.Helpers
{
    /// <summary>  
    /// User's culture information.  
    /// </summary>  
    public class UserCultureInfo
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="UserCultureInfo"/> class.  
        /// </summary>  
        public UserCultureInfo()
        {
            // TODO: Need to through DB Context.  
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            TimeZone = timeZones.FirstOrDefault(el => el.Id == "FLE Standard Time" || el.Id == "EET");
            TimeZone = null;
            DateTimeFormat = "dd.MM.yyyy HH:m:ss"; // Default format.
        }
        /// <summary>  
        /// Gets or sets the date time format.  
        /// </summary>  
        /// <value>  
        /// The date time format.  
        /// </value>  
        public string DateTimeFormat { get; set; }
        /// <summary>  
        /// Gets or sets the time zone.  
        /// </summary>  
        /// <value>  
        /// The time zone.  
        /// </value>  
        public TimeZoneInfo TimeZone { get; set; }
        /// <summary>  
        /// Gets the user local time.  
        /// </summary>  
        /// <returns></returns>  
        public DateTime GetUserLocalTime()
        {
            //TODO : set value  TimeZoneInfo (temporary)
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Local);
        }
        /// <summary>  
        /// Gets the UTC time.  
        /// </summary>  
        /// <param name="datetime">The datetime.</param>  
        /// <returns>Get universal date time based on User's Timezone</returns>  
        public DateTime GetUtcTime(DateTime datetime)
        {
            return TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.Local).ToUniversalTime();
        }
    }
}
