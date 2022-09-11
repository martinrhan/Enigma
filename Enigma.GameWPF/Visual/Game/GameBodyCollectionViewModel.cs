using Enigma.Game;
using Enigma.Game.Extensions;

namespace Enigma.GameWPF.Visual.Game {
    public class GameBodyCollectionViewModel : CollectionViewModel<GameBodyViewModel, GameBody> {
        public GameBodyCollectionView View { get; set; }
        public double ViewWidth => View.ActualWidth;
        public double ViewHeight => View.ActualHeight;
        public void UpdateDataFromModel(GameWorld gameWorld, Camera camera) {
            UpdateDataFromModel_Protected(
                gameWorld.AABBHitTest(camera.AABB),
                (gameBodyViewModel, gameBody) => gameBodyViewModel.UpdateDataFromModel(gameBody, this, camera)
            );
        }
    }
}
