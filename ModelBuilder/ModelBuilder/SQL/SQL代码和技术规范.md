# SQL数据库代码及其技术规范
-----------------------------------
 
为了让初学者或者没有较多数据库编程经验的朋友也能加入，编写本技术规范。
基础信息： 
> * 数据库：Microsoft SQL Server 2014
> * 访问机制：Entity Framework（简称EF）
> * 命名空间：ModelBuilder.SQL

[可点此链接完整学习EF技术](https://www.cnblogs.com/caofangsheng/p/5020541.html)



### 1. 什么是Frist Code
Frist Code是Entity Framework 框架4.1版本之后提供的一种数据库访问方法。该方法允许我们完全使用代码来定义数据库、表，然后自动化来生成数据库和表。

#### 2. 安装Frist Code
在解决方案文件夹右键，点击“管理解决方案的NuGet程序包”，在新窗口中浏览搜索到“EntityFramework”，选择搜索结果的第一个安装即可。

#### 3. 定义数据库连接字符串
数据库连接字符串在项目的App.config文件中定义。
App.config是一个标准的xml文件，其根节点为`<configuration>`。
数据库连接字符串使用节点`<connectionStrings>`，该节点为根节点`<configuration>`的字节点。
数据库连接字符串代码如下：
```xml
<connectionStrings>
    <add name="HPMD" connectionString="Data Source=SZEPDM;Initial Catalog=HPMD;Persist Security Info=False;User ID=mbAdmin;Password=mbAdmin;" providerName="System.Data.SqlClient" />
</connectionStrings>
```
上述代码中：
name后面的字符串定义了一个数据库连接名称，后面我们都使用这个名称来指代到后面真正的连接字符串。
connectionString字符串中，按实际情况更改其中的值。

|键                |值          |含义               |说明|
|--------          |--------    |:-----             |:----|
| Data Source      |SZEPDM      |数据库服务器名称   |一般是安装数据库的计算机名称，也可以使用IP
| Initial Catalog  |HPMD        |数据库名称         |数据库名称按实际情况自行取定
| Persist Security Info |False  |是否保存安全信息   |即ADO在数据库连接成功后是否保存密码信息|
| User ID          |mbAdmin     |登录数据库的用户名 |在数据库管理工具中设定（数据库必须是混合登录模式），至少具有owner级别的权限
| Password         |mbAdmin     |登录数据库的用户密码|

#### 4. 定义数据库访问对象
所有数据库的访问对象均在ModelBuilder.SQL命名空间中，即在文件夹ModelBuilder/SQL中。

其中Database.cs文件定义了数据库访问上下文对象 myEFContext，通过以下代码实现:
```csharp
namespace ModelBuilder.SQL
{
    public  class myEFContext : DbContext
    {
        public myEFContext()
        {
            Database.SetInitializer<myEFContext>(null);//关闭Codefirst对数据库的检测。这样代码只会访问数据，而不会检查数据库结构的更改。避免不需要的警告
        }
        public myEFContext(string connectionName)
            : base(connectionName) { }
        public DbSet<sysInfo> sysInfo { get; set; }
    }
}

```
上述代码中，myEFContext类的myEFContext方法需要一个字符串作为参数，该字符串参数将会到App.config文件自动通过节点`<connectionStrings>`获取到对应的数据库连接字符串，然后创建一个数据库数据上下文对象。

#### 5. 定义表对象
一个表对象均需要在SQL文件夹中创建一个以表名命名的.cs类文件。
该类文件基本结构示例如下：
```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelBuilder.SQL
{
   
    [Table("sysInfo")]
    public class sysInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }//主键自增ID
        public string companyName { get; set; }//公司名称
        public DateTime sysCreateTime { get; set; }//系统创建时间
        public DateTime? sysDeadTime { get; set; }//系统终止时间
        public int? MaxUser { get; set; }//最大使用用户数
        public string remark { get; set; }//备注
    }
}

```
说明：
> * 请勿修改引用头和命名空间。
> * 一个类文件中只存放一个类。
> * Table("")括号和下面的类名均和数据库表名保持一致。
> * 每一个数据表列用一个相同名称的类属性对应定义和映射。
> * 请保持列名和属性模型类型一致；如有存在空值的时间、整型数据，请使用带?的类型定义。
> * 建议不要使用限定长度的列定义；如果要定义长度，请也在列修饰词中定义。
> * 类定义中，必须包含ID项，而且按示例要求给定修饰词。
> * 类定义中，对于记录类型的表，建议包含一个创建时间CreateTime和更新时间UpdateTime的项。
> * 尽量避免在类中进行逻辑处理，除非具有内在的逻辑联系。
> * 在每一列可注释其表示的含义。
> * 不在数据表中映射的属性，使用`[NotMapped]`修饰词说明。
> * 更多的修饰词请

**【请注意】任一表还必须在Database.cs文件中的类myEFContext中类似于使用`public DbSet<sysInfo> sysInfo { get; set; }`代码说明是其中的表**

### 6. 使用EF
每一个从表中查询的记录和相关类对应。
下面的代码示例了查询、删除、更新和插入数据的基本方法：

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
            myEFContext db = new myEFContext("HPMD");//通过定义数据库连接字符串创建数据库连接对象
            
            List<sysInfo> resultList1 = db.sysInfo.ToList();//得到表sysInfo所有记录列表
            List<sysInfo> resultList2 = db.sysInfo.Finde();//得到表sysInfo所有记录列表
            List<sysInfo> resultList3 = db.sysInfo.Where(Item => Item.sysName == "系统名称" && Item.sysCreateTime != null).ToList();//使用Lambda表达式作为条件查询
            
            //以下代码更改查询到的result
            result.sysName = "test";
            db.Entry(result).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
          
           //以下代码删除查询到的result
            db.sysInfo.Remove(result);
            db.SaveChanges();

           //以下代码新增一个记录

            result = new sysInfo();
            result.sysName = "test";
            result.sysVersion = "1";
            result.sysCreateTime = DateTime.Now;
            db.sysInfo.Add(result);
            db.SaveChanges();                 
        }
    }
}

```
查询数据时，可使用各种合法Lambda表达式进行数据筛选，可使用列表（泛型）对象的方法进行灵活的数据处理。
### 7. 相关约定
由于是集体开发项目，我们约定：
- [x]本项目中仅使用EF的方法来访问数据库，实现业务的增删改查。
- [x]数据库使用Microsoft SQL Server Management Studio设计和维护。
- [x]应在每一个数据表类中用注释的方法给出创建表的SQL脚本。
- [x]所有业务逻辑均在代码层面实现，不使用数据库的触发、约束、函数等复杂技术。
- [x]必要时可以使用事务，其方法封装在db.Database中。
- [x]数据库使用Microsoft SQL Server Management Studio设计和访问。
- [x]不使用Fluent API技术定义数据库和表。
