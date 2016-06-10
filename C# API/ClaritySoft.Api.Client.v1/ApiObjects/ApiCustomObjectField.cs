using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiObjects
{
    public class ApiCustomObjectField
    {
        public System.Guid ObjectDetailDataId { get; set; }
        public System.Guid ObjectDetailFieldId { get; set; }
        public System.Guid ParentId { get; set; }
        public string Data { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> DateData { get; set; }
        public Nullable<decimal> NumericData { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> DetailsStamp { get; set; }

        public virtual ApiObjectDetailField Field { get; set; }
    }
    public partial class ApiObjectDetailField
    {

        public System.Guid ObjectDetailFieldId { get; set; }
        public System.Guid SourceObjectTypeID { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string DataFormat { get; set; }
        public string DropDownValues { get; set; }
        public string Description { get; set; }
        public short Order { get; set; }
        public bool Enabled { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }


    }
    public class ClarityCustomFieldDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        // property type, etc
        public CustomFieldPropertyType Type { get; set; }
    }
    public enum CustomFieldPropertyType
    {
        Text,
        Numeric,
        DateTime
    }
    public class ClarityCustomFieldValue
    {
        /// <summary>
        ///  This is for obsolete schema only (ID of field object)
        /// </summary>
        public Guid? ValueId { get; set; }
        public ClarityCustomFieldDefinition Field { get; set; }
        public string FieldMappingAlias { get; set; }
        public string ValueString { get; set; }
        public decimal? ValueInt { get; set; }
        public DateTime? ValueDate { get; set; }
    }
}
