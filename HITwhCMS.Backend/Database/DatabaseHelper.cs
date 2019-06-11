using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ArLib.Console;
using HITwhCMS.Backend.DataTemplate;
using HITwhCMS.Backend.Utils;

namespace HITwhCMS.Backend.Database
{
    public class DatabaseHelper
    {
        /// <summary>
        /// MySQL connection.
        /// </summary>
        private MySqlConnection sqlcon;

        /// <summary>
        /// This string is used to connect MySQL.
        /// </summary>
        public string Str_connection
        {
            get
            {
                string tmp = "";

                tmp = "server=" + hostname + ";port=" + port + ";user=" + user + ";password=" + password + ";";
                tmp = tmp + "database=" + database + ";";

                return tmp;
            }

            private set { }
        }
        private string database = "";
        private readonly string port = "3306";
        private readonly string hostname = "192.168.200.132";
        private readonly string user = "root";
        private readonly string password = "Wzqhs666368";

        //private readonly string hostname = "localhost";
        //private readonly string user = "root";
        //private readonly string password = "Wzqhs666368";

        /// <summary>
        /// A flag indicating if the MySQL connection is alive.
        /// </summary>
        public bool bConnectionAlive { get; private set; } = false;

        /// <summary>
        /// Construct a DatabaseHelper to connect to a given database
        /// </summary>
        /// <param name="db_name">Expected database name we want to connect</param>
        public DatabaseHelper(string db_name = "hitwhccms")
        {
            ARConsole.CreateConsole(false, this.GetType().ToString(), false, "[WARNING]This logger should be removed in Release version");
            Console.BackgroundColor = ConsoleColor.Black;

            database = db_name;
        }

        /// <summary>
        /// Try to make a connection with MySQL
        /// </summary>
        /// <returns>Returns null if we successfully connected to server</returns>
        public Exception Connect()
        {
            sqlcon = new MySqlConnection(Str_connection);
            ARConsole.WriteLine("Built MySqlConnection");
            try
            {
                ARConsole.WriteLine("Please wait while we're logging into " + hostname);
                sqlcon.Open();

                bConnectionAlive = true;

                ARConsole.WriteLine("Connection to " + hostname + " Established.", MsgLevel.Harmless);
                return null;
            }
            catch(Exception ex)
            {
                ARConsole.WriteLine("Exception in DatabaseHelper::Connect(): " + ex.Message, MsgLevel.Critical);
                return ex;
            }
        }

        /// <summary>
        /// Try to wipe the connection when an action is done.
        /// </summary>
        /// <returns>Returns an exception if we got an error.</returns>
        public Exception Disconnect()
        {
            try
            {
                if (!bConnectionAlive) throw new Exception("Connection<" + hostname + "> has already closed.");
                sqlcon.Close();
                bConnectionAlive = false;
                return null;
            }
            catch(Exception ex)
            {
                ARConsole.WriteLine("Exception in DatabaseHelper::Disconnect(): " + ex.Message, MsgLevel.Critical);
                return ex;
            }
            finally
            {
                ARConsole.WriteLine("Wiped connection from host <" + hostname + ">.");
            }
        }

        public string GetStudentNumFromID(string id)
        {
            string id2 = AESHelper.Encrypt(id, AES_Static.AES256Key);
            string qstring = "SELECT * FROM user_basic WHERE sIDNumber='" + id2 + "'";


            try
            {
                if (!bConnectionAlive) throw new Exception("No live connection detected, check if there's previous error.");

                MySqlCommand query = new MySqlCommand(qstring, sqlcon);
                var reader = query.ExecuteReader();

                //Read only once for getting only the first record
                //If there's no match record, return a default StudentInfo
                // and set exSQL = null to let app know that this isn't an exception
                if (!reader.Read())
                {
                    return "";
                }

                return reader.GetString("sStudentID");
            }
            catch (Exception ex)
            {
                ARConsole.WriteLine("Exception in DatabaseHelper::RetrieveStudentInfo(): " + ex.Message, MsgLevel.Further);
                return "";
            }
        }

