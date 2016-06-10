using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
    public class ApiAccount : IHasId
    {

        public System.Guid AccountId { get; set; }
        public string Name { get; set; }
        public string CompanyPhone { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> OwnershipId { get; set; }
        public Nullable<System.Guid> IndustryId { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public System.Guid OwnerId { get; set; }
        public Nullable<bool> IsPrivate { get; set; }
        public string Website { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Country { get; set; }
        public string CoreName { get; set; }
        public string BranchName { get; set; }
        public string CountryName { get; set; }
        public Nullable<System.DateTime> DetailsStamp { get; set; }
        public bool HasShippingAddress { get; set; }
        public Nullable<System.Guid> FormID { get; set; }
        public Nullable<System.Guid> LeadID { get; set; }
        public Nullable<System.Guid> LeadSourceID { get; set; }
        public Nullable<System.Guid> PriceLevelID { get; set; }

        public Guid Id
        {
            get { return AccountId; }
            set { AccountId = value; }
        }
    }
}
