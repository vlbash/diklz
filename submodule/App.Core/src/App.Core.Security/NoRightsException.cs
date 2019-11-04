using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Security
{
    public class NoRightsException: ApplicationException
    {
        public NoRightsException(string message = "There is no permission to perform this operation") : base(message)
        {
        }

        public NoRightsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
