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
    public partial class dbSet : Form
    {
        public dbSet()
        {
            InitializeComponent();
        }

        private void Button6_Click(object sender, EventArgs e)
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

            //myEFContext
            //bool b = DbDeal.CheckConnect();

            //if (b == true)
            //{
            //    MessageBox.Show("数据库连接成功");
            //}


        }



        private void Button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FileName = "setting.ini";
            fileDialog.Filter = "配置文件|*.ini";
            fileDialog.Title = "请选择配置文件setting.ini，默认存储在程序当前目录";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string iniPath = fileDialog.FileName;
                Dictionary<string, string> SetDict = new Dictionary<string, string>();
                SetDict = SysSec.ReadSetInIni(iniPath);
                if (SetDict == null)
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
    }
}
