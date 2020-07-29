using ModelBuilder.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelBuilder.sys
{
    public partial class SysSet : Form
    {
        public SysSet()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string dbSvrName = textBox11.Text;
            string dbName = textBox4.Text;
            string dbUserName = textBox6.Text;
            string dbUserPwd = textBox5.Text;

            //测试数据库连接
            if (sys.SysSec.CheckdbCon(dbSvrName, dbName, dbUserName, dbUserPwd) == false)
            {
                return;
            }
 

        }

      

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FileName = "setting.ini";
            fileDialog.Filter = "配置文件|*.ini";
            fileDialog.Title = "请选择配置文件setting.ini，默认存储在程序当前目录";
            if (fileDialog.ShowDialog()==DialogResult.OK)
            {
                string iniPath = fileDialog.FileName;
                Dictionary<string, string> SetDict = new Dictionary<string, string>();
                SetDict = SysSec.ReadSetInIni(iniPath);
                if (SetDict==null)
                {
                    MessageBox.Show("数据错误");
                    return;
                }
                try
                {
                      textBox11.Text = SetDict["1"];
                      textBox4.Text = SetDict["2"];
                      textBox6.Text = SetDict["3"];
                      textBox5.Text = SetDict["4"];

                    //测试数据库连接
                    if (sys.SysSec.CheckdbCon(textBox11.Text, textBox4.Text, textBox6.Text, textBox5.Text) == false)
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("数据错误");
                    return;
                }

            }
          
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            //保存数据
            string dbSvrName = textBox11.Text;
            string dbName = textBox4.Text;
            string dbUserName = textBox6.Text;
            string dbUserPwd = textBox5.Text;

            //测试数据库连接
            if (sys.SysSec.CheckdbCon(dbSvrName, dbName, dbUserName, dbUserPwd) == false)
            {
                return;
            }

            SysSec.SaveSetInReg(dbSvrName, dbName, dbUserName, dbUserPwd);
            SysSec.SaveSetInIni(dbSvrName, dbName, dbUserName, dbUserPwd);



        }

        private void Button10_Click(object sender, EventArgs e)
        {
            //保存数据
            string dbSvrName = textBox11.Text;
            string dbName = textBox4.Text;
            string dbUserName = textBox6.Text;
            string dbUserPwd = textBox5.Text;

            //测试数据库连接
            if (sys.SysSec.CheckdbCon(dbSvrName, dbName, dbUserName, dbUserPwd) == false)
            {
                return;
            }
      
            SysSec.SaveSetInIni(dbSvrName, dbName, dbUserName, dbUserPwd);
            MessageBox.Show("数据已经保存在当前目录的setting.ini文件中，你可以复制该配置文件到其他计算机中应用");

        }

        private void SysSet_Load(object sender, EventArgs e)
        {
            //加载数据
            TextBox9.Text = Application.ProductVersion;
            if (sys.SysSec.CheckdbCon(sys.SysSec.dbSvrName, sys.SysSec.dbName, sys.SysSec.dbUserName, sys.SysSec.dbUserPwd) ==true)
            {
                textBox11.Text= sys.SysSec.dbSvrName;
                textBox4.Text= sys.SysSec.dbName;
              textBox6.Text= sys.SysSec.dbUserName;
                 textBox5.Text= sys.SysSec.dbUserPwd;

                myEFContext db = new myEFContext();

                sysInfo sysInfo = db.sysInfo.First();

                if (sysInfo != null)
                {
                    TextBox8.Text = sysInfo.companyName;
                    TextBox10.Text = sysInfo.sysName;
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //保存信息

            if (sys.SysSec.CheckdbCon(sys.SysSec.dbSvrName, sys.SysSec.dbName, sys.SysSec.dbUserName, sys.SysSec.dbUserPwd) == false)
            {
                MessageBox.Show("数据库连接失败，请先设置数据库配置");
                return;
            }
            myEFContext db = new myEFContext();
         
            int n = db.sysInfo.Count();

            if (n==0)
            {
                //新增 
                sysInfo sysInfo = new sysInfo
                {
                    companyName = TextBox8.Text,
                    sysName = TextBox10.Text,
                    sysVersion = Application.ProductVersion,
                    sysCreateTime = DateTime.Now,
                    MaxUser = 100,
                    sysDeadTime = DateTime.Now.AddYears(10),
                    sysKey = "",
                    sharePath = ""
                };
                db.sysInfo.Add(sysInfo);
                db.SaveChanges();

                SysSec.companyName = TextBox8.Text;
                SysSec.sysName = TextBox10.Text;
              
            }
            else if (n==1)
            {
                sysInfo sysInfo = db.sysInfo.First();
                sysInfo.companyName = TextBox8.Text;
                sysInfo.sysName = TextBox10.Text;
                sysInfo.sysVersion = Application.ProductVersion;
                 
                db.SaveChanges();

                SysSec.companyName = TextBox8.Text;
                SysSec.sysName = TextBox10.Text;
            }
            

           
        }

        private void SysSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            //检查数据库连接

            if (sys.SysSec.CheckdbCon(sys.SysSec.dbSvrName, sys.SysSec.dbName, sys.SysSec.dbUserName, sys.SysSec.dbUserPwd) == false)
            {
                MessageBox.Show("数据库连接失败，确认退出吗？");
                Environment.Exit(0);

            }

            //检查系统设置
            myEFContext db = new myEFContext();

            int n = db.sysInfo.Count();

            if (n == 1)
            {
                sysInfo sysInfo = db.sysInfo.First();
                
                SysSec.companyName = sysInfo.companyName;
                SysSec.sysName = sysInfo.sysName;
            }
           
        }
    }
}
