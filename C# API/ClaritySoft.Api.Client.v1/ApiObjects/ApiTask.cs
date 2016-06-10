using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{
    public partial class ApiTask
    {
        public System.Guid TaskId { get; set; }
        public Nullable<System.Guid> ActivityTypeId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<System.Guid> ContactId { get; set; }
        public string Status { get; set; }
        public System.Guid AssignedToId { get; set; }
        public System.Guid OwnerId { get; set; }
        public string Priority { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public ApiTaskCategory CategoryId { get; set; }
        public string Location { get; set; }
        public string Recurrence { get; set; }
        public Nullable<bool> AllDayEvent { get; set; }
        public Nullable<bool> ReminderEnabled { get; set; }
        public Nullable<int> ReminderInterval { get; set; }
        public Nullable<bool> IsCalendar { get; set; }
        public System.DateTime LastEditDate { get; set; }
        public System.DateTime Creationdate { get; set; }
        public Nullable<System.Guid> SourceObjectID { get; set; }
        public Nullable<System.Guid> SourceObjectTypeID { get; set; }
        public Nullable<System.Guid> TimeZoneID { get; set; }
        public Nullable<System.Guid> CaseID { get; set; }
        public Nullable<System.Guid> CategoryId1 { get; set; }
        public Nullable<System.Guid> CategoryId2 { get; set; }
        public Nullable<System.Guid> LeadID { get; set; }
        public Nullable<System.Guid> LeadSourceID { get; set; }
        public Nullable<System.Guid> CampaignId { get; set; }
        public string AssociatedEmails { get; set; }

        public ApiContactInfo MVContact { get; set; }
    }
    public enum ApiTaskCategory
    {
        Undefined=0,
        Ideas=1,
        CustomerRelated=2,
        Personal=3,
        Strategies=4,
        Presentation=5
    }
}
