using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator
{
    class Program
    {
        private Elevator el;

        static void Main(string[] args)
        {
            var p = new Program();
            p.el = p.InputStartData();
            p.PrintInstructions();
            /*var t = Task.Run(() => p.Loop());
            while (true)
            {
                Task.Delay(1000);
                p.el.AddTargetFloor(int.Parse(Console.ReadLine()));
            }*/
            p.Loop();
        }

        private Elevator InputStartData()
        {
            Console.Write("Введите кол-во этажей в подъезде (от 5 до 20): ");
            int n;
            while (!int.TryParse(Console.ReadLine(), out n) | n < 5 | n > 20)
            {
                Console.Write("Что-то пошло не так. Введите кол-во этажей в подъезде (от 5 до 20): ");
            }

            Console.Write("Введите высоту одного этажа (в метрах): ");
            double h;
            while (!double.TryParse(Console.ReadLine(), out h) | h <= 0)
            {
                Console.Write("Что-то пошло не так. Введите высоту одного этажа (в метрах): ");
            }

            Console.Write("Введите скорость движения лифта (в метрах в секунду): ");
            double s;
            while (!double.TryParse(Console.ReadLine(), out s) | s <= 0)
            {
                Console.Write("Что-то пошло не так. Введите скорость движения лифта (в метрах в секунду): ");
            }

            Console.Write("Введите время между открытием и закрытием дверей (в секундах): ");
            double t;
            while (!double.TryParse(Console.ReadLine(), out t) | t <= 0)
            {
                Console.Write("Что-то пошло не так. Введите время между открытием и закрытием дверей (в секундах): ");
            }

            return new Elevator(n, h, s, t);
        }

        private void PrintInstructions()
        {
            Console.WriteLine();
            Console.WriteLine(@"В течение выполнения программы вы можете печатать числа, это будут " +
                   "нажатия на кнопки лифта на этажах или внутри кабины. Ввод должен быть завершен нажатием " +
                   "клавиши 'Ввод'.");
        }

        private void Loop()
        {
            //var t = Console.In.ReadLineAsync();
            var t = Task.Run(() => Console.ReadLine());
            //t.RunSynchronously();
            //var t = Console.Read();
            while (true)
            {
                //var s = Console.;
                //Task.Delay(10000);
                //Thread.Sleep(1000);
                //Console.WriteLine("Все идет нормально");
                if (t.IsCompleted)
                {
                    int num;
                    if (!int.TryParse(t.Result, out num) | num < 1 | num > el.N)
                    {
                        Console.WriteLine("Некорректный формат ввода.");
                        Console.WriteLine(el.TimeAtTheLastFloor + " " + el.MoveDirection + " " + el.CurrentFloor);
                        Console.WriteLine((DateTime.Now - el.TimeAtTheLastFloor >= el.FloorChangingTime));
                        Console.WriteLine(el.FloorChangingTime + " " + (DateTime.Now - el.TimeAtTheLastFloor) + " "+ el.TimeAtTheLastFloor);
                    }
                    else
                    {
                        //Console.WriteLine(t.Result);
                        el.AddTargetFloor(num);
                    }       
                    t = Task.Run(() => Console.ReadLine());
                    //t.S
                }
                el.Move();
                //Console.WriteLine(DateTime.Now +" "+ el.TimeFromTheLastFloor.ToString() + " " + (DateTime.Now - el.TimeFromTheLastFloor).ToString());
            }
        }
    }
}
