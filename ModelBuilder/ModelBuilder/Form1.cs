
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
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

        private void button1_Click(object sender, EventArgs e)
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
    }
}
