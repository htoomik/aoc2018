using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2018.Code
{
    class Day20
    {
        public static List<string> GetPaths(string input)
        {
            var root = new Segment(null);
            var currentSegment = root;

            foreach (var c in input)
            {
                if ("NSEW".Contains(c))
                {
                    currentSegment.PathFromBeginning += c;
                }
                else if (c == '(')
                {
                    var newSegment = new Segment(currentSegment);
                    currentSegment.Onwards.Add(newSegment);
                    currentSegment = newSegment;
                }
                else if (c == '|')
                {
                    var newSegment = new Segment(currentSegment.Previous);
                    currentSegment.Previous.Onwards.Add(newSegment);
                    currentSegment = newSegment;
                }
                else if (c == ')')
                {
                    currentSegment = currentSegment.Previous;
                }
            }

            return GetPaths(root);
        }

        private static List<string> GetPaths(Segment root)
        {
            var paths = new List<string>();
            AddPaths(paths, root);
            return paths;
        }

        private static void AddPaths(List<string> paths, Segment segment)
        {
            if (segment.Onwards.Count == 0)
            {
                paths.Add(segment.CompletePath);
            }
            else
            {
                foreach (var segmentOnward in segment.Onwards)
                {
                    AddPaths(paths, segmentOnward);
                }
            }
        }

        public static string Map(string input)
        {
            var paths = GetPaths(input);
            var rooms = ExploreRooms(paths);

            Print(rooms);

            return GenerateMap(rooms);
        }

        private static Dictionary<Coords, Room> ExploreRooms(List<string> paths)
        {
            var origin = new Coords(0, 0);
            var startingRoom = new Room(origin, r => {});
            var rooms = new Dictionary<Coords, Room> { { origin, startingRoom } };

            foreach (var path in paths)
            {
                var current = startingRoom;
                foreach (var c in path)
                {
                    Room newRoom = null;
                    switch (c)
                    {
                        case 'N':
                        {
                            newRoom = current.MoveToNorth();
                            break;
                        }
                        case 'S':
                        {
                            newRoom = current.MoveToSouth();
                            break;
                        }
                        case 'E':
                        {
                            newRoom = current.MoveToEast();
                            break;
                        }
                        case 'W':
                        {
                            newRoom = current.MoveToWest();
                            break;
                        }
                    }

                    if (newRoom == null)
                    {
                        throw new Exception("null coords");
                    }

                    if (rooms.ContainsKey(newRoom.Coords))
                    {
                        current = rooms[newRoom.Coords];
                    }
                    else
                    {
                        rooms.Add(newRoom.Coords, newRoom);
                        current = newRoom;
                    }
                }
            }

            return rooms;
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

        private static void Print(Dictionary<Coords, Room> rooms)
        {
            const string path = "C:\\Code\\aoc2018\\output20.txt";
            File.Delete(path);

            foreach (var room in rooms.Values)
            {
                File.AppendAllText(path, $"[{room.Coords.X}, {room.Coords.Y}], North: {room.DoorToNorth}, South: {room.DoorToSouth}, East: {room.DoorToEast}, West: {room.DoorToWest}\r\n");
            }
        }

        public static int Solve(string input)
        {
            var paths = GetPaths(input);
            var rooms = ExploreRooms(paths);

            var origin = new Coords(0, 0);
            
            var cost = new Dictionary<Coords, int>();
            cost.Add(origin, 0);

            var frontier = new Queue<Coords>();
            frontier.Enqueue(origin);

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                var room = rooms[current];
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

        private class Segment
        {
            public Segment Previous { get; }
            public string PathToBeginning { get; }
            public string PathFromBeginning { get; set; }
            public string CompletePath => PathToBeginning + PathFromBeginning;

            public readonly List<Segment> Onwards = new List<Segment>();

            public Segment(Segment previous)
            {
                Previous = previous;
                PathToBeginning = previous?.CompletePath;
            }
        }
    }
}
