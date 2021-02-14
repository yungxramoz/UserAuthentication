using System;
using System.Globalization;

namespace UserAuthentication.Helpers
{
    public class InfoException: Exception
    {
        public InfoException() : base() { }

        public InfoException(string message) : base(message) { }

        public InfoException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
