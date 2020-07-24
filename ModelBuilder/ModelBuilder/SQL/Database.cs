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

namespace ModelBuilder.SQL
{
    public class myEFContext : DbContext
    {

        public myEFContext()
            : base(sys.SysSec.connectionString)
        {
            Database.SetInitializer<myEFContext>(null);//关闭Codefirst对数据库的检测。这样代码只会访问数据，而不会检查数据库结构的更改。避免不需要的警告
            
            //策略一：数据库不存在时重新创建数据库
            //Database.SetInitializer<testContext>(new CreateDatabaseIfNotExists<testContext>());

            //策略二：每次启动应用程序时创建数据库
            //Database.SetInitializer<testContext>(new DropCreateDatabaseAlways<testContext>());

            //策略三：模型更改时重新创建数据库
            //Database.SetInitializer<testContext>(new DropCreateDatabaseIfModelChanges<testContext>());

            //策略四：从不创建数据库
            //Database.SetInitializer<testContext>(null);

            //Database.SetInitializer<myEFContext>(new MigrateDatabaseToLatestVersion<myEFContext, Configuration>());

        }



        public DbSet<sysInfo> sysInfo { get; set; }
    }

}
