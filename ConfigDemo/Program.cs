using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigDemo
{
    class Program
    {
        /// <summary>SharpConfig组件的使用Demo</summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Test1();
            //Console.WriteLine("完成");
            Console.ReadKey();
        }

        static void Test1()
        {
            //按文件名称加载配置文件
            Configuration config = Configuration.LoadFromFile("example.ini",
                                    Encoding.GetEncoding("gb2312"));
            Section section = config["General"];
            string someString = section["SomeString"].Value;
            Console.WriteLine("字符串SomeString值:{0}", someString);
            return;

            //修改值
            config["General"]["Width"].Value = "1920";
            /*
            //按照节的名称读取节
            Section section = config["General"];
            string someString = section["SomeString"].Value;
            //依次根据每个配置项的名称来读取，如果配置文件类型搞错了，会报错
            string someString = section["SomeString"].Value;
            //如果不为空，注意！符号
            //if (!string.IsNullOrEmpty(someString)) { } //给textbox赋值

            //读取SomeInteger的值
            int someInteger = section["SomeInteger"].GetValue<int>();
            //if(someInteger !=0) //赋值
            float someFloat = section["SomeFloat"].GetValue<float>();
            Boolean someBool = section["ABoolean"].GetValue<Boolean>();
            object[] int32Arr = section["MyArray"].GetValue<object[]>();
            Console.WriteLine("当前节名称:{0}",section.Name );
            Console.WriteLine("字符串SomeString值:{0}", someString);
            Console.WriteLine("整数someInteger值:{0}", someInteger);
            Console.WriteLine("双精度someFloat值:{0}", someFloat);
            Console.WriteLine("布尔值someBool值:{0}", someBool);
            section["SomeFloat"].Value = "333";
            config.Save("example.ini", Encoding.GetEncoding("gb2312"));*/
        }
        static void Test2()
        {
            var myConfig = new Configuration();
            //节点Video
            myConfig["Video"]["Width"].Value = "1920";
            myConfig["Video"]["Height"].Value = "1080";
            //设置数组
            myConfig["Video"]["Formats"].SetValue(new string[] { "RGB32", "RGBA32" });
            //可以使用循环获取节点以及节点的所有项目，进行操作
            foreach ( var section in myConfig )
            {
                foreach ( var setting in section )
                {
                    //TODO:
                }
            }
            //也可以直接使用节点和项目的名称来访问：
            myConfig.Save("example.ini");
            Console.WriteLine("Width:{0}", myConfig["Video"]["Width"].GetValue<Int32>());
            Console.WriteLine("Height:{0}", myConfig["Video"]["Height"].GetValue<Int32>());
        }
        static void Test3()
        {
            Configuration config = Configuration.LoadFromFile("example.ini");
            Person person = config["Person"].CreateObject<Person>();
            Console.WriteLine("Name:{0}",person.Name);
            Console.WriteLine("Age:{0}", person.Age);
            Console.WriteLine("Gender:{0}", person.Gender);
        }
        static void Test4()
        {

        }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    enum Gender
    {
        Male,
        Female
    }
}
