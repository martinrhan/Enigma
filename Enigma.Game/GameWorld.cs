using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.PhysicsEngine;
using Enigma.Spacial;

namespace Enigma.Game {
    public class GameWorld {
        public static ValueTuple<GameWorld, Player> New() {
            GameWorld gameWorld = new GameWorld();
            return (gameWorld, gameWorld.AddPlayer());
        }
        private GameWorld() {
            OperationManager = new GameWorldOperationManager(this);
        }

        public double Width => physicsWorld.Width;
        public double Height => physicsWorld.Height;

        internal readonly PhysicsWorld<GameBody> physicsWorld = new PhysicsWorld<GameBody>(new Quadtree<GameBody>(5, 4000, 4000));
        internal readonly List<GameBody> playerGameBodyList = new List<GameBody>();
        public IEnumerable<GameBody> GameBodies => physicsWorld;
        public IReadOnlyList<GameBody> PlayerGameBodyList => playerGameBodyList;

        internal readonly GameWorldOperationManager OperationManager;

        public EnemyWaveManager EnemyWaveManager { get; } = new EnemyWaveManager();

        private Task updateTask = Task.CompletedTask;
        public Task BeginUpdate(double deltaTime, IReadOnlyList<PlayerBehaviourUpdateData> playerBehaviourUpdateDataList) {
            if (playerBehaviourUpdateDataList.Count < playerGameBodyList.Count)
                throw new ArgumentException("There are" + playerGameBodyList.Count + "Players in this GameWorld but only" + playerBehaviourUpdateDataList.Count + "PlayerBehaviourUpdateDatas are input.");
            updateTask = Task.Run(() => {
                for (int i = 0; i < playerGameBodyList.Count; i++) {
                    (playerGameBodyList[i].Behaviour as PlayerGameBodyBehaviour).UpdateData = playerBehaviourUpdateDataList[i];
                }
                EnemyWaveManager.CurrentEnemyWave?.Update(this, deltaTime);
                physicsWorld.ParallelForEach(body => {
                    body.Update_Internal(deltaTime);
                });
                OperationManager.ApplyAll(deltaTime);
                physicsWorld.Update((gameBodyA, gameBodyB) => true);
            });
            return updateTask;
        }

        private Player AddPlayer() {
            PlayerGameBodyBehaviour PlayerGameBodyBehaviour = new PlayerGameBodyBehaviour();
            AbilityCollection abilityCollection = AbilityCollection.New(new AbilityCollectionConstructorElement[] { });
            GameBody playerGameBody = new GameBody(abilityCollection, PlayerGameBodyBehaviour, new Vector2(Width / 2, Height / 2), new GameBody.PropertyBuffer() {
                [GameBody.Property.Radius] = 20,
                [GameBody.Property.Speed] = 200
            });
            playerGameBodyList.Add(playerGameBody);
            AddGameBody(playerGameBody);
            return new Player(PlayerGameBodyBehaviour, playerGameBody);
        }

        internal void AddGameBody(GameBody gameBody) {
            gameBody.GameWorld = this;
            physicsWorld.Add(gameBody);
        }
        internal bool RemoveGameBody(GameBody gameBody) {
            gameBody.GameWorld = null;
            return physicsWorld.Remove(gameBody);
        }
    }
}