using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc2018.Code.Day15
{
    public class Unit
    {
        public int Row { get; }
        public int Col { get; }
        public Race Race { get; }
        public int HitPoints { get; }

        private readonly Engine _engine;

        public Unit(int row, int col, Race race, Engine engine = null)
        {
            _engine = engine;

            Row = row;
            Col = col;
            Race = race;
            HitPoints = 200;
        }

        public void Move()
        {
            var targetsInRange = _engine.GetAdjacentEnemies(this);
            if (targetsInRange.Any())
                return;

            var (target, routeLength) = ChooseTarget();
            var theRoute = ChooseRoute(target, routeLength);
            Step(theRoute);
        }

        public Route ChooseRoute(Coords target, int routeLength)
        {
            var shortestRoutes = _engine.FindAllRoutes(this, target, routeLength);
            var theRoute = shortestRoutes.OrderBy(r => r.FirstStep.Row).ThenBy(r => r.FirstStep.Col).First();
            return theRoute;
        }

        public (Coords target, int routeLength) ChooseTarget()
        {
            var enemies = _engine.GetAllEnemies(this);
            var moveTargets = _engine.GetSquaresInRangeOf(enemies);
            var reachableTargets = _engine.GetReachableTargets(this, moveTargets);
            var routes = GetRoutes(reachableTargets);
            var minLength = routes.Min(r => r.Length);
            var closestTargets = routes.Where(r => r.Length == minLength);
            var topLeftTarget = closestTargets.Select(r => r.Target).OrderBy(e => e.Row).ThenBy(e => e.Col).First();
            return (topLeftTarget, minLength);
        }

        private void Step(Route route)
        {
            throw new NotImplementedException();
        }

        private List<Route> GetRoutes(List<Coords> targets)
        {
            var routes = new List<Route>();
            var coords = new Coords(Row, Col);
            foreach (var target in targets)
            {
                routes.Add(_engine.FindShortestRoute(coords, target));
            }

            return routes;
        }

        public void Attack()
        {
            var targetsInRange = _engine.GetAdjacentEnemies(this);
            if (!targetsInRange.Any())
                return;

            var theTarget = targetsInRange.OrderBy(t => t.HitPoints).ThenBy(t => t.Row).ThenBy(t => t.Col).First();
            Attack(theTarget);
        }

        private void Attack(Unit target)
        {
            throw new NotImplementedException();
        }
    }
}
