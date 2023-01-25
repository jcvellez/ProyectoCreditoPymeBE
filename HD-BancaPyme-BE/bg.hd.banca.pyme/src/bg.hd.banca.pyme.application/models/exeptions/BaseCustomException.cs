using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class BaseCustomException : Exception
    {
        public int Code { get; }
        public override string StackTrace { get; }

        public BaseCustomException(string message, string stackTrace, int code) : base(message)
        {
            Code = code;
            StackTrace = stackTrace;
        }
        public BaseCustomException(string message, string stackTrace, int code, string? source) : base(message)
        {
            Code = code;
            StackTrace = stackTrace;
            this.Source = source;
        }
        public BaseCustomException(string message, string stackTrace, int code, string? source, string? helpLink) : base(message)
        {
            Code = code;
            StackTrace = stackTrace;
            this.Source = source;
            this.HelpLink = helpLink;
        }
    }
}
