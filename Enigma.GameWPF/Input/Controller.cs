using System;
using System.Reflection;
using System.Windows;
using Enigma.Common.Math;
using Enigma.Game;
using Enigma.GameWPF.Visual;
using Enigma.GameWPF.Visual.Game;

namespace Enigma.GameWPF.Input {
    public class Controller {
        public Controller(Player player, GamePage gamePage) {
            this.player = player;
            this.gamePage = gamePage;
        }
        private readonly Player player;
        private readonly GamePage gamePage;

        private bool leftButtonDown, rightButtonDown, upButtonDown, downButtonDown;
        public void LeftButtonDown() {
            leftButtonDown = true;
            MovementKeyStatusChanged();
        }
        public void LeftButtonUp() {
            leftButtonDown = false;
            MovementKeyStatusChanged();
        }
        public void RightButtonDown() {
            rightButtonDown = true;
            MovementKeyStatusChanged();
        }
        public void RightButtonUp() {
            rightButtonDown = false;
            MovementKeyStatusChanged();
        }
        public void UpButtonDown() {
            upButtonDown = true;
            MovementKeyStatusChanged();
        }
        public void UpButtonUp() {
            upButtonDown = false;
            MovementKeyStatusChanged();
        }
        public void DownButtonDown() {
            downButtonDown = true;
            MovementKeyStatusChanged();
        }
        public void DownButtonUp() {
            downButtonDown = false;
            MovementKeyStatusChanged();
        }
        private void MovementKeyStatusChanged() {
            int xUnitVector = Convert.ToInt32(rightButtonDown) - Convert.ToInt32(leftButtonDown);
            int yUnitVector = Convert.ToInt32(upButtonDown) - Convert.ToInt32(downButtonDown);
            if (xUnitVector == 0 && yUnitVector == 0) {
                gamePage.GameWorldViewModel.UserInputCollection.ToSetMovementActionTuple = new Tuple<GameBodyMovementAction>(null);
            } else {
                double direction = Math.Atan2(yUnitVector, xUnitVector);
                gamePage.GameWorldViewModel.UserInputCollection.ToSetMovementActionTuple = new Tuple<GameBodyMovementAction>(new GameBodyMovementAction.BuiltIn.MoveToDirection(direction));
                //Debug.WriteLine(direction);
            }
        }

        public void AbilityButtonDown(int index, Point mousePosition) {
            if (player.PlayerGameBody.AbilityCollection[index] != null) {
                gamePage.GameWorldViewModel.PlayerGameBodyInfoViewModel.SelectableAbilitiesViewModel.SelectedAbilityIndex = index;
            }
        }
        public void AbilityButtonUp(int index, Point mousePosition) {
        }

        private Vector2 ConvertToGameWorldPosition(in Point point) {
            Camera camera = gamePage.GameWorldViewModel.Camera;
            double gameWorldRelativePositionX = point.X * camera.Width / gamePage.ActualWidth;
            double actualMouseY = gamePage.ActualHeight - point.Y;//the WPF's origin is top left while the gameworld's origin is bottum left.
            double gameWorldRelativePositionY = actualMouseY * camera.Height / gamePage.ActualHeight;
            return new(camera.LowerBound.X + gameWorldRelativePositionX, camera.LowerBound.Y + gameWorldRelativePositionY);
        }
        public void CastAbilityAtPointerButtonDown(Point mousePosition) {
            CastAbilityAtPointerButtonHold(mousePosition);
        }
        public void CastAbilityAtPointerButtonHold(Point mousePosition) {
            int abilityIndex = gamePage.GameWorldViewModel.PlayerGameBodyInfoViewModel.SelectableAbilitiesViewModel.SelectedAbilityIndex;
            if (player.PlayerGameBody.AbilityCollection.Count <= abilityIndex || player.PlayerGameBody.AbilityCollection[abilityIndex] == null) return;
            if (player.PlayerGameBody.AbilityCollection[abilityIndex].AbilityMechanism.CurrentStageIndex == 0) {
                gamePage.GameWorldViewModel.UserInputCollection.ToStartCastingIndexHashSet.Add(abilityIndex);
            }
            Vector2 targetGameWorldPosition = ConvertToGameWorldPosition(mousePosition);
            if (gamePage.GameWorldViewModel.UserInputCollection.ToSetInputDataDictionary.ContainsKey(abilityIndex)) {
                gamePage.GameWorldViewModel.UserInputCollection.ToSetInputDataDictionary[abilityIndex] = new AbilityCastInputData() { TargetPoint = targetGameWorldPosition };
            } else {
                gamePage.GameWorldViewModel.UserInputCollection.ToSetInputDataDictionary.Add(abilityIndex, new AbilityCastInputData() { TargetPoint = targetGameWorldPosition });
            }
        }
        public void CastAbilityAtPointerButtonUp() {
        }
        public void CenterCameraAtPlayerGameBody() {
            gamePage.GameWorldViewModel.Camera.NeedMoveTo = gamePage.GameWorldViewModel.player.PlayerGameBody; // Cannot directly set Camera.Center by PlayerGameBody.Center because the GameWorld may be updating
        }
        public void ToggleEnemyWaveManager() {
            gamePage.GameWorldViewModel.PausePanelsViewModel.SelectEnemyWaveManager();
        }
        public void ToggleInventory() {
            gamePage.GameWorldViewModel.PausePanelsViewModel.SelectInventory();
        }
        public void ToggleGamePause() {
            return;
        }
    }
}
