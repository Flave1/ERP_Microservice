using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deposit.Contracts.GeneralExtension
{
    public enum Doc_identifier
    {
        Signature = 1,
        Other_document = 2
    }
    public static class Amount_and_currency_pair
    {
        public static KeyValuePair<long, decimal> Of(long key, decimal value)
        {
            return new KeyValuePair<long, decimal>(key, value);
        }
    }
    public static class ConfirmationCode
    {
        private static Random random = new Random();
        public static string Generate()
        {
            const string chars = "!@#$//||%&*ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public static class Transaction_ID
    {
        private static Random random = new Random();
        public static string Generate()
        { 
            const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var chars = new string(Enumerable.Repeat(_chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());

            const string _num = "1234567890";
            var num = new string(Enumerable.Repeat(_num, 3).Select(s => s[random.Next(s.Length)]).ToArray());

            var date = DateTime.UtcNow.Date.ToString().Replace("/", "")[1];

            return $"{date}{chars}{num}";
        }
    }

    public class CustomEncoder
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
     
    public static class CustomValidators
    {
        public static bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
