using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{

    public class ApiLeadBase : IHasId
    {
        public System.Guid LeadID { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public string Website { get; set; }
        public Nullable<System.Guid> ContactID { get; set; }
        public Guid? LeadSourceID { get; set; }
       
       

        public Guid Id
        {
            get { return this.LeadID; }
            set { this.LeadID = value; }
        }
    }
    /// <summary>
    /// This is used TO UPDATE database
    /// </summary>
    public class ApiLeadUpdateDatabase : ApiLeadBase
    {
        public CustomFieldValue[] CustomFields { get; set; }
        public string[] Tokens { get; set; }
    }
    public class ApiLeadReadFromDatabase : ApiLeadBase
    {
        /// <summary>
        /// This is used TO GET
        /// </summary>
        public ApiCustomObjectField[] Custom { get; set; }
    }
}
