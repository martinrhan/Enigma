using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private readonly ConcurrentBag<(GameBody, Vector2)> additionalDisplacements = new ();
        internal void PendAdditionalDisplacement(GameBody gameBody, Vector2 displacement) {
            additionalDisplacements.Add((gameBody, displacement));
        }

        internal void ApplyAll(double deltaTime) {
            GameBody gameBody;
            while (pendingDestroyGameBodies.TryTake(out gameBody)) {
                gameWorld.RemoveGameBody(gameBody);
            }
            while (pendingAddGameBodies.TryTake(out gameBody)) {
                gameWorld.AddGameBody(gameBody);
            }
            (GameBody, Vector2) additionalDisplacement;
            while (additionalDisplacements.TryTake(out additionalDisplacement)) {
                additionalDisplacement.Item1.Translate(additionalDisplacement.Item2);
            }
            gameWorld.physicsWorld.ParallelForEach(body => {
                body.StepMovement(deltaTime);
            });
        }
    }
}
