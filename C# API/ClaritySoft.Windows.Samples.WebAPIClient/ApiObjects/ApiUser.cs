using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient.ApiObjects
{
    public class ApiUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Guid? ReportsTo { get; set; }
        public bool? IsAdmin { get; set; }
        public bool IsInactive { get; set; }
        public Guid? TimeZoneID { get; set; }
        public string CultureId { get; set; }
        public string Profile { get; set; }

    }
}
