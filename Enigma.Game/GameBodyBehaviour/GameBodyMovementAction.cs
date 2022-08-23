using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.Game;

namespace Enigma.Game {

    /// <summary>
    /// Defines a simple movement job. It is called every frame to set movement direction of the GameBody at that frame.
    /// </summary>
    /// <typeparam name="T">Applicable GameBody type</typeparam>
    public abstract partial class GameBodyMovementAction : GameBodyAction {
        internal bool Update_Internal(GameBody gameBody, double deltaTime) {
            UpdateInterface updateInterface = new UpdateInterface(gameBody, deltaTime);
            if (!IsStarted) {
                if (Start(updateInterface)) {
                    IsCompleted = true;
                    return true;
                }
                IsStarted = true;
            }
            if (Update(updateInterface)) {
                IsCompleted = true;
                return true;
            } else return false;
        }
        protected abstract bool Start(UpdateInterface updateInterface);
        protected abstract bool Update(UpdateInterface updateInterface);
        internal void Cancel_Internal(GameBody gameBody, double deltaTime) {
            UpdateInterface updateInterface = new UpdateInterface(gameBody, deltaTime);
            Cancel(updateInterface);
        }
        protected abstract void Cancel(UpdateInterface updateInterface);

        public class UpdateInterface {
            internal UpdateInterface(GameBody gameBody, double deltaTime) {
                GameBody = gameBody;
                DeltaTime = deltaTime;
            }
            public readonly double DeltaTime;
            public readonly GameBody GameBody;

            public void SetMovement(double direction) => GameBody.SetMovement(direction);
            public void StopMovement() {
                GameBody.SetMovementToZero();
            }
        }
    }
}
