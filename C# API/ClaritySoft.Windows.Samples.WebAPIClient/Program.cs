//#define USE_LOCAL_TESTS 
#undef USE_LOCAL_TESTS 
using ClaritySoft.Api.Client.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //ApiSampleTest.TestErrorReport();
            var sampleTest = new ApiSampleTest();
            
#if USE_LOCAL_TESTS
            sampleTest.API_URL = "http://localhost:26316/api/";
            sampleTest.YOURAPIKEY_LEADGENERATION = "c1_7MCEMLV62318";
            sampleTest.YOURAPIKEY_FULLACCESS = "c1_7MCEMLV62318";
            Task.WaitAll(sampleTest.TestGetCustomField());
            Task.WaitAll(sampleTest.TestPutCustomField());
#endif
            sampleTest.YOURAPIKEY_FULLACCESS = "**PUT YOUR KEY HERE**";
            Task.WaitAll(sampleTest.TestTasks());
            Task.WaitAll(sampleTest.TestPutAddress());
            Task.WaitAll(sampleTest.TestGetCustomField());

        }

        public static async Task TestErrorReport()
        {
            var url = "http://localhost:26316/api/ErrorReport";
            var r = new ApiReader(url, "");
            {
                await r.PostFile("",
                    new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string,string>("sourceObjectTypeId","1")
                        }, new byte[] { 1 }, "file", "attachment.zip"
                    );
            }
        }
    }
}
