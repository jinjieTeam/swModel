using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Forms
{
    /// <summary>
    /// 当导航菜单中加载到相关项时，封装一个数据结构到.tag；当用户点击到时，通过.tag在tagcontrol中到要加载的内容
    /// </summary>
    public class TabData
    {


         //使用反射方法可以使用类型名称创建一个实例
        //Type type = Type.GetType("类的完全限定名");
        //dynamic obj = type.Assembly.CreateInstance(type);



        /// <summary>
        /// tabpage显示的标题文本
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// tabpage显示内容需要加载的窗口类名称
        /// </summary>
        public string FormName { set; get; }

        /// <summary>
        /// tabpage显示内容需要加载的数据实体类名称，包括命名空间
        /// </summary>
        public string ClassName { set; get; }

        /// <summary>
        /// tabpage显示内容数据需要加载的数据实体记录ID
        /// </summary>
        public int ID { set; get; }


    }
}
