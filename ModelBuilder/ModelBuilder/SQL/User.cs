using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.SQL
{
    [Table("Users")]
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserLoginName { get; set; }
        public string UserPassword { get; set; }
        public bool Enable { get; set; }
        public int Level { get; set; }
        public DateTime UserCreateTime { get; set; }
        public DateTime? LastLoginTime{ get; set; }      
        public string remark { get; set; }
    }
    //SQL脚本
    //CREATE TABLE[dbo].[Users]
    //(
    //  [ID][int] IDENTITY(1,1) NOT NULL,
    //  [UserName] [nvarchar](max) NULL,
    //  [UserLoginName] [nvarchar]  (max) NULL,
    //  [UserPassword] [nvarchar] (max) NULL,
    //  [Enable] [bit] NULL,  
    //  [UserCreateTime] [datetime] NULL CONSTRAINT[DF_sysInfo_sysCreateTime]  DEFAULT(getdate()),
    //  [LastLoginTime] [datetime] NULL,
    //  [Level] [int] NULL,
    //  [remark]  [nvarchar] (max) NULL
    //) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
}

