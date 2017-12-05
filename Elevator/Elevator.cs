using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public enum Direction
    {
        Up,
        Down,
        None
    }

    public class Elevator
    {
        private int _n;

        public int N
        {
            get { return _n; }
            private set { _n = value; }
        }

        private TimeSpan OpenTime;
        public Direction MoveDirection;
        public int CurrentFloor;
        public DateTime TimeAtTheLastFloor;
        public TimeSpan FloorChangingTime;
        private bool Opening = false;

        private List<int> _targetFloorsUp = new List<int>();
        private List<int> _targetFloorsDown = new List<int>();

        public void AddTargetFloor(int f)
        {
            if (f < 1 |f > N)
            {
                throw new ArgumentException("Неверный номер этажа");
            }
            switch (f.CompareTo(CurrentFloor))
            {
                case -1:
                    if (_targetFloorsDown.Contains(f))
                        break;

                    _targetFloorsDown.Add(f);
                    break;
                case 0:
                    break;
                case 1:
                    if (_targetFloorsUp.Contains(f))
                        break;

                    _targetFloorsUp.Add(f);
                    break;
                default:
                    break;
            }
            Console.WriteLine("Добавлено "+f);
        }

        private bool SetDirectionIfNone()
        {
            if (_targetFloorsDown.Count != 0 & _targetFloorsDown.Count >= _targetFloorsUp.Count)
            {
                MoveDirection = Direction.Down;
                TimeAtTheLastFloor = DateTime.Now;
                return true;
            }
            else if (_targetFloorsUp.Count != 0 & _targetFloorsUp.Count >= _targetFloorsDown.Count)
            {
                MoveDirection = Direction.Up;
                TimeAtTheLastFloor = DateTime.Now;
                return true;
            }
            return false;
        }

        public void Move()
        {
            if (MoveDirection == Direction.None)
                if (!SetDirectionIfNone())
                {
                    return; //Состояние так и осталось неопределенным
                }

            if (Opening)
            {
                if (DateTime.Now - TimeAtTheLastFloor > OpenTime)
                {
                    Opening = false;
                    Console.WriteLine("Лифт закрыл двери");
                }
            }
            else if (DateTime.Now - TimeAtTheLastFloor >= FloorChangingTime)
            {
                if (MoveDirection == Direction.Up)
                {                  
                    //TimeFromTheLastFloor = DateTime.Now;
                    CurrentFloor += 1;
                    Console.WriteLine("Лифт проезжает этаж " + CurrentFloor);
                    if (_targetFloorsUp.Contains(CurrentFloor))
                    {
                        Opening = true;
                        Console.WriteLine("Лифт открыл двери");
                        _targetFloorsUp.Remove(CurrentFloor);
                    }
                    //TimeAtTheLastFloor.Add(FloorChangingTime);
                }
                else
                {                   
                    CurrentFloor -= 1;
                    Console.WriteLine("Лифт проезжает этаж " + CurrentFloor);
                    if (_targetFloorsDown.Contains(CurrentFloor))
                    {
                        Opening = true;
                        Console.WriteLine("Лифт открыл двери");
                        _targetFloorsDown.Remove(CurrentFloor);
                    }
                    //TimeAtTheLastFloor.Add(FloorChangingTime);
                }
                TimeAtTheLastFloor.Add(FloorChangingTime);
            }

        }

        public Elevator(int n, double h, double s, double t)
        {
            if (n < 5 | n > 20)
            {
                throw new ArgumentException("Количество этажей может быть от 5 до 20.");
            }
            if (h <= 0 | s <= 0 | t <= 0)
            {
                throw new ArgumentException("Параметры должны быть положительными.");
            }
            N = n;
            OpenTime = new TimeSpan(0, 0, 0, 0, (int)t * 1000);
            CurrentFloor = 1;
            var c = h / s * 1000;
            FloorChangingTime = new TimeSpan(0, 0, 0, 0, (int)c);
            MoveDirection = Direction.None;
            TimeAtTheLastFloor = DateTime.Now;
        }
    }
}
