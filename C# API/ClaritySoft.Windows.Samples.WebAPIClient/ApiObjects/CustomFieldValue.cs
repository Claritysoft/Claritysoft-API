using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient.ApiObjects
{
    public struct CustomFieldValue
    {
        /// <summary>
        /// FieldID (GUID). this is preferred way to fill custom field
        /// </summary>
        public string FieldId { get; set; }
        /// <summary>
        /// You may use FieldName instead of FieldId, but FIeldId is ALWAYS preferable
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Value to fill
        /// </summary>
        public string FieldValue { get; set; }
        
    }
}
