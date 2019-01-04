using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2018.Code
{
    public class Day20
    {
        private readonly Dictionary<Coords, Room> _rooms = new Dictionary<Coords, Room>();

        public string Map(string input)
        {
            ExploreRooms(input);

            return GenerateMap(_rooms);
        }

        private void ExploreRooms(string input)
        {
            var origin = new Coords(0, 0);
            var startingRoom = new Room(origin, r => {});
            _rooms.Add(origin, startingRoom);
            var current = startingRoom;

            using (var it = input.GetEnumerator())
            {
                Explore(it, current);
            }
        }

        private void Explore(CharEnumerator it, Room current)
        {
            var start = current;

            while (it.MoveNext())
            {
                var c = it.Current;
                if ("NSEW".Contains(c))
                {
                    switch (c)
                    {
                        case 'N':
                        {
                            current = current.MoveToNorth();
                            break;
                        }
                        case 'S':
                        {
                            current = current.MoveToSouth();
                            break;
                        }
                        case 'E':
                        {
                            current = current.MoveToEast();
                            break;
                        }
                        case 'W':
                        {
                            current = current.MoveToWest();
                            break;
                        }
                    }

                    if (!_rooms.ContainsKey(current.Coords))
                    {
                        _rooms.Add(current.Coords, current);
                    }
                }

                if (c == '|')
                {
                    // Go back to the starting point of this exploration
                    current = start;
                }

                if (c == '(')
                {
                    // Branching starts here. Remember this point so that we can come back to it when the branching ends.
                    Explore(it, current);
                }

                if (c == ')')
                {
                    // End of branch. Go back to last branching point.
                    return;
                }
            }
        }

        private static string GenerateMap(Dictionary<Coords, Room> rooms)
        {
            var minX = rooms.Keys.Min(c => c.X);
            var minY = rooms.Keys.Min(c => c.Y);
            var maxX = rooms.Keys.Max(c => c.X);
            var maxY = rooms.Keys.Max(c => c.Y);

            var sb = new StringBuilder();
            sb.AppendLine(new string('#', (maxX - minX + 1) * 2 + 1));
            for (var j = 0; j < maxY - minY + 1; j++)
            {
                // Offset to our coordinate system
                var y = j + minY;

                // Left wall
                sb.Append("#");

                // East-west doors
                for (var i = 0; i < maxX - minX + 1; i++)
                {
                    // Offset to our coordinate system
                    var x = i + minX;

                    // Room
                    if (x == 0 && y == 0)
                        sb.Append('X');
                    else
                        sb.Append('.');

                    var westRoom = rooms.ContainsKey(new Coords(x, y)) ? rooms[new Coords(x, y)] : null;
                    if (westRoom != null && westRoom.DoorToEast)
                    {
                        sb.Append('|');
                    }
                    else
                    {
                        sb.Append('#');
                    }
                }

                sb.AppendLine();

                // Left wall
                sb.Append("#");

                // North-south doors
                for (var i = 0; i < maxX - minX + 1; i++)
                {
                    // Offset to our coordinate system
                    var x = i + minX;

                    var northRoom = rooms.ContainsKey(new Coords(x, y)) ? rooms[new Coords(x, y)] : null;
                    if (northRoom != null && northRoom.DoorToSouth)
                    {
                        sb.Append('-');
                    }
                    else
                    {
                        sb.Append('#');
                    }

                    // Pillar
                    sb.Append('#');
                }

                sb.AppendLine();
            }

            var map = sb.ToString();
            return map;
        }

        public int Solve(string input)
        {
            ExploreRooms(input);

            var origin = new Coords(0, 0);
            
            var cost = new Dictionary<Coords, int>();
            cost.Add(origin, 0);

            var frontier = new Queue<Coords>();
            frontier.Enqueue(origin);

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                var room = _rooms[current];
                var newCost = cost[current] + 1;

                if (room.DoorToNorth && !cost.ContainsKey(current.ToNorth))
                {
                    frontier.Enqueue(current.ToNorth);
                    cost[current.ToNorth] = newCost;
                }

                if (room.DoorToSouth && !cost.ContainsKey(current.ToSouth))
                {
                    frontier.Enqueue(current.ToSouth);
                    cost[current.ToSouth] = newCost;
                }

                if (room.DoorToEast && !cost.ContainsKey(current.ToEast))
                {
                    frontier.Enqueue(current.ToEast);
                    cost[current.ToEast] = newCost;
                }

                if (room.DoorToWest && !cost.ContainsKey(current.ToWest))
                {
                    frontier.Enqueue(current.ToWest);
                    cost[current.ToWest] = newCost;
                }
            }

            var maxCost = cost.Values.Max();

            return maxCost;
        }

        private struct Coords
        {
            public Coords(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }

            public Coords ToNorth => new Coords(X, Y - 1);
            public Coords ToSouth => new Coords(X, Y + 1);
            public Coords ToEast => new Coords(X + 1, Y);
            public Coords ToWest => new Coords(X - 1, Y);
        }

        private class Room
        {
            public Room(Coords coords, Action<Room> makeDoor)
            {
                Coords = coords;
                makeDoor(this);
            }

            public Coords Coords { get; }

            public bool DoorToNorth { get; private set; }
            public bool DoorToSouth { get; private set; }
            public bool DoorToEast { get; private set; }
            public bool DoorToWest { get; private set; }

            public Room MoveToNorth()
            {
                DoorToNorth = true;
                return new Room(Coords.ToNorth, r => r.DoorToSouth = true);
            }

            public Room MoveToSouth()
            {
                DoorToSouth = true;
                return new Room(Coords.ToSouth, r => r.DoorToNorth = true);
            }

            public Room MoveToEast()
            {
                DoorToEast = true;
                return new Room(Coords.ToEast, r => r.DoorToWest = true);
            }

            public Room MoveToWest()
            {
                DoorToWest = true;
                return new Room(Coords.ToWest, r => r.DoorToEast = true);
            }
        }
    }
}
