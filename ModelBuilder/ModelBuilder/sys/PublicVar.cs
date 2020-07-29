using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.sys
{
    /// <summary>
    /// 全局公共变量
    /// </summary>
    public static class  PublicVar
    {

        public static string sysName { set; get; }
        public static string myCompanyName { set; get; }
        public static string UserName { set; get; }
        public static string UserLoginName { set; get; }


        public static string GetUserName()
        {
            
            string RegBas = @"Software\Solidworks\Applications\ArgCenterUser\";
            RegistryKey myRegIetm = Registry.CurrentUser.CreateSubKey(RegBas, true);
            string UserName = null;
            UserName = myRegIetm.GetValue("LoginUserName") + "";
            myRegIetm.Close();
            return UserName;


        }
        public static void SetUserName(string LoginUserName)
        {
            string RegBas = @"Software\Solidworks\Applications\ArgCenterUser\";
            RegistryKey myRegIetm = Registry.CurrentUser.CreateSubKey(RegBas, true);
            myRegIetm.SetValue("LoginUserName", LoginUserName);
            myRegIetm.Close();

        }

    }
}
