using Enigma.Game;
using Enigma.Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class GameWorldDrawingViewModel : ViewModel {
        public GameWorldDrawingViewModel() {

        }
        public void UpdateDataFromModel(GameWorld gameWorld, Camera camera) {
            int i = 0;
            CameraWidth = camera.Width;
            CameraHeight = camera.Height;
            foreach(var gameBody in gameWorld.AABBHitTest(camera.AABB)) {
                if (GameBodyViewModels.Count == i) {
                    GameBodyViewModels.Add(new GameBodyViewModel());
                }
                GameBodyViewModels[i].UpdateDataFromModel(gameBody, camera);
                i++;
            }
            while (GameBodyViewModels.Count > i) {
                GameBodyViewModels.RemoveAt(GameBodyViewModels.Count - 1);
            }
        }
        
        public readonly List<GameBodyViewModel> GameBodyViewModels = new List<GameBodyViewModel>();

        public double CameraWidth { get; private set; }
        public double CameraHeight { get; private set; }

        public void NotifyChanged() {
            NotifyPropertyChanged(null);
        }
    }
}
