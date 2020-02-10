using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    internal interface IHasError
    {
        IError TheError { get; }
    }

    internal interface IError
    {
        string Message { get;  }
        uint Code { get;  }
        string Type { get;  }
    }

    //Grabs an error, if there was one, from the reply
    internal class Error : IError
    {
        private Error(uint code,
                      string type,
                      string message)
        {
            this.Code = code;
            this.Type = type;
            this.Message = message;
        }


        public string Message { get; }

        public uint Code { get;  }

        public string Type { get; }


        // JSON
        // TODO: Maybe put this into a factory??
        internal const string ERROR_OBJECT_PROPERTY_NAME = "error";
        internal const string CODE_PROPERTY_NAME = "code";
        internal const string TYPE_PROPERTY_NAME = "type";
        internal const string MESSAGE_PROPERTY_NAME = "message";

        internal const string EXCEPTION_NAME = "exception";

        public static IError FromJson(JObject json)
        {
            JToken errorValue = null;
            if (!json.TryGetValue(ERROR_OBJECT_PROPERTY_NAME, out errorValue))
            {
                // Sometimes the Deezer API will just shove back the 3 error properties
                // as the root object, rather than inside an 'error' property. 
                // Just gotta deal with it....
                errorValue = json;
            }

            uint code = errorValue.Value<uint>(CODE_PROPERTY_NAME);
            string type = errorValue.Value<string>(TYPE_PROPERTY_NAME);
            string message = errorValue.Value<string>(MESSAGE_PROPERTY_NAME);

            // Type will be either Exception or XXXException
            if (!string.IsNullOrEmpty(type) && type.ToLowerInvariant()
                                                   .Contains(EXCEPTION_NAME))
            {
                return new Error(code,
                                 type,
                                 message);
            }

            // No error parsed
            return null;
        }


        // Used for testing purposes
        internal static IError FromValues(uint code,
                                          string type,
                                          string message)
            => new Error(code, type, message);
    }
}
