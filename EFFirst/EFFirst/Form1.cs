using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFFirst
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

            //myEFContext db = new myEFContext();

            //sysInfo result;

            //result = db.sysInfo.Find(2);
            //MessageBox.Show(result.sysName);

            List<sysInfo> resultList = db.sysInfo.ToList();

            foreach (sysInfo item in resultList)
            {
                //MessageBox.Show(item.sysName);
                textBox1.AppendText(System.Environment.NewLine + item.sysName);

            }

            sysInfo result;

            result = new sysInfo();
            result.sysName = "test";
            result.sysVersion = "1";
            result.sysCreateTime = DateTime.Now;
            result.sysDeadTime = DateTime.Now;
            result.companyName = "rrrr";

            db.sysInfo.Add(result);
            db.SaveChanges();
            MessageBox.Show(result.ID.ToString());

        }
    }
    [Table("sysInfo")]
    public class sysInfo
    {
        [Key]
        public int ID { get; set; }
        public string companyName { get; set; }
        public string sysName { get; set; }
        public string sysVersion { get; set; }
        public string sysKey { get; set; }
        public string sysLicence { get; set; }
        public DateTime  sysCreateTime { get; set; }
        public DateTime?  sysDeadTime { get; set; }
        public int?  MaxUser { get; set; }
        public string sharePath { get; set; }
        public string remark { get; set; }


    }

    public class myEFContext : DbContext
    {
        static myEFContext()
        {
            Database.SetInitializer<myEFContext>(null);//关闭Codefirst对数据库的检测

            //策略一：数据库不存在时重新创建数据库
            //Database.SetInitializer<testContext>(new CreateDatabaseIfNotExists<testContext>());

            //策略二：每次启动应用程序时创建数据库
            //Database.SetInitializer<testContext>(new DropCreateDatabaseAlways<testContext>());

            //策略三：模型更改时重新创建数据库
            //Database.SetInitializer<testContext>(new DropCreateDatabaseIfModelChanges<testContext>());

            //策略四：从不创建数据库
            //Database.SetInitializer<testContext>(null);
        }
        public myEFContext(string connectionName)
            : base(connectionName) { }
  

        public DbSet<sysInfo> sysInfo { get; set; }
        
    }

 

}
