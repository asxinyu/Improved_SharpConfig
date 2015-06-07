using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpConfig;

namespace FormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //按文件名称加载配置文件
            Configuration config = Configuration.LoadFromFile("example.txt");
            //按照节的名称读取节
            Section section = config["General"];
            //依次根据每个配置项的名称来读取，如果配置文件类型搞错了，会报错
            string someString = section["SomeString"].Value;
            int someInteger = section["SomeInteger"].GetValue<int>();
            float someFloat = section["SomeFloat"].GetValue<float>();
            Boolean someBool = section["ABoolean"].GetValue<Boolean>();
            object[] int32Arr = section["MyArray"].GetValue<object[]>();
            richTextBox1.Text = someString;
        }
    }
}
