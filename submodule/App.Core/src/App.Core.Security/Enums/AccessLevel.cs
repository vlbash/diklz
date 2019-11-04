using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Security
{
    // DO NOT change the order of elements in the enum
    // it has significant meaning in rights model
    public enum AccessLevel
    {
        No,
        Read,
        Write
    }
}
