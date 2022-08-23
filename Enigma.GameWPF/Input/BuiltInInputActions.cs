using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Input {
    public partial class InputBindingManager {
        private static readonly InputAction[] builtInInputActions = new InputAction[] {
            new InputAction() {
                Id = "MoveLeft",
                DownAction = (c,m) => c.LeftButtonDown(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) => c.LeftButtonUp()
            },
            new InputAction() {
                Id = "MoveRight",
                DownAction = (c,m) =>  c.RightButtonDown(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) =>  c.RightButtonUp()
            },
            new InputAction() {
                Id = "MoveUp",
                DownAction = (c,m) => c.UpButtonDown(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) =>  c.UpButtonUp()
            },
            new InputAction() {
                Id = "MoveDown",
                DownAction = (c,m) => c.DownButtonDown(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) =>  c.DownButtonUp()
            },
            new InputAction() {
                Id = "MoveToPointerPosition",
                DownAction = (c,m) => { },
                HoldAction = (c,m) => { },
                UpAction = (c,m) => {  }
            },
            new InputAction() {
                Id = "CastAtPointerPosition",
                DownAction = (c,m) =>  c.CastAbilityAtPointerButtonDown(m),
                HoldAction = (c,m) => c.CastAbilityAtPointerButtonHold(m),
                UpAction = (c,m) => c.CastAbilityAtPointerButtonUp()
            },
            new InputAction() {
                Id = "CenterCameraAtPlayerGameBody",
                DownAction = (c,m) => { },
                HoldAction = (c,m) => c.CenterCameraAtPlayerGameBody(),
                UpAction = (c,m) => { }
            },
            new InputAction() {
                Id = "EnemyWaveManager",
                DownAction = (c,m) =>  c.ToggleEnemyWaveManager(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) => { }
            },
            new InputAction() {
                Id = "Inventory",
                DownAction = (c,m) =>  c.ToggleInventory(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) => { }
            },
            new InputAction() {
                Id = "Exit",
                DownAction = (c,m) =>  c.ToggleGamePause(),
                HoldAction = (c,m) => { },
                UpAction = (c,m) => { }
            }
        };
    }
}
