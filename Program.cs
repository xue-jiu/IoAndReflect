using System;
using System.Reflection;

namespace IoAndReflect
{
    class Program
    {
        static void Main(string[] args)
        {
            //此部分为文件调用
            //添加引用就是 将dll文件复制到bin.exe目录中
            Assembly assembly = Assembly.Load("Assembly1");
            Console.WriteLine(nameof(assembly));
            string path = Environment.CurrentDirectory;
            Console.WriteLine(path);
            Console.WriteLine(path + @"\Assembly2");
            Assembly assembly1 = Assembly.LoadFile(path + @"\Assembly2");
            //以上两种都可以 全路径或者加载进exe
            Assembly assembly2 = Assembly.LoadFrom(path + @"\Assembly2");
            assembly2 = Assembly.LoadFrom("Assembly1");

            //开始反射创造类
            Console.WriteLine("开始通过反射实例化类");
            var type1 = assembly.GetType("Assembly1.Dog");//获得类
            Console.WriteLine(type1);
            Activator.CreateInstance(type1);
            Activator.CreateInstance(type1,"wangwang");//带参
            Activator.CreateInstance(type1,new object[]{ "dadad","16"});//params 关键字解析参数
            Activator.CreateInstance(type1, "dada","123");//带参
        }
    }
}
