using ModelBuilder.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelBuilder.sys
{
    public partial class Login : Form
    {
        [DllImport("gdi32", EntryPoint = "CreateRoundRectRgn", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern Int32 CreateRoundRectRgn(Int32 X1, Int32 Y1, Int32 X2, Int32 Y2, Int32 X3, Int32 Y3);
        [DllImport("user32", EntryPoint = "SetWindowRgn", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern Int32 SetWindowRgn(Int32 hWnd, Int32 hRgn, bool bRedraw);
        //GDI重绘API

        public Login()
        {
            InitializeComponent();
           

        }

        private void Label6_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Label2_Paint(object sender, PaintEventArgs e)
        {
            //左边上边右边底边
            ControlPaint.DrawBorder(e.Graphics, Label2.ClientRectangle, Color.White, 0, ButtonBorderStyle.None, Color.White, 0, ButtonBorderStyle.None, Color.DimGray, 0,
            ButtonBorderStyle.Solid, Color.DimGray, 1, ButtonBorderStyle.Solid);
        }


        private void LoadData()
        {
            CheckSet();
            LoadSysData();

            try
            {
               
                Label2.Text = "登录到" + SysSec.sysName;
                Label3.Text = SysSec.companyName + "版权所有" + Environment.NewLine + "COPYRIGHT@(2020 - 2023)";


                int r1 = CreateRoundRectRgn(0, 0, Panel3.Width, Panel3.Height, 10, 10);
                SetWindowRgn(Panel3.Handle.ToInt32(), r1, true);

                int r2 = CreateRoundRectRgn(0, 0, Panel4.Width, Panel4.Height, 10, 10);
                SetWindowRgn(Panel4.Handle.ToInt32(), r2, true);
                //SetWindowRgn(TextBox2.Handle, r, True)

                //在注册表中获取是否记住用户名

                string yhmt = null;
                yhmt = PublicVar.GetUserName();

                if (!string.IsNullOrEmpty(yhmt))
                {
                    CheckBox1.Checked = true;
                    TextBox1.Text = yhmt;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }

        }

        public void LoadSysData()
        {
            //如果没有admin用户，添加该用户

            myEFContext db = new myEFContext();

            //检查系统设置

            int n = db.sysInfo.Count();

            if (n == 1)
            {
                sysInfo sysInfo = db.sysInfo.First();

                SysSec.companyName = sysInfo.companyName;
                SysSec.sysName = sysInfo.sysName;
            }

            n = db.Users.Where (t => t.UserLoginName.ToLower() == "admin").Count();
            if (n==0)
            {
                User adminUser = new User
                {
                    UserLoginName = "admin",
                    UserPassword = SysSec.StringSec("admin"),
                    UserName = "系统管理员",
                    Level = 0,
                    Enable = true,
                    remark = "系统内置管理员，不能删除",
                    UserCreateTime = DateTime.Now,
                    LastLoginTime = null
                  
                };

                db.Users.Add(adminUser);
                db.SaveChanges();

                //自动打开系统配置界面
                MessageBox.Show("第一次登录使用系统，请设置系统数据");
                Form dbset = new sys.SysSet();

                dbset.ShowDialog();


            }
          
        }

        public void CheckSet()
        {
            //获取配置文件数据

            string iniPath = Application.StartupPath;
            if (iniPath.EndsWith(@"\") == false)
            {
                iniPath = iniPath + @"\";
            }
            iniPath = iniPath + "setting.ini";
            Dictionary<string, string> SetDict1 = new Dictionary<string, string>();

            bool b1 = false;
            if (File.Exists(iniPath) == true)
            {
                SetDict1 = sys.SysSec.ReadSetInIni();
                if (SetDict1 == null)
                {
                    b1 = false;
                }
                try
                {
                    //测试数据库连接
                    if (sys.SysSec.CheckdbCon(SetDict1["1"], SetDict1["2"], SetDict1["3"], SetDict1["4"]) == true)
                    {
                        b1 = true;
                    }
                }
                catch (Exception)
                {
                    b1 = false;
                }
                 
            }
            if (b1 == true)
            {
                //把数据写入注册表

                sys.SysSec.SaveSetInReg(SetDict1["1"], SetDict1["2"], SetDict1["3"], SetDict1["4"]);
                return;
            }
            //如果数据出错，继续尝试从注册表中读取数据
            Dictionary<string, string> SetDict2 = new Dictionary<string, string>();
            bool b2;
            b2 = false;

            SetDict2 = sys.SysSec.ReadSetInReg();

            if (SetDict2 == null)
            {
                b2 = false;
                MessageBox.Show("数据库配置错误，转入配置界面");
                Form dbset = new sys.dbSet();

                dbset.ShowDialog();
            }
            try
            {
                //测试数据库连接
                if (sys.SysSec.CheckdbCon(SetDict2["1"], SetDict2["2"], SetDict2["3"], SetDict2["4"]) == true)
                {
                    b2 = true;
                    //把数据写入配置文件
                    sys.SysSec.SaveSetInIni(SetDict2["1"], SetDict2["2"], SetDict2["3"], SetDict2["4"]);
                }
                else
                {
                    b1 = false;
                    b2 = false;
                    MessageBox.Show("数据库配置错误，转入配置界面");
                    Form dbset = new sys.dbSet();

                    dbset.ShowDialog();
                }
            }
            catch (Exception)
            {
                b1 = false;
                b2 = false;
                MessageBox.Show("数据库配置错误，转入配置界面");
                Form dbset = new sys.dbSet();

                dbset.ShowDialog();
            }


        }

        private void Button1_MouseHover(object sender, System.EventArgs e)
        {
            Button1.BackColor = Color.FromArgb(62, 95, 154);
        }

        private void Button1_MouseLeave(object sender, System.EventArgs e)
        {
            Button1.BackColor = Color.DarkOliveGreen;
        }

        private void Label6_MouseHover(object sender, System.EventArgs e)
        {
            Label6.ForeColor = Color.White;
        }

        private void Label6_MouseLeave(object sender, System.EventArgs e)
        {
            Label6.ForeColor = Color.Black;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //检查登录
            string yhm =  TextBox1.Text;
            string mima = TextBox4.Text;

            if (string.IsNullOrEmpty(yhm) | string.IsNullOrEmpty(mima))
            {
                MessageBox.Show(this,"用户名或者密码为空，请检查。系统不允许用户使用空密码。", 
                    "用户名或密码不能为空",MessageBoxButtons.OK);
                return;
            }

            //检查用户名
            myEFContext db = new myEFContext();
 
            User user = db.Users.FirstOrDefault(t => t.UserLoginName.ToUpper() == yhm.ToUpper());

            if (user==null)
            {
                MessageBox.Show(this, "未找到用户名，请检查。用户名一般为姓名全拼，请联系管理员添加。",
                 "用户名不存在", MessageBoxButtons.OK);
                return;
            }
            //检查用户密码
            if (user.UserPassword!=sys.SysSec.StringSec(mima))
            {
                MessageBox.Show(this, "用户密码不配对。",
                "密码错误", MessageBoxButtons.OK);
                return;
            }

            if (user.Enable ==false )
            {
                MessageBox.Show(this, "用户已经被禁止登录，请联系管理员", "警告",MessageBoxButtons.OK);
                return;
            }


            //更新登录信息

            user.LastLoginTime = DateTime.Now;
            db.SaveChanges();

            PublicVar.UserLoginName = user.UserLoginName;
            PublicVar.UserName = user.UserName;

            //记住用户
            if (CheckBox1.Checked == true)
            {
                PublicVar.SetUserName(yhm);
            }
            else
            {
                PublicVar.SetUserName("");
            }
            this.Visible = false;

            if (user.Level == 0)
            {

                Form SysSet = new sys.SysSet();
                SysSet.ShowDialog();
                this.Close();

            }

            Form mian = new mian();
            mian.ShowDialog();
            this.Close();


        }
    }
}
