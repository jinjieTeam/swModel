using ModelBuilder.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelBuilder.sys
{

    /// <summary>
    /// 处理系统数据库配置数据
    /// </summary>
    public static class SysSec
    {
        public static string connectionString; 

        public static Dictionary<string, string> ReadSetInReg()
        {

            Dictionary<string, string> SetDict = new Dictionary<string, string>();
            var r= RegDeal.ReadRegDict().Where(t=>t.Key=="1"|| t.Key == "2" || t.Key == "3" || t.Key == "4").Select(t => new
                {
                t.Key,
                t.Value
                });
            SetDict = r.ToDictionary(k => k.Key, v => GetSec(v.Value,true));//解密数据

            return SetDict;

        }

        /// <summary>
        /// 在注册表中保存数据库配置信息
        /// </summary>
        /// <param name="dbSvrName"></param>
        /// <param name="dbName"></param>
        /// <param name="dbUserName"></param>
        /// <param name="dbUserPwd"></param>
        /// <returns></returns>
        public static void SaveSetInReg(string dbSvrName, string dbName, string dbUserName, string dbUserPwd)
        {
            Dictionary<string, string> SetDict = new Dictionary<string, string>
            {
                { "1", SetSec(dbSvrName, true) },
                { "2", SetSec(dbName, true) },
                { "3", SetSec(dbUserName, true) },
                { "4", SetSec(dbUserPwd, true) }
            };

            RegDeal.WriteReg(SetDict);
 
        }
        /// <summary>
        /// 在ini文件中保存数据库配置信息
        /// </summary>
        /// <param name="dbSvrName"></param>
        /// <param name="dbName"></param>
        /// <param name="dbUserName"></param>
        /// <param name="dbUserPwd"></param>
        /// <returns></returns>
        public static void SaveSetInIni(string dbSvrName, string dbName, string dbUserName, string dbUserPwd)
        {
            StringBuilder s = new StringBuilder();
            s.Append(SetSec(dbSvrName, true));
            s.AppendLine();
            s.Append(SetSec(dbName, true));
            s.AppendLine();
            s.Append(SetSec(dbUserName, true));
            s.AppendLine();
            s.Append(SetSec(dbUserPwd, true));

            string iniPath = Application.StartupPath;
            if (iniPath.EndsWith(@"\")==false)
            {
                iniPath= iniPath + @"\";
            }
            iniPath = iniPath + "setting.ini";
            File.WriteAllText(iniPath, s.ToString());
             
        }

        /// <summary>
        /// 在配置文件中读取数据库配置信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ReadSetInIni(string iniPath="")
        {

            if (iniPath=="")
            {
                iniPath = Application.StartupPath;
                if (iniPath.EndsWith(@"\") == false)
                {
                    iniPath = iniPath + @"\";
                }
                iniPath = iniPath + "setting.ini";
            }
            if (File.Exists(iniPath)==false)
            {
                return null;
            }
            string[] t= File.ReadAllLines(iniPath);
            Dictionary<string, string> SetDict = new Dictionary<string, string>();

            for (int i = 0; i < t.Length; i++)
            {
                SetDict.Add((i + 1).ToString(), GetSec(t[i], true));
            }
 
            return SetDict;

        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SetSec(string str, bool revB)
        {

            //加密算法：取逆序字符串，分别把每一个字符转换为16进制字节码，如果revB=true，字节码的每一个数值取为256和原值的差

            str = string.Join("", str.ToArray().Reverse());

            string textAscii = string.Empty;//用来存储转换过后的ASCII码

            textAscii = StringToHexString(str, encode, revB);

            return textAscii;
        }

        /// <summary>
        /// 把数据库配置加密转换为字符串
        /// </summary>
        /// <param name="textAscii"></param>
        /// <returns></returns>
        public static string GetSec(string textAscii, bool revB)
        {
            //得到加密字符串初始字符串
 
            string textStr = string.Empty;

            textStr = HexStringToString(textAscii, encode, revB);

            textStr = string.Join("", textStr.ToArray().Reverse());

            return textStr;
        }

        private static readonly Encoding encode = System.Text.Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 把一个按字节值顺序存储的字符串按指定编码转换为字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string HexStringToString(string hs, Encoding encode, bool revB)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);//每一个字节占2位字符

                if (revB) //需要用256减去当前值得到原值
                {
                    int t = Convert.ToInt16(strTemp, 16); //字符串转换为10进制数          
                    t = 256 - t;//得到原值
                    strTemp = Convert.ToString(t, 16);//再次转换回16进制字符串
                }

                b[i] = Convert.ToByte(strTemp, 16);//还原为字节
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        /// <summary>
        /// 把一个字符串每一个字符转换为字节值，然后按指定编码连接为字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <param name="revB">true：每一个值用256减</param>
        /// <returns></returns>
        private static string StringToHexString(string s, Encoding encode, bool revB)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                if (revB)
                {
                    string h = b[i].ToString("X2"); //转换为16进制字符串
                    int t = Convert.ToInt16(h, 16);//转换为整型
                    t = 256 - t;//得到差值

                    if (t < 10) //如果小于0，转换之后只有1为字符串，需要补0
                    {
                        string ss = Convert.ToString(t, 16);//再转换为16进制字符串
                        ss = "0" + ss;
                        result += ss;
                    }
                    else
                    {
                        result += Convert.ToString(t, 16);
                    }

                }
                else
                {
                    result += Convert.ToString(b[i], 16);
                }
            }
            return result;
        }
        /// <summary>
        /// 检查数据库输入
        /// </summary>
        /// <returns>只检查输入，不验证连接</returns>
        public static bool CheckdbInput(string dbSvrName, string dbName, string dbUserName, string dbUserPwd)
        {
            //return false;
            Dictionary<string, string> SetDict = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(dbSvrName) == false)
            {
                SetDict.Add("dbSvrName", dbSvrName);

            }
            else
            {
                MessageBox.Show("请输入可正确访问的数据库服务器名称或者IP地址");
                return false;
            }
            if (string.IsNullOrWhiteSpace(dbName) == false)
            {
                SetDict.Add("dbName", dbName);

            }
            else
            {
                MessageBox.Show("请输入可正确访问的数据库名称");
                return false;
            }
            if (string.IsNullOrWhiteSpace(dbUserName) == false)
            {
                SetDict.Add("dbUserName", dbUserName);

            }
            else
            {
                MessageBox.Show("请输入可正确访问的数据库用户名，要求至少owner权限级别");
                return false;
            }
            if (string.IsNullOrWhiteSpace(dbUserPwd) == false)
            {
                SetDict.Add("dbUserPwd", dbUserPwd);

            }
            else
            {
                MessageBox.Show("请输入可正确访问的数据库用户密码");
                return false;
            }
           connectionString = "Data Source=" + dbSvrName + 
                "; Initial Catalog=" + dbName + "; Persist Security Info=False;User ID=" +
                dbUserName +"; Password=" + dbUserPwd + "; Connect Timeout=3";

            return true;
        }
        /// <summary>
        /// 检查数据库连接
        /// </summary>
        /// <returns>成功连接返回true</returns>
        public static bool CheckdbCon(string dbSvrName, string dbName, string dbUserName, string dbUserPwd)
        {
            if (CheckdbInput(dbSvrName, dbName, dbUserName, dbUserPwd)==false)
            {
                return false;
            }
            try
            {
                myEFContext db = new myEFContext();
               int n= db.Database.ExecuteSqlCommand("SELECT 1");
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
