using EasyDDD.Infrastructure.Crosscutting.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Infrastructure.Exceptions
{
    public class CheckArgument
    {
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty(string argument, string argumentName)
        {
            if (String.IsNullOrEmpty(argument))
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.ArgumentCannotBeNullOrEmptyString, ErrorMessage.ArgumentCannotBeNullOrEmptyString.FormatWith(argumentName));
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty<T>(IList<T> argument, string argumentName)
        {
            if (argument == null || argument.Count <= 0)
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.ArgumentCannotBeNullOrEmptyString, ErrorMessage.ArgumentCannotBeNullOrEmptyString.FormatWith(argumentName));
            }
        }

        /// <summary>
        /// Asserts that an object is not null
        /// </summary>
        /// <param name="argument">Argument</param>
        /// <param name="argumentName">Argument name</param>
        /// <exception cref="ArgumentException">If the argument is null.</exception>
        [DebuggerStepThrough]
        public static void IsNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.ArgumentCannotBeNull, ErrorMessage.ArgumentCannotBeNull.FormatWith(argumentName));
            }
        }
    }
}
