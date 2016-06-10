using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClaritySoft.Api.Client.ApiClient;
using ClaritySoft.Api.Client.ApiObjects;

namespace ClaritySoft.Windows.Samples.WebAPIClient
{
    public class ApiSampleTest
    {
        /// <summary>
        /// Use these constants to set NULL values on PUT only
        /// </summary>
        static readonly Guid SET_NULL_GUID = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
        static readonly string SET_NULL_STRING = new string(' ', 2); // two spaces
        static readonly DateTime SET_NULL_DATETIME = new DateTime(1800, 1, 1);
        static readonly int SET_NULL_NUMERIC = Int16.MinValue; //-32768



        public string YOURAPIKEY_FULLACCESS = "YOURDBNAME_YOURDOMAIN";
        public string YOURAPIKEY_LEADGENERATION = "YOURDBNAME_YOURDOMAIN";

        Guid testAccountId = new Guid("f590d99d-b5ff-4cac-9eee-517838d30907");
        public Guid TestAccountId
        {
            get { return testAccountId; }
            set { testAccountId = value; }
        }

    

        public string API_URL = "https://api.claritycrm.com/api/v1/";

        // this sample will delete leads having that email
        public string TEST_NONEXISTENT_EMAIL = "mynonexistingemail@nonexisting.domain";
        ApiReader CreateApiReader(string relativeUrl)
        {
            var reader = new ApiReader(relativeUrl, "");
            reader.BaseUri = new Uri(API_URL);
            return reader;

        }
        public async Task GetAllUsers()
        {
            var reader = CreateApiReader("users");
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var queryUsers = await reader.QueryList<ApiUser>();
            // users is read-only
        }
        public async Task QueryAllLeads()
        {
            var reader = CreateApiReader("leads");
            // use API key with full permissions (Module = CS.GENERIC) 
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var queryAllLeads = (await reader.QueryList<ApiLeadReadFromDatabase>()).ToArray();

            var leadWithAtLeastOneCustomField = queryAllLeads.FirstOrDefault(l => l.Custom != null && l.Custom.Length > 0);
            // or
            var oDataFilter = ApiReader.BuildFilter("FirstName eq {0}", "Stan");
            var queryLeadsWhoseFirstNameIsStan = reader.QueryListByFilter<ApiLeadReadFromDatabase>(oDataFilter);
            // you can view result in browser by pasting this url
            Console.WriteLine(reader.LastUrlUsedToQuery);
        }

        public async Task TestCreateApiAccount()
        {
            var reader = CreateApiReader("accounts");
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var accObj = new ApiAccount
            {
                Name = "CreatedFromAPI"
            };
            await reader.Post(accObj);
        }
        public async void UpdateAccountDescription()
        {
            var reader = CreateApiReader("accounts");
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var accObj = new ApiAccount
            {
                AccountId = this.TestAccountId,
                Description = "Updated from API"
            };
            await reader.Put(accObj);
        }
        public async Task QueryAllContacts()
        {
            var reader = CreateApiReader("contacts");
            // use API key with full permissions (Module = CS.GENERIC) 
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var queryAllLeads = (await reader.QueryList<ApiContact>()).ToArray();


            // you can view result in browser by pasting this url
            Console.WriteLine(reader.LastUrlUsedToQuery);
        }
        public async Task DeleteLead()
        {
            var reader = CreateApiReader("leads");
            // use API key with full permissions (Module = CS.GENERIC) 
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var oDataFilter = ApiReader.BuildFilter("Email eq {0}", TEST_NONEXISTENT_EMAIL);
            foreach (var obj in await reader.QueryListByFilter<ApiLeadReadFromDatabase>(oDataFilter))
            {
                await reader.Delete(obj.LeadID);
            }

        }

