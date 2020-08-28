
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using ModelBuilder.SQL;
using ModelBuilder.sys;

namespace ModelBuilder.Forms
{
    public partial class Mian : Form
    {
        public Mian()
        {
            InitializeComponent();
        }
         
        
        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel4.Text = "当前登录用户：" + PublicVar.UserName;

            TreeView treeView1 = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                Name = "treeView1",
                ShowLines = false
            };
            for (int i = 0; i < 10; i++)
            {

                TreeNode treeNode = new TreeNode
                {
                    Text = "测试" + i.ToString(),
                    Name = "测试" + i.ToString()
                };

                TabData tabData = new TabData
                {
                    Text = treeNode.Text
                };

                treeNode.Tag = tabData;
                 
                treeView1.Nodes.Add(treeNode);
                 
            }
            treeView1.NodeMouseClick += My_TreeNodeMouseClick;
             
            xPanderPanel1.Controls.Add(treeView1);
             
        }

       
        //绑定treeview的节点点击事件
        private void My_TreeNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {         
            TreeNode treeNode = e.Node;          
            TabData tabData =(TabData)treeNode.Tag;
            ShowTab(tabData);
        }



        private void ShowTab(TabData tabData)
        {
            if (tabData == null)
            {
                return;
            }
            string TabText = tabData.Text;
             

            var tt = (from  TabPage ss  in tabControl1.TabPages where ss.Text == TabText select ss).ToList();
           
            if (tt.Count == 1)
            {
                tabControl1.SelectedTab = tt[0];

            }
            else
            {
                TabPage curTabPage = new TabPage
                {
                    Name = TabText,
                    Tag = TabText,
                    Text = TabText
                };
                tabControl1.TabPages.Add(curTabPage);
                curTabPage.Controls.Add(new TextBox());
                tabControl1.SelectedTab = curTabPage;

            }



            //if (Application.OpenForms.Item("mainTabFrm") == null)
            //{

            //    //End If
            //    //If IsNothing(thisMainTabFrm) = True Then

            //    thisMainTabFrm = mainTabFrm;

            //    thisMainTabFrm.Show();

            //}
            //else
            //{
            //    thisMainTabFrm.Show();

            //    thisMainTabFrm.Focus();
            //}

            ////向 thisMainTabFrm发送激活命令


            //thisMainTabFrm.WindowState = FormWindowState.Normal;
            //BringWindowToTop(thisMainTabFrm.Handle);
            ////检查到tab

            //thisMainTabFrm.ActiveTag(curTag);

        }


        private void TabControl1_DoubleClick(object sender, EventArgs e)
        {
            //双击选项卡时关闭当前选项卡
            tabControl1.SelectedTab.Parent = null;

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            //注销当前用户，退回到登录窗口
        
            Thread th = new Thread(new ThreadStart(StartLoginForm));
            th.Start();

            this.Close();


        }
        /// <summary>
        /// 重新启动登录界面
        /// </summary>
        [STAThread]
        private static void StartLoginForm()
        {
            Login login = new Login();
            Application.Run(login);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString();
        }
 
    }
}
