using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.sys
{

    /// <summary>
    /// 处理注册表数据
    /// </summary>
    public static class RegDeal
    {
        /// <summary>
        /// 注册表节点
        /// </summary>
        public static string RegBas = @"Software\Solidworks\Applications\ArgCenter\";

        /// <summary>
        /// 删除注册表数据
        /// </summary>
        public static void DelRegList()
        {
            //删除注册表所有文件列表
            RegistryKey myRegIetm = Registry.CurrentUser.OpenSubKey(RegBas, true);

            string[] myItem = null;

            myItem = myRegIetm.GetValueNames();

            //删除数据
            int j;

            for (j = 0; j <= myItem.GetUpperBound(0); j++)
            {
                myRegIetm.DeleteValue(myItem[j], false);
            }
            myItem = myRegIetm.GetValueNames();

            myRegIetm.Flush();
            myRegIetm.Close();

        }
        /// <summary>
        /// 从注册表中读取数据
        /// </summary>
        public static Dictionary<string, string> ReadRegDict()
        {

            RegistryKey myRegIetm = Registry.CurrentUser.CreateSubKey(RegBas, RegistryKeyPermissionCheck.ReadWriteSubTree);

            string[] myItem = null;

            myItem = myRegIetm.GetValueNames();

            if (myItem == null)
            {
                myRegIetm.Close();
                return null;
            }

            Dictionary<string, string> SetDict = new Dictionary<string, string>();

            int j;
            j = myItem.GetUpperBound(0);

            for (j = 0; j <= myItem.GetUpperBound(0); j++)
            {
                string t = null;

                t = myRegIetm.GetValue(myItem[j]).ToString();

                SetDict.Add(myItem[j], t);
            }
            myRegIetm.Close();
            return SetDict;

        }

        /// <summary>
        /// 在注册表中写入字典数据
        /// </summary>
        /// <param name="SetDict"></param>
        public static void WriteReg(Dictionary<string, string> SetDict)
        {

            RegistryKey myRegIetm = Registry.CurrentUser.CreateSubKey(RegBas, RegistryKeyPermissionCheck.ReadWriteSubTree);
            List<string> keyList= new List<string>(SetDict.Keys);

            for (int i = 0; i < SetDict.Count; i++)
            {
               myRegIetm.SetValue(keyList[i], SetDict[keyList[i]]);
            }
           
            myRegIetm.Flush();
            myRegIetm.Close();

        }


    }

}
