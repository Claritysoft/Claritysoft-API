using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient.ApiObjects
{

    public class ApiNote :IHasId
    {
        public System.Guid Id { get; set; }
        public System.Guid OwnerId { get; set; }
        public string Note { get; set; }
        public System.DateTime Date { get; set; }
    }
}
