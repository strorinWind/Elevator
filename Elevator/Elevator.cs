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
        None,
        Opening
    }

    public class Elevator
    {
        private int _n;

        public int N
        {
            get { return _n; }
            private set { _n = value; }
        }

        private double Height;
        private double Speed;
        private double OpenTime;
        private Direction MoveDirection = Direction.None;
        private int CurrentFloor;
        private DateTime TimeFromTheLastFloor;
        private TimeSpan FloorChangingTime;

        private List<int> _targetFloorsUp = new List<int>();
        private List<int> _targetFloorsDown = new List<int>();

        public void AddTargetFloor(int f)
        {
            if (f<5 | f>N)
            {
                throw new ArgumentException("Неверный номер этажа");
            }
            switch (f.CompareTo(CurrentFloor))
            {
                case -1:
                    if (_targetFloorsDown.Contains(f))
                        break;

                    _targetFloorsDown.Add(f);
                    _targetFloorsDown.Sort();
                    _targetFloorsDown.Reverse();
                    break;
                case 0:
                    break;
                case 1:
                    if (_targetFloorsUp.Contains(f))
                        break;

                    _targetFloorsUp.Add(f);
                    _targetFloorsUp.Sort();
                    break;
                default:
                    break;
            }
        }

        public void Move()
        {
            if (MoveDirection == Direction.None)
            {
                if (_targetFloorsDown.Count != 0 & _targetFloorsDown.Count >= _targetFloorsUp.Count)
                {
                    MoveDirection = Direction.Down;
                    TimeFromTheLastFloor = DateTime.Now;
                }
                else if (_targetFloorsUp.Count != 0 & _targetFloorsUp.Count >= _targetFloorsDown.Count)
                {
                    MoveDirection = Direction.Up;
                    TimeFromTheLastFloor = DateTime.Now;
                }
                else
                {
                    return; //Если мы так и не установили направление
                }
            }
            if (DateTime.Now - TimeFromTheLastFloor >= FloorChangingTime)
            {
                
            }
        }

        public Elevator(int n, double h, double s, double t)
        {
            if (n<5 | n>20)
            {
                throw new ArgumentException("Количество этажей может быть от 5 до 20.");
            }
            N = n;
            Height = h;
            Speed = s;
            OpenTime = t;
            CurrentFloor = 1;
            var c = Height / Speed * 1000;
            FloorChangingTime = new TimeSpan(0, 0, 0, 0, (int)c);
        }
    }
}
