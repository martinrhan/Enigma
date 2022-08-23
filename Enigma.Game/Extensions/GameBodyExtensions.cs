using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enigma.Game.Extensions.GameWorldExtensions;

namespace Enigma.Game.Extensions {
    public static class GameBodyExtensions {
        public static double GetCenterDistance(this GameBody gameBody, GameBody target) =>
            target.GameWorld.GetShortestDisplacement(target.Center, gameBody.Center).Length;

        public static double GetBounderyDistance(this GameBody gameBody, GameBody target) => 
            target.GameWorld.GetShortestDisplacement(target.Center, gameBody.Center).Length - target.Radius - gameBody.Radius;


    }
}