        public async Task PopulateLead()
        {

            // with leadspopulation, you can use API key limited to module CS.LEADS.GENERATION
            // leadspopulation only supports creating new lead (you cannot query existing information)
            var reader = CreateApiReader("leadspopulation");
            // you can use "api/leads" as well, but that would require API key with administrative permissions
            reader.ApiKey = YOURAPIKEY_LEADGENERATION;
            var postLead = new ApiLeadUpdateDatabase();
            // specify email if possible - it will be used for deduplication purposes
            postLead.Email = TEST_NONEXISTENT_EMAIL;
            // if email is not specified, ClaritySoft will try to use deduplication by mobile number or phone number
            // FirstName is mandatory
            postLead.FirstName = "Stan";

            // LeadSource is mandatory. To see list of LeadSources, use link below, having replaced YOURDBNAME_YOURAPIKEY with your actual API key
            //https://api.claritycrm.com/api/leads/leadsources?key=YOURDBNAME_YOURAPIKEY

            // specify CustomFields[] array to populate any custom field values

            /* When using custom fields, use IDs rather than Names. Names may change
             To view all custom field definitions for Leads, use link below, having replaced YOURDBNAME_YOURAPIKEY with your actual API key
            https://api.claritycrm.com/api/leads/customfieldsdefinition?key=YOURDBNAME_YOURAPIKEY
            
            
             * You will get result like this
             * [{"Tasks":[],"ObjectTypeID":"b50b1c50-bfba-4333-b888-ba451e553d91","Name":"Website","Description":"","Order":7,"Enabled":true,"OwnerUserID":"2373c6a8-938d-4588-9bb4-6a1e642d721e","SourceID":"","DeletedOn":null,"LastEditDate":"2014-09-19T03:33:32.71","CreationDate":"2014-07-30T17:04:12.657","XmlData":"","Code":"","Id":"d3a691c9-1564-4802-8da5-016415ec54fd","ParentObjectID":null},
             * Use Id to specify LeadSourceId on the custom field
             */
            postLead.LeadSourceID = new Guid("b50b1c50-bfba-4333-b888-ba451e553d91");
            // alternatively, if you do NOT know Id but you know name of lead source, you can use Tokens collection
            //postLead.Tokens = new string[] { "LeadSource:Website" };


            postLead.CustomFields = new CustomFieldValue[]
            {
                new CustomFieldValue
                {
                    FieldId = "4bff5df0-fb9d-4c4b-901b-f8fd36b31137",
                    //FieldName = "test", - if you specified FieldId, no need to use FieldName
                    FieldValue = "03/23/2015" // datetime - must be in U.S. locale
                },
                new CustomFieldValue
                {
                    FieldId="e1ff5736-c5fc-4ace-bdac-58be0bfb6ccf",
                    FieldValue = "test3"
                }
            };
            await reader.Post(postLead);
            // or update using Put operation - however api/leadspopulation only supports POSTS
            //Console.WriteLine(returnValue.Id.ToString());
        }
        public async Task TestGetCustomField()
        {
            var accountId = new Guid("39CB3A00-A4E5-4022-BB96-00020D154C9E");
            var reader = CreateApiReader("accounts");
            reader.ApiKey = this.YOURAPIKEY_FULLACCESS;
            var allCustomFields = (await reader.QueryList<ClarityCustomFieldDefinition>("/CustomFieldsDefinition")).ToArray();
            var checkCustomFieldsByObject = (await reader.QueryList<ClarityCustomFieldValue>("/GetFields?id=" + accountId.ToString())).ToArray();
        }
        public async Task TestPutCustomField()
        {
            var accountId = new Guid("39CB3A00-A4E5-4022-BB96-00020D154C9E");
            var reader = CreateApiReader("accounts");
            reader.ApiKey = this.YOURAPIKEY_FULLACCESS;
            var checkFieldId = new Guid("00A1BDE2-9415-4100-96A0-217583DEBDB3");
            var fieldObj = new ClarityCustomFieldValue[]
            {
                 new ClarityCustomFieldValue
                 {
                     Field = new ClarityCustomFieldDefinition { Id = checkFieldId },
                     ValueInt = 0.01M
                 }

            };
            await reader.Put("UpdateFields", accountId, fieldObj);

        }
        public async Task TestPutAddress()
        {
            var reader = CreateApiReader("accountaddresses");
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var newAddress = new ApiAccountAddress();
            newAddress.AccountId = new Guid("075660f7-6a07-47bc-a05d-f039fc9ad3a7");
            newAddress.Address1 = "10550 S. Sam Houston Parkway West";
            newAddress.City = "Houston";
            await reader.Put(newAddress);
        }
        public async Task TestTasks()
        {
            var reader = CreateApiReader("tasks");
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            var allTasks = (await reader.QueryList<ApiTask>()).ToArray();
            var t = allTasks.FirstOrDefault();
            // change category
            // use the following URL to look up Category1
            var checkCategory1Url = "https://api.claritycrm.com/api/v1/namedobjects?key=API_KEY_GOES_HERE&$filter=ObjectTypeID%20eq%20guid%2742C44051-225D-42F4-8249-798AE742735E%27".Replace("API_KEY_GOES_HERE", YOURAPIKEY_FULLACCESS);
            var checkCategory2Url = "https://api.claritycrm.com/api/v1/namedobjects?key=API_KEY_GOES_HERE&$filter=ObjectTypeID%20eq%20guid%2739FE180C-46C1-42C0-8F1B-3A37B669C10E%27".Replace("API_KEY_GOES_HERE", YOURAPIKEY_FULLACCESS);
            // todo: set ID to one you'll look up above
            //t.CategoryId1 = ID_GOES_HERE
            // await reader.Put(...)
        }
        

    
        public async Task TestAddresses()
        {
            var reader = CreateApiReader("accountaddresses");
            reader.ApiKey = YOURAPIKEY_FULLACCESS;
            // check existing addresses
            var allAddresses = await reader.QueryList<ApiAccountAddress>();
            // if we do not have an address, create one
            
            if (!(await reader.QueryListByFilter<ApiAccountAddress>(ApiReader.BuildFilter("AccountId eq {0}", testAccountId))).Any())
            {
                // an address is separate object and it has its own ID: BranchID
                // however, BranchId will be phased out

                // you should use AccountId instead.


                var newAddress = new ApiAccountAddress();
                // make sure you specify valid AccountId
                newAddress.AccountId = testAccountId;
                newAddress.Address1 = "123 Main Street";
                newAddress.City = "New York";
                newAddress.Country = "US";
                await reader.Post(newAddress);

                // to update address, also use specify valid AccountId; remove BranchId from ApiAccountAddress object.
                var postAddress = new ApiAccountAddress();
                postAddress.AccountId = testAccountId;
                postAddress.Address1 = "Another address";
                await reader.Put(testAccountId, postAddress);
            }

            // delete


            await reader.Delete(testAccountId);


        }
    }
}