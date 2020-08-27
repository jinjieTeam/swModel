
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
            //pictureBox_1.Image = Properties.Resources.朝上按钮;//设置图像信息
            //pictureBox_2.Image = Properties.Resources.朝上按钮;//设置图像信息
            //pictureBox_3.Image = Properties.Resources.朝上按钮;//设置图像信息
            //Var_Font = label_1.Font;//得到字体对象
 
            TreeView treeView1 = new TreeView();
            //treeView1.Location = new Point(20,30);

            treeView1.Dock = DockStyle.Fill;
            treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            
           
            treeView1.Name = "treeView1";
            treeView1.ShowLines = false;
            for (int i = 0; i < 100; i++)
            {
                treeView1.Nodes.Add("测试" + i.ToString());
            }
           


           xPanderPanel1.Controls.Add(treeView1);
        }
 
       
    }
}
