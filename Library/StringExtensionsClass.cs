using System;

namespace SEV.Library
{
    public static class StringExtensionsClass
    {
        public static string ToTemplated(this string s, params object[] args)
        {
            string Templated = string.Format("%{0}%",s);
            return Templated;
        }
    }
}
