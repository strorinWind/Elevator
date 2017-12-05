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

        private TimeSpan OpenTime;
        private Direction MoveDirection = Direction.None;
        private int CurrentFloor;
        private DateTime TimeFromTheLastFloor;
        private TimeSpan FloorChangingTime;

        private List<int> _targetFloorsUp = new List<int>();
        private List<int> _targetFloorsDown = new List<int>();

        public void AddTargetFloor(int f)
        {
            if (f < 5 | f > N)
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

        private bool SetDirectionIfNone()
        {
            if (_targetFloorsDown.Count != 0 & _targetFloorsDown.Count >= _targetFloorsUp.Count)
            {
                MoveDirection = Direction.Down;
                TimeFromTheLastFloor = DateTime.Now;
                return true;
            }
            else if (_targetFloorsUp.Count != 0 & _targetFloorsUp.Count >= _targetFloorsDown.Count)
            {
                MoveDirection = Direction.Up;
                TimeFromTheLastFloor = DateTime.Now;
                return true;
            }
            return false;
        }

        public void Move()
        {
            if (MoveDirection == Direction.None & !SetDirectionIfNone())
            {
                return; //Состояние так и осталось неопределенным
            }

            if (MoveDirection == Direction.Up || MoveDirection == Direction.Down)
            {
                if (DateTime.Now - TimeFromTheLastFloor >= FloorChangingTime)
                {
                    TimeFromTheLastFloor += FloorChangingTime;
                    if (MoveDirection == Direction.Up)
                    {
                        CurrentFloor += 1;
                        if (_targetFloorsUp.Contains(CurrentFloor))
                        {
                            MoveDirection = Direction.Opening;
                            _targetFloorsUp.Remove(CurrentFloor);
                        }
                    }
                    else
                    {
                        CurrentFloor -= 1;
                        if (_targetFloorsDown.Contains(CurrentFloor))
                        {
                            MoveDirection = Direction.Opening;
                            _targetFloorsDown.Remove(CurrentFloor);
                        }
                    }
                }
            }
            else if (MoveDirection == Direction.Opening)
            {

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
        }
    }
}
