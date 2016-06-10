using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{
    public partial class ApiCoordinate : IHasId
    {
        public System.Guid ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public System.Guid ObjectTypeID { get; set; }
        public System.Guid CoordTypeID { get; set; }
        public string Coord { get; set; }
        public string Country { get; set; }
        public Nullable<int> Idx { get; set; }
        public string Ext { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }

        public Guid Id
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }
    }
}
