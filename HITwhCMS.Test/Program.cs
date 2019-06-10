using System;
using HITwhCMS.Backend.Database;
using HITwhCMS.Backend.DataTemplate;
using HITwhCMS.Backend.Utils;

namespace HITwhCMS.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var instance = new DatabaseHelper();

            //instance.Connect();
            //foreach (var s in instance.Query("select `sPasswd` from hitwhcms.accounts where `sStudentID`='admin'", "sPasswd", DatabaseHelper.TableType.Full))
            //{
            //    Console.WriteLine(s);
            //}
            //instance.Disconnect();
            string key = "haerbingongyedaxueweihai-1604102";
            //string key = "66666666666666666666666666666666";
            string enc = AESHelper.Encrypt(@"430521", key);
            //string dec = AESHelper.Decrypt(enc, key);
            string dec = AESHelper.Decrypt(enc, key);
            Console.WriteLine(enc);
            Console.WriteLine(dec);
            //StudentInfo test = new StudentInfo()
            //{
            //    CurrentFormat = DataFormat.Full,
            //    exSQL = new Exception("This is 1")
            //};
            //Console.WriteLine(test.exSQL.Message);
            //test.exSQL = new Exception("This has been changes.");
            //Console.WriteLine(test.exSQL.Message);
        }
    }
}
