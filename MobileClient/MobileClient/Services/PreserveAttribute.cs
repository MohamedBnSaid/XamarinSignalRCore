using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient.Services
{
    public sealed class PreserveAttribute : System.Attribute
    {
        public bool AllMembers;
        public bool Conditional;
    }
}
