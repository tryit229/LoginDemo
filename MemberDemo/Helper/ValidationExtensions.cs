using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MemberDemo.Helper
{
    public static class ValidationExtensions
    {
        private static Validation Check<T>(this Validation validation, Func<bool> filterMethod, T exception) where T : Exception
        {
            if (filterMethod())
            {
                return validation ?? new Validation() { IsValid = true };
            }
            else
            {
                throw exception;
            }
        }

        //public static Validation Check(this Validation validation, Func<bool> filterMethod)
        //{
        //    return Check<Exception>(validation, filterMethod, new Exception("Parameter InValid!"));
        //}

        //public static Validation NotNull(this Validation validation, Object obj)
        //{
        //    return Check<ArgumentNullException>(
        //        validation,
        //        () => obj != null,
        //        new ArgumentNullException(string.Format("Parameter {0} can't be null", obj))
        //    );
        //}


        public static Validation IsEmail(this Validation validation, string email)
        {
            return Check<ArgumentException>(
                validation,
                () => Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"),
                new ArgumentException(string.Format("請確認email格式是否正確"))
            );
        }

        public static Validation IsPasswordFomat(this Validation validation, string pwd)
        {
            return Check<ArgumentException>(
                validation,
                () => Regex.IsMatch(pwd, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,30}$"),
                new ArgumentException(string.Format("請確認密碼是否符合規則"))
            );
        }
    }
}