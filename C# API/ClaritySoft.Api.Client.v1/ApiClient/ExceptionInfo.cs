using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Api.Client.ApiClient
{
    public struct ExceptionInfo
    {
        public static ExceptionInfo Success
        {
            get { return new ExceptionInfo(); }
        }
        public string UserFriendlyError {get;set;}
        public string TechnicalError {get;set;}
        public string Response { get; set; }
        public bool HasValue
        {
            get {
                return !string.IsNullOrEmpty(UserFriendlyError) 
                    || !string.IsNullOrEmpty(TechnicalError)
                    || !string.IsNullOrEmpty(Response);
                    
            }
        }
        public void ThrowIfError()
        {

            var ex = GenerateException();
            if (ex!=null)
            {
                throw ex;
            }
                
        }
        public Exception GenerateException()
        {
            if (HasValue)
            {
                if (!string.IsNullOrEmpty(Response))
                {
                    return new Exception(Response);
                }
                return new Exception(ToString());
            }
            return null;
        }
        public override string ToString()
        {
            if (TechnicalError!=null && UserFriendlyError!=null && TechnicalError.Contains(UserFriendlyError))
            {
                return TechnicalError;
            }
            return string.Join(" ", UserFriendlyError, TechnicalError).Trim();
        }
    }
}