        public bool CheckIdentity(string id, string name)
        {
            string id2 = AESHelper.Encrypt(id, AES_Static.AES256Key);
            string qstring = "SELECT * FROM user_basic WHERE sIDNumber='" + id2 + "'";

            try
            {
                if (!bConnectionAlive) throw new Exception("No live connection detected, check if there's previous error.");

                MySqlCommand query = new MySqlCommand(qstring, sqlcon);
                var reader = query.ExecuteReader();

                //Read only once for getting only the first record
                //If there's no match record, return a default StudentInfo
                // and set exSQL = null to let app know that this isn't an exception
                if (!reader.Read())
                {
                    return false;
                }

                if (name != reader.GetString("sName")) return false;
            }
            catch (Exception ex)
            {
                ARConsole.WriteLine("Exception in DatabaseHelper::RetrieveStudentInfo(): " + ex.Message, MsgLevel.Further);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieve a built student info struct from the first querying record using the name given.
        /// </summary>
        /// <param name="id">The name given.</param>
        /// <param name="dataFormat">Which kind of data that will be retrieved.</param>
        /// <returns>Returns a built StudentInfo which contains expected info.</returns>
        public StudentInfo RetrieveStudentInfo(string id, DataFormat dataFormat)
        {
            string qstring = "SELECT * FROM user_basic WHERE sStudentID='" + id + "'";

            try
            {
                if (!bConnectionAlive) throw new Exception("No live connection detected, check if there's previous error.");
                if (dataFormat == DataFormat.NULL) throw new Exception("NULL data format is only used for exceptions, if you want to use NULL data please create yourself instead of using DataFormat.NULL.");

                MySqlCommand query = new MySqlCommand(qstring, sqlcon);
                var reader = query.ExecuteReader();

                //Read only once for getting only the first record
                //If there's no match record, return a default StudentInfo
                // and set exSQL = null to let app know that this isn't an exception
                if (!reader.Read())
                {
                    return new StudentInfo
                    {
                        CurrentFormat = DataFormat.NULL,
                        exSQL = null
                    };
                }

                //Build StudentInfo
                //Login stage, I made it containing standard stage data.
                //But why on the earth did I do this? This leaves 'Standard' option unuseful!
                var info = new StudentInfo()
                {
                    CurrentFormat = dataFormat,
                    exSQL = null,
                    sStudentID = id,
                    sPasswd = reader.GetString("sPasswd"),
                    sName = reader.GetString("sName"),
                    bRegistered = reader.GetBoolean("bRegistered"),
                    bAdmin = reader.GetBoolean("eUserGroup")
                };

                //Person Introdution stage
                if(dataFormat != DataFormat.Login && dataFormat != DataFormat.Standard)
                {
                    info.dAvatarIdx = reader.GetInt32("dAvatarIdx");
                    info.dSex = reader.GetInt32("dSex");
                    info.sNickname = reader.GetString("sNickname");
                    info.sBio = reader.GetString("sBio");
                    info.sTelephone = reader.GetString("sTelephone");
                    info.sEmail = reader.GetString("sEmail");
                    info.sProfession = reader.GetString("sProfession");
                    info.sBirthday = reader.GetString("sBirthday");

                    //Full stage
                    info.sLastLogin = (dataFormat == DataFormat.Full)? reader.GetString("sPasswd") : "" ;
                }

                ARConsole.WriteLine("MySQL connection is still active, don't forget to wipe it!", MsgLevel.Further);

                return info;
            }
            catch (Exception ex)
            {
                ARConsole.WriteLine("Exception in DatabaseHelper::RetrieveStudentInfo(): " + ex.Message, MsgLevel.Further);
                return new StudentInfo()
                {
                    CurrentFormat = DataFormat.NULL,
                    exSQL = ex
                };
            }
        }
    }
}
