using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient.ApiObjects
{
    public class ApiContact : IHasId
    {
        public bool HasEmail(string email)
        {
            return string.IsNullOrEmpty(this.Email) || this.Email == email || (AllEmails != null && AllEmails.Contains(email));

        }
        public System.Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }
        public string MiddleName { get; set; }
        public string SuffixName { get; set; }
        public Nullable<System.Guid> LeadSourceID { get; set; }
        public string LeadSourceOther { get; set; }
        public int RowConventionID { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> Anniversary { get; set; }
        public Nullable<System.Guid> OwnerID { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Country { get; set; }
        public string AllEmails { get; set; }
        public string AllAddresses { get; set; }
        public string CountryName { get; set; }
        public Nullable<System.DateTime> DetailsStamp { get; set; }
        public bool OptOutFromMassMail { get; set; }
        public bool IsPrimaryContact { get; set; }
        public string AllPhones { get; set; }
        public Nullable<System.Guid> FormID { get; set; }
        public string PrefixName { get; set; }
        public Nullable<System.Guid> LeadID { get; set; }

        public virtual ApiAccount Account { get; set; }
        //public List<ApiCoord> Coords { get; set; }
        //public List<ApiNote> ContactNotes { get; set; }
        //public List<ApiContactDetailData> Custom { get; set; }


        public Guid Id
        {
            get { return this.ContactId; }
            set { this.ContactId = value; }
        }
    }

}
