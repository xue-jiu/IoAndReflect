using Assembly1;
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
            //Activator.CreateInstance(type1, true,"cat");//报错了,先不学

            Console.WriteLine("--------------------------------------------------------------");
            //
            foreach (var item in type1.GetConstructors(BindingFlags.Instance| BindingFlags.Public))//BindingFlags可以限制找哪些BindingFlags.Public  "|"表示按位或
            {
                Console.WriteLine($"构造方法 {item}");
                foreach (var paramsOfConstructor in item.GetParameters())
                {
                    Console.WriteLine($"构造方法的参数{paramsOfConstructor}");
                }
            }
            Console.WriteLine("--------------------------------------------------------------");
            foreach (var item in assembly.GetTypes())//查找所有的类type[]
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------调用普通方法-------------------------------");
            var instanceOfDog= Activator.CreateInstance(type1);
            Dog dog = instanceOfDog as Dog;
            dog.run();
            foreach (var item in type1.GetMethods())
            {
                Console.WriteLine(item);
            }
            var method= type1.GetMethod("run", new Type[] {typeof(int)});//重载调用
            method.Invoke(instanceOfDog,new object[]{12});

            Console.WriteLine("-------------------------------调用泛型方法-------------------------------");
            var method1 = type1.GetMethod("Eat");
            var genrikMenthod = method1.MakeGenericMethod(new Type[] { typeof(string)});//传入泛型
            genrikMenthod.Invoke(instanceOfDog,new object[] { });//空参
            var mytype = typeof(string);
            Console.WriteLine("-------------------------------实例化泛型类-------------------------------");
            var type3 = assembly.GetType("Assembly1.house`1").MakeGenericType(typeof(Dog));//new Type[] {typeof(string)}
            var genrikClass = Activator.CreateInstance(type3);
            var method2 = type3.GetMethod("EatBy");
            if (method2!=null)
            {
                Console.WriteLine("方法不为空");
            }
            Console.WriteLine(nameof(method2));
            foreach (var item in method2.GetParameters())
            {
                Console.WriteLine(item);
            };
            var smalldog = new Dog();
            method2.Invoke(genrikClass, new object[] {smalldog});
            //var hourrse = genrikClass as house<Dog>;
            //hourrse.EatBy(smalldog);
            Console.WriteLine("-------------------------------操作属性和字段-------------------------------");
            var type4 = assembly.GetType("Assembly1.Dog");
            var classs = Activator.CreateInstance(type4);//若构造函数是私有 需要加上ture
            var classs2 = Activator.CreateInstance(type4);
            var propertys = type4.GetProperties();
            foreach (var item in propertys)
            {
                Console.WriteLine(item);
                //设置值
                if (item.Name=="Name")
                {
                    item.SetValue(classs,"hahahhahaha");
                    item.SetValue(classs2, "dadadaadad");
                }
                //获取属性值
                Console.WriteLine($"这是值{item.GetValue(classs)}");
            }
            Console.WriteLine("-------------------------------私有方法-------------------------------");
            var PrivateMethod = type4.GetMethod("Attack",BindingFlags.Instance| BindingFlags.NonPublic);
            PrivateMethod.Invoke(classs2,new object[] { });

            var Dooog = classs2 as Dog;
            //Dooog.Attack();
        }
    }
}
