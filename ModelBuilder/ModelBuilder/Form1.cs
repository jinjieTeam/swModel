
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ModelBuilder.SQL;

namespace ModelBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            myEFContext db = new myEFContext("HPMD");

            List<sysInfo> resultList = db.sysInfo.ToList();

            sysInfo result = db.sysInfo.Find(2);

            List<sysInfo> resultList2 = db.sysInfo.Where(Item => Item.sysName == "系统名称" && Item.sysCreateTime != null).ToList();

            foreach (sysInfo item in resultList)
            {
                textBox1.AppendText(System.Environment.NewLine + item.sysName);

            }


            //db.Entry(result).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();



            //sysInfo result;

            //result = new sysInfo();
            //result.sysName = "test";
            //result.sysVersion = "1";
            //result.sysCreateTime = DateTime.Now;
            //result.sysDeadTime = DateTime.Now;
            //result.companyName = "rrrr";

            //db.sysInfo.Add(result);
            //db.SaveChanges();
            //MessageBox.Show(result.ID.ToString());
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = sys.SysSec.SetSec(textBox1.Text, checkBox1.Checked);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = sys.SysSec.GetSec(textBox2.Text, checkBox1.Checked);
        }

        private void Form1_Load(object sender, EventArgs e)
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
                    if (sys.SysSec.CheckdbInput(SetDict1["1"], SetDict1["2"], SetDict1["3"], SetDict1["4"]) == false)
                    {
                        b1 = true;
                    }
                }
                catch (Exception)
                {
                    b1 = false;
                }
                //
            }
            if (b1 == true)
            {
                //把数据写入注册表

                sys.SysSec.SaveSetInReg(SetDict1["1"], SetDict1["2"], SetDict1["3"], SetDict1["4"]);
                return;
            }
            //如果数据出错，继续尝试从注册表中读取数据
            Dictionary<string, string> SetDict2 = new Dictionary<string, string>();
            bool b2 = false;
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
                if (sys.SysSec.CheckdbInput(SetDict2["1"], SetDict2["2"], SetDict2["3"], SetDict2["4"]) == false)
                {
                    b2 = true;
                    //把数据写入配置文件
                    sys.SysSec.SaveSetInIni(SetDict2["1"], SetDict2["2"], SetDict2["3"], SetDict2["4"]);
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
    }
}
