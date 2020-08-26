
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
    public partial class mian : Form
    {
        public mian()
        {
            InitializeComponent();
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
                    if (sys.SysSec.CheckdbCon(SetDict1["1"], SetDict1["2"], SetDict1["3"], SetDict1["4"]) == true)
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
                if (sys.SysSec.CheckdbCon(SetDict2["1"], SetDict2["2"], SetDict2["3"], SetDict2["4"]) ==true)
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
    }
}
