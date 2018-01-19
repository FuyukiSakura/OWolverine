﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities
{
    public static class StringHelper
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0 || toCheck == "";
        }
    }
}
