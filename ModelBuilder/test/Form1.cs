using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int t = 5;

            //int j = (int t) => { return t * t; };

            //Func<int> func1 = ( ) => t;

            //MessageBox.Show(func1.ToString() );

           

            ////下面带有返回值的
            //Func<int> func0 = () => { return DateTime.Now.Month; };//有一个返回值
            //Func<int> func1() = (n) => {

            //    MessageBox.Show(DateTime.Now.Month.ToString());
            //    return n;
      

            //}; //如果方法体只有一行，去掉大括号分号和return
            //func1.Invoke(10);

        }
    }
}
