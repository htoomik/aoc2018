using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace aoc2018.Code
{
    class Day13
    {
        private char[][] _tracks;
        private List<Cart> _carts = new List<Cart>();
        
        public Coords Solve1(string input)
        {
            Parse(input);

            Cart collision;
            do
            {
                collision = AllCartsMove();
            } while (collision == null);

            return collision.Coordinates;
        }

        public Coords Solve2(string input)
        {
            Parse(input);

            do
            {
                void InCaseOfCollision(Cart cart1, Cart cart2)
                {
                    cart1.Kill();
                    cart2.Kill();
                }

                AllCartsMove(InCaseOfCollision);
                _carts = _carts.Where(c => c.Alive).ToList();
            } while (_carts.Count > 1);

            var single = _carts.Single();
            return single.Coordinates;
        }

        private void Parse(string input)
        {
            var lines = input.Trim('\r').Trim('\n').Split("\r\n");
            _tracks = lines.Select(l => l.ToCharArray()).ToArray();

            for (var r = 0; r < _tracks.Length; r++)
            {
                for (var c = 0; c < _tracks[0].Length; c++)
                {
                    var (cart, track) = LookForCart(r, c, _tracks[r][c]);
                    if (cart == null) continue;

                    _carts.Add(cart);
                    _tracks[r][c] = track;
                }
            }
        }

        private static (Cart, char) LookForCart(int r, int c, char t)
        {
            Cart cart = null;
            char track = t;
            switch (t)
            {
                case '>':
                {
                    cart = new Cart(r, c, MoveDirection.Right);
                    track = '-';
                    break;
                }
                case '<':
                {
                    cart = new Cart(r, c, MoveDirection.Left);
                    track = '-';
                    break;
                }
                case 'v':
                {
                    cart = new Cart(r, c, MoveDirection.Down);
                    track = '|';
                    break;
                }
                case '^':
                {
                    cart = new Cart(r, c, MoveDirection.Up);
                    track = '|';
                    break;
                }
            }

            return (cart, track);
        }

        private Cart AllCartsMove(Action<Cart, Cart> inCaseOfCollision = null)
        {
            _carts = _carts
                .OrderBy(c => c.Coordinates.Row)
                .ThenBy(c => c.Coordinates.Col)
                .ToList();

            foreach (var cart in _carts)
            {
                var track = _tracks[cart.Coordinates.Row][cart.Coordinates.Col];
                cart.Move(track);
                var collidedWith = CheckForCollision(cart);

                if (collidedWith != null)
                {
                    if (inCaseOfCollision == null)
                    {
                        return collidedWith;
                    }

                    inCaseOfCollision(cart, collidedWith);
                }
            }

            return null;
        }

        private Cart CheckForCollision(Cart cart)
        {
            foreach (var otherCart in _carts)
            {
                if (cart == otherCart)
                    continue;
                if (cart.Coordinates.Equals(otherCart.Coordinates))
                {
                    return otherCart;
                }
            }

            return null;
        }

        [DebuggerDisplay("Row: {Row}, Col: {Col}")]
        public class Coords
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public Coords(int row, int col)
            {
                Row = row;
                Col = col;
            }

            public bool Equals(Coords other)
            {
                return Row == other.Row &&
                       Col == other.Col;
            }
        }

        [DebuggerDisplay("At {Coordinates.Row}, {Coordinates.Col}")]
        private class Cart
        {
            public Coords Coordinates { get; }
            public bool Alive { get; private set; }

            private TurnDirection NextTurnDirection { get; set; }
            private MoveDirection MoveDirection { get; set; }

            public Cart(int row, int col, MoveDirection moveDirection)
            {
                Alive = true;
                Coordinates = new Coords(row, col);
                MoveDirection = moveDirection;
                NextTurnDirection = TurnDirection.Left;
            }

            public void Move(char track)
            {
                if (track == '+')
                {
                    MoveDirection = Turn();
                    NextTurnDirection = GetNextTurnDirection();

                    switch (MoveDirection)
                    {
                        case MoveDirection.Right:
                            Coordinates.Col++;
                            break;
                        case MoveDirection.Down:
                            Coordinates.Row++;
                            break;
                        case MoveDirection.Left:
                            Coordinates.Col--;
                            break;
                        case MoveDirection.Up:
                            Coordinates.Row--;
                            break;
                    }

                    //Print();

                    return;
                }

                switch (MoveDirection)
                {
                    case MoveDirection.Right:
                    {
                        switch (track)
                        {
                            case '-':
                                Coordinates.Col++;
                                break;
                            case '\\':
                                Coordinates.Row++;
                                MoveDirection = MoveDirection.Down;
                                break;
                            case '/':
                                Coordinates.Row--;
                                MoveDirection = MoveDirection.Up;
                                break;
                            default:
                                throw new Exception("Off track");
                        }

                        break;
                    }
                    case MoveDirection.Down:
                    {
                        switch (track)
                        {
                            case '|':
                                Coordinates.Row++;
                                break;
                            case '\\':
                                Coordinates.Col++;
                                MoveDirection = MoveDirection.Right;
                                break;
                            case '/':
                                Coordinates.Col--;
                                MoveDirection = MoveDirection.Left;
                                break;
                            default:
                                throw new Exception("Off track");
                        }

                        break;
                    }
                    case MoveDirection.Left:
                    {
                        switch (track)
                        {
                            case '-':
                                Coordinates.Col--;
                                break;
                            case '\\':
                                Coordinates.Row--;
                                MoveDirection = MoveDirection.Up;
                                break;
                            case '/':
                                Coordinates.Row++;
                                MoveDirection = MoveDirection.Down;
                                break;
                            default:
                                throw new Exception("Off track");
                        }

                        break;
                    }
                    case MoveDirection.Up:
                    {
                        switch (track)
                        {
                            case '|':
                                Coordinates.Row--;
                                break;
                            case '\\':
                                Coordinates.Col--;
                                MoveDirection = MoveDirection.Left;
                                break;
                            case '/':
                                Coordinates.Col++;
                                MoveDirection = MoveDirection.Right;
                                break;
                            default:
                                throw new Exception("Off track");
                        }

                        break;
                    }
                }
            }

            private MoveDirection Turn()
            {
                if (NextTurnDirection == TurnDirection.Straight)
                {
                    return MoveDirection;
                }

                switch (MoveDirection)
                {
                    case MoveDirection.Right:
                        switch (NextTurnDirection)
                        {
                            case TurnDirection.Left: return MoveDirection.Up;
                            case TurnDirection.Right: return MoveDirection.Down;
                        }

                        break;
                    case MoveDirection.Down:
                        switch (NextTurnDirection)
                        {
                            case TurnDirection.Left: return MoveDirection.Right;
                            case TurnDirection.Right: return MoveDirection.Left;
                        }

                        break;
                    case MoveDirection.Left:
                        switch (NextTurnDirection)
                        {
                            case TurnDirection.Left: return MoveDirection.Down;
                            case TurnDirection.Right: return MoveDirection.Up;
                        }

                        break;
                    case MoveDirection.Up:
                        switch (NextTurnDirection)
                        {
                            case TurnDirection.Left: return MoveDirection.Left;
                            case TurnDirection.Right: return MoveDirection.Right;
                        }

                        break;
                }

                throw new Exception("Don't know where to turn");
            }

            private TurnDirection GetNextTurnDirection()
            {
                var nextDirection = ((int) NextTurnDirection + 1) % 3;
                return (TurnDirection) nextDirection;
            }

            public void Kill()
            {
                Alive = false;
            }
        }

        private enum TurnDirection
        {
            Left,
            Straight,
            Right
        }

        private enum MoveDirection
        {
            Right,
            Down,
            Left,
            Up,
        }
    }
}
