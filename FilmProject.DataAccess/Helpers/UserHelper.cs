﻿namespace FilmProject.DataAccess.Helpers
{
    public class UserHelper
    {
        public static string CheckEmailorPhoneNum(string field)
        {
            var isEmail = field.Contains("@") ? "Email" : "PhoneNum";
            return isEmail;
        }
    }
}
