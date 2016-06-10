using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient.ApiClient
{
    public struct ExceptionInfo
    {
        public static ExceptionInfo Success
        {
            get { return new ExceptionInfo(); }
        }
        public string UserFriendlyError {get;set;}
        public string TechnicalError {get;set;}
        public bool HasValue
        {
            get { return !string.IsNullOrEmpty(UserFriendlyError) || !string.IsNullOrEmpty(TechnicalError); }
        }
        public override string ToString()
        {
            return string.Join(" ", UserFriendlyError, TechnicalError).Trim();
        }
    }
}
