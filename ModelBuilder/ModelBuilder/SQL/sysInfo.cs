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
        public int ID { get; set; }
        public string companyName { get; set; }
        public string sysName { get; set; }
        public string sysVersion { get; set; }
        public string sysKey { get; set; }
        public string sysLicence { get; set; }
        public DateTime sysCreateTime { get; set; }
        public DateTime? sysDeadTime { get; set; }
        public int? MaxUser { get; set; }
        public string sharePath { get; set; }
        public string remark { get; set; }
    }
    //SQL脚本
    //CREATE TABLE[dbo].[sysInfo]
    //(
    //  [ID][int] IDENTITY(1,1) NOT NULL, 
    //  [companyName] [nvarchar](max) NULL,
    //  [sysName] [nvarchar](max) NULL, 
    //  [sysVersion] [nvarchar](max) NULL,  
    //  [sysKey] [nvarchar](max) NULL,
    //  [sysLicence] [nvarchar](max) NULL,  
    //  [sysCreateTime] [datetime] NULL CONSTRAINT[DF_sysInfo_sysCreateTime]  DEFAULT(getdate()),
    //  [sysDeadTime] [datetime] NULL,
    //  [MaxUser] [int] NULL,
    //  [sharePath] [nvarchar](max) NULL,
    //  [remark] [nvarchar](max) NULL
    //) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]


}

