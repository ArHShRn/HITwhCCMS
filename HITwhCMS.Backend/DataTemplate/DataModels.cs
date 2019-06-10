using System;

namespace HITwhCMS.Backend.DataTemplate
{
    /// <summary>
    /// Choose to retrieve corresponding data struct.
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// A struct contains nothing
        /// </summary>
        NULL = -1,
        /// <summary>
        /// A struct contains data needed for validating login info
        /// </summary>
        Login,
        /// <summary>
        /// A struct contains standard info
        /// </summary>
        Standard,
        /// <summary>
        /// A struct contains personal info
        /// </summary>
        PersIntro,
        /// <summary>
        /// A struct contains full info
        /// </summary>
        Full
    };

    /// <summary>
    /// A data model of a user, containing everything essential.
    /// </summary>
    public struct StudentInfo
    {
        public DataFormat CurrentFormat;

        private bool bExSQLSetOnce;

        /// <summary>
        /// A flag makes that exSQL can only be set once.
        /// </summary>
        private Exception _exSQL;
        /// <summary>
        /// The exception that occurs
        /// </summary>
        public Exception exSQL
        {
            get { return _exSQL; }
            set
            {
                if (bExSQLSetOnce)
                {
                    value = _exSQL;
                    return;
                }

                _exSQL = value;
                bExSQLSetOnce = true;
            }
        }

        //Login data
        public string sStudentID;
        public string sPasswd;
        public string sName;
        public bool bRegistered;

        //Plus standard data
        public bool bAdmin;

        //Plus personal introduction data
        public int dAvatarIdx;          //
        public int dSex;                //0-Default 1-Male 2-Female
        public string sNickname;
        public string sBio;
        public string sTelephone;
        public string sEmail;
        public string sProfession;
        public string sBirthday;

        //Plus full data
        public string sLastLogin;       //Format refers to sBirthday

    }

    /// <summary>
    /// A data model of a set of CC records, formated, listed.
    /// </summary>
    public struct CampusCardRecord
    {
        public DataFormat CurrentFormat;

        //TO-DOs: Add variables
    }

    /// <summary>
    /// A data model of a real-time-fetched activity
    /// </summary>
    public struct ActivityInfo
    {
        public string Website;
        public string TitleLevel1;
        public string TitleLevel2;
    }

    /// <summary>
    /// A standard data model, for dev use only.
    /// </summary>
    public static class StandardDataTemplate
    {
        /// <summary>
        /// Dev activity.
        /// </summary>
        public static ActivityInfo activityInfo = new ActivityInfo()
        {
            Website = "来自 160400219 何凯",
            TitleLevel1 = "2019年05月29日",
            TitleLevel2 = "\"我在学子附近捡到一张校园卡，" + Environment.NewLine + "卡号是1604102xx，请尽快联系我！\""
        };
    }
}
