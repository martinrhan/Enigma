using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.Game;
using Enigma.Spacial;

namespace Enigma.Game.Extensions {
    public static class GameWorldExtensions {
        public static IEnumerable<GameBody> QueryCollisionDictionary(this GameWorld gameWorld, GameBody gameBody) => gameWorld.physicsWorld.QueryCollisionDictionary(gameBody);

        public static IEnumerable<GameBody> CircleHitTest(this GameWorld gameWorld, in Vector2 center, double radius) => gameWorld.physicsWorld.CircleHitTest(center, radius, gameBody => true);

        public static IEnumerable<GameBody> CircularSectionHitTest(this GameWorld gameWorld, in Vector2 center, double radius, double angleStart, double theta) {
            Vector2 c = center;
            return gameWorld.physicsWorld.CircleHitTest(center, radius, gameBody => {
                Vector2 displacement = gameWorld.GetPositiveDisplacement(c, gameBody.Center);
                double direction = displacement.Direction;
                double relativeDirection = direction - angleStart;
                return 0 < relativeDirection && relativeDirection < theta;
            });
        }

        public static IEnumerable<GameBody> AABBHitTest(this GameWorld gameWorld, in AABB aABB) => gameWorld.physicsWorld.AABBHitTest(aABB, gameBody => true);

        public static GameBody FindNearest(this GameWorld gameWorld, Vector2 center, double range, Predicate<GameBody> predicate) {
            GameBody currentChosen = default;
            double knownShortestDistance = default;
            foreach (GameBody gameBody in gameWorld.physicsWorld.CircleHitTest(center, range, predicate)) {
                if (currentChosen == null) {
                    currentChosen = gameBody;
                } else {
                    double distance = gameWorld.GetShortestDisplacement(center, gameBody.Center).Length;
                    if (distance < knownShortestDistance) {
                        currentChosen = gameBody;
                        knownShortestDistance = distance;
                    }
                }
                if (currentChosen != null) {
                    return currentChosen;
                }
            }
            return null;
        }
        public static Vector2 GetShortestDisplacement(this GameWorld gameWorld, in Vector2 from, in Vector2 to) => gameWorld.physicsWorld.GetShortestDisplacement(from, to);
        public static Vector2 GetPositiveDisplacement(this GameWorld gameWorld, in Vector2 from, in Vector2 to) => gameWorld.GetPositivePosition(to - from);
        public static Vector2 GetPositivePosition(this GameWorld gameWorld, in Vector2 position) {
            (double x, double y) = position;
            if (x < 0) x += gameWorld.Width;
            if (y < 0) y += gameWorld.Height;
            return new Vector2(x, y);
        }
        /// <summary>
        /// Returns a position that is positive but less than gameWorld dimention.
        /// </summary>
        /// <param name="gameWorld"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 GetClampedPosition(this GameWorld gameWorld, in Vector2 position) => gameWorld.physicsWorld.GetClampedPosition(position);
    }
}
