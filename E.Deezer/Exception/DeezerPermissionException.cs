using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer
{
    public class DeezerPermissionsException : Exception
    {
        internal const string EXCEPTION_MESSAGE_FORMAT = "The provided access token doesn't provide '{0}' permission and so this operation can't be performed. Please ensure token hasn't expired.";
        internal const string UNKNOWN_EXCEPTION_MESSAGE = "The provided access token doesn't provide the correct permissions and so this operation can't be performed. Please ensure token hasn't expired.";


        public DeezerPermissionsException(DeezerPermissions permission)
        {
            bool hasFormattedText = Permissions.PERMISSION_NAME_LOOKUP.ContainsKey(permission);

            this.Message = hasFormattedText ? string.Format(EXCEPTION_MESSAGE_FORMAT, Permissions.PERMISSION_NAME_LOOKUP[permission])
                                            : UNKNOWN_EXCEPTION_MESSAGE;
        }


        public override string Message { get; }
    }
}
