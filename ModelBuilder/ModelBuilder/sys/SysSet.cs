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

        private void SysSet_Load(object sender, EventArgs e)
        {
            //加载数据
            TextBox9.Text = Application.ProductVersion;
            if (sys.SysSec.CheckdbCon(sys.SysSec.dbSvrName, sys.SysSec.dbName, sys.SysSec.dbUserName, sys.SysSec.dbUserPwd) == true)
            {
                SysLoad();
                UsersLoad();

            }
        }

        private void SysLoad()
        {

            textBox11.Text = sys.SysSec.dbSvrName;
            textBox4.Text = sys.SysSec.dbName;
            textBox6.Text = sys.SysSec.dbUserName;
            textBox5.Text = sys.SysSec.dbUserPwd;

            myEFContext db = new myEFContext();

            sysInfo sysInfo = db.sysInfo.First();

            if (sysInfo != null)
            {
                TextBox8.Text = sysInfo.companyName;
                TextBox10.Text = sysInfo.sysName;
            }
        }
        private void UsersLoad()
        {

            myEFContext db = new myEFContext();

            List<User> users = db.Users.ToList();

            if (users != null)
            {

                foreach (var item in users)
                {
                    ListBox1.Items.Add(item.UserName + "(" + item.UserLoginName + ")");

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

            if (n == 0)
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
            else if (n == 1)
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

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = ListBox1.SelectedItem.ToString();

            string[] a = curItem.Split('(');

            string loginUserName = a[a.GetUpperBound(0)].Replace(")", "");

            myEFContext db = new myEFContext();

            List<User> users = db.Users.Where(t => t.UserLoginName == loginUserName).ToList();

            if (users.Count == 1)
            {
                User user = users[0];
                TextBox7.Text = user.UserLoginName;
                TextBox7.Tag = user.ID;
                TextBox3.Text = user.UserName;
                TextBox2.Text = user.LastLoginTime.ToString();
                ComboBox5.Text = user.Level.ToString();
                checkBox1.Checked = user.Enable;
                TextBox1.Text = user.remark;


            }

        }

        private void Button3_Click(object sender, EventArgs e)
        {

            if (TextBox7.Text.ToLower() == "admin")
            {
                MessageBox.Show(this, "不能删除内置管理员admin。",
                 "操作禁止", MessageBoxButtons.OK);
                return;
            }
            //删除
            myEFContext db = new myEFContext();

            User user = db.Users.Find(TextBox7.Tag);

            if (user != null)
            {
                if (MessageBox.Show(this, "确认删除用户吗？如果仅仅是禁止用户登录，可以考虑不勾选【允许登录】",
                  "用户删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show(this, "未能找到指定用户名的用户。",
                  "用户错误", MessageBoxButtons.OK);
                return;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (TextBox7.Text.ToLower() == "admin" && PublicVar.UserLoginName.ToLower() != "admin")
            {
                MessageBox.Show(this, "您不能更改内置管理员admin密码。",
                 "操作禁止", MessageBoxButtons.OK);
                return;
            }
            myEFContext db = new myEFContext();

            User user = db.Users.Find(TextBox7.Tag);

            if (user != null)
            {
                user.UserPassword = sys.SysSec.StringSec(user.UserLoginName);

                db.SaveChanges();
                MessageBox.Show(this, "操作成功。", "操作提示", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show(this, "未能找到指定用户名的用户。",
                  "用户错误", MessageBoxButtons.OK);
                return;
            }


        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if (TextBox7.Tag == null)
            {
                MessageBox.Show(this, "请先选择到一个指定用户", "操作终止", MessageBoxButtons.OK);
                return;
            }
            if (TextBox7.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户名", "操作终止", MessageBoxButtons.OK);
                return;
            }
            if (TextBox3.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户姓名", "操作终止", MessageBoxButtons.OK);
                return;
            }
            if (ComboBox5.Text.Trim() == "")
            {
                MessageBox.Show(this, "请选择用户权限级别", "操作终止", MessageBoxButtons.OK);
                return;
            }

            myEFContext db = new myEFContext();
            //检查用户名是否重复

            int count = db.Users.Where(t => t.ID != int.Parse(TextBox7.Tag.ToString()) &&
              t.UserLoginName.ToUpper() == TextBox7.Text.ToUpper()).Count();

            if (count != 0)
            {
                MessageBox.Show(this, "用户名重复。", "操作终止", MessageBoxButtons.OK);
                return;
            }
            User user = db.Users.Find(TextBox7.Tag);

            if (user != null)
            {
                user.UserLoginName = TextBox7.Text.Trim();
                user.UserName = TextBox3.Text.Trim();
                user.UserCreateTime = DateTime.Now;
                user.Level = int.Parse(ComboBox5.Text);
                user.remark = TextBox1.Text.Trim();
                user.Enable = checkBox1.Checked;
                //db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            else
            {
                MessageBox.Show(this, "未能找到指定用户名的用户。",
                  "用户错误", MessageBoxButtons.OK);
                return;
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {

            if (TextBox7.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户名", "操作终止", MessageBoxButtons.OK);
                return;
            }
            if (TextBox3.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户姓名", "操作终止", MessageBoxButtons.OK);
                return;
            }
            if (ComboBox5.Text.Trim() == "")
            {
                MessageBox.Show(this, "请选择用户权限级别", "操作终止", MessageBoxButtons.OK);
                return;
            }

            myEFContext db = new myEFContext();
            //检查用户名是否重复

            int count = db.Users.Where(t => t.UserLoginName.ToUpper() == TextBox7.Text.ToUpper()).Count();

            if (count != 0)
            {
                MessageBox.Show(this, "用户名重复。", "操作终止", MessageBoxButtons.OK);
                return;
            }
            User user = new User
            {
                UserLoginName = TextBox7.Text.Trim(),
                UserName = TextBox3.Text.Trim(),
                UserCreateTime = DateTime.Now,
                UserPassword = sys.SysSec.StringSec(TextBox7.Text.Trim()),
                Level = int.Parse(ComboBox5.Text),
                remark = TextBox1.Text.Trim(),
                Enable = checkBox1.Checked
            };
            db.Users.Add(user);
            db.SaveChanges();

        }
    }
}
