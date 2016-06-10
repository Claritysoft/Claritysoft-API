using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{
    public partial class ApiAccountBranch : IHasId
    {
        public System.Guid BranchId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public bool Primary { get; set; }
        public System.Guid AccountId { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }



        public Guid Id
        {
            get
            {
                return BranchId;
            }
            set
            {
                BranchId = value;
            }
        }
    }
}
