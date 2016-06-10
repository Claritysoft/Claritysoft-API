using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{
    public class ApiActivity : IHasId
    {
        public System.Guid ActivityId { get; set; }
        public Nullable<System.Guid> ContactId { get; set; }
        public Nullable<System.Guid> ActivityTypeId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public string UsedCoord { get; set; }
        public Nullable<System.Guid> SourceObjectID { get; set; }
        public Nullable<System.Guid> SourceObjectTypeID { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.Guid> CaseID { get; set; }
        public Nullable<System.Guid> OwnerID { get; set; }
        public Nullable<System.Guid> CampaignId { get; set; }
        public ApiContact Contact { get; set; }

        

        public Guid Id
        {
            get { return ActivityId; }
            set { ActivityId = value; }
        }

        
    }
}
