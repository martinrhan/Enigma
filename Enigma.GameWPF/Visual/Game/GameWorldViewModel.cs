using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Enigma.Game;
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Game {
    public class GameWorldViewModel : ViewModel, IDisposable {
        public GameWorldViewModel(GameWorld gameWorld, Player player, InputBindingManager inputBindingManager) {
            this.gameWorld = gameWorld;
            this.player = player;
            this.inputBindingManager = inputBindingManager;
            Camera = new Camera(gameWorld);
            Camera.Center = player.PlayerGameBody.Center;
            eventHandler = new EventHandler(PerFrameCallback);
            CompositionTarget.Rendering += eventHandler;
        }
        private readonly GameWorld gameWorld;
        public Player player { get; }
        private readonly InputBindingManager inputBindingManager;
        public Camera Camera { get; }

        public GameWorldDrawingViewModel GameWorldDrawingViewModel { get; } = new GameWorldDrawingViewModel();
        public MinimapViewModel MinimapViewModel { get; } = new MinimapViewModel();
        public PausePanelsViewModel PausePanelsViewModel { get; } = new PausePanelsViewModel();
        public PlayerGameBodyInfoViewModel PlayerGameBodyInfoViewModel { get; } = new PlayerGameBodyInfoViewModel();
        public FPSViewModel FPSViewModel { get; } = new FPSViewModel();

        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly EventHandler eventHandler;
        private Task gameUpdateTask = Task.CompletedTask;
        private void PerFrameCallback(object sender, EventArgs e) {
            if (PausePanelsViewModel.IsPaused) {
                stopwatch.Stop();
                UserInputCollection.Clear();
                return;
            } else {
                stopwatch.Start();
            }
            if (gameUpdateTask.IsCompleted) {//Check if the backgroud update has completed
                FPSViewModel.PlusOneFrameCount();
                UpdateDataFromModel();//Copy the data but not update UI
                double deltaTime = stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();
                gameUpdateTask = gameWorld.BeginUpdate(deltaTime, GetPlayerBehaviourUpdateData());//Let backgroud game engine calculate next frame of the game.
                //gameUpdateTask = Task.CompletedTask;
                NotifyChanged();//Update UI, runing concurrently with backgroud game engine thread.
            }
        }
        public readonly Input.UserInputCollection UserInputCollection = new();
        private PlayerBehaviourUpdateData[] GetPlayerBehaviourUpdateData() {
            var result = new PlayerBehaviourUpdateData[] {
                new PlayerBehaviourUpdateData() {
                    FocusedAbilityIndex = PlayerGameBodyInfoViewModel.SelectableAbilitiesViewModel.SelectedAbilityIndex,
                    ToStartCastingAbilityIndexes = UserInputCollection.ToStartCastingIndexHashSet.ToArray(),
                    ToCancelCastingAbilityIndexes = UserInputCollection.ToCancelCastingIndexHashSet.ToArray(),
                    ToSetInputDatas = UserInputCollection.ToSetInputDataDictionary.Select(p => (p.Key, p.Value)).ToArray(),
                    ToSetMovementAction = UserInputCollection.ToSetMovementActionTuple,
                }
            };
            UserInputCollection.Clear();
            return result;
        }
        private void UpdateDataFromModel() {
            if (Camera.NeedMoveTo != null) {
                Camera.Center = Camera.NeedMoveTo.Center;
                Camera.NeedMoveTo = null;
            }
            GameWorldDrawingViewModel.UpdateDataFromModel(gameWorld, Camera);
            MinimapViewModel.UpdateDataFromModel(gameWorld, Camera);
            PausePanelsViewModel.UpdateDataFromModel(gameWorld.EnemyWaveManager, player, inputBindingManager);
            PlayerGameBodyInfoViewModel.UpdateDataFromModel(player, inputBindingManager);
        }

        private void NotifyChanged() {
            GameWorldDrawingViewModel.NotifyChanged();
            MinimapViewModel.NotifyChanged();
            PausePanelsViewModel.NotifyChanged();
            PlayerGameBodyInfoViewModel.NotifyChanged();
        }

        public void Dispose() {
            CompositionTarget.Rendering -= eventHandler;
        }
    }
}