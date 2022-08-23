using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;

namespace Enigma.Game {
    internal class GameWorldOperationManager {
        internal GameWorldOperationManager(GameWorld gameWorld) {
            this.gameWorld = gameWorld;
        }
        private readonly GameWorld gameWorld;

        private readonly ConcurrentBag<GameBody> pendingAddGameBodies = new ConcurrentBag<GameBody>();
        internal void PendAddGameBody(GameBody gameBody) {
            pendingAddGameBodies.Add(gameBody);
        }

        private readonly ConcurrentBag<GameBody> pendingDestroyGameBodies = new ConcurrentBag<GameBody>();
        internal void PendDestroyGameBody(GameBody gameBody) {
            pendingDestroyGameBodies.Add(gameBody);
        }

        internal void ApplyAll(double deltaTime) {
            foreach (GameBody gameBody in pendingDestroyGameBodies) {
                gameWorld.RemoveGameBody(gameBody);
            }
            pendingDestroyGameBodies.Clear();
            foreach (GameBody gameBody in pendingAddGameBodies) {
                gameWorld.AddGameBody(gameBody);
            }
            pendingAddGameBodies.Clear();
            gameWorld.physicsWorld.ParallelForEach(body => {
                body.StepMovement(deltaTime);
            });
        }
    }
}
