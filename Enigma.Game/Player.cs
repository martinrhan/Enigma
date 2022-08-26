﻿using Enigma.Common.Math;
using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class Player {
        internal Player(PlayerGameBodyBehaviour playerGameBodyBehaviour, GameBody playerGameBody) {
            PlayerGameBodyBehaviour = playerGameBodyBehaviour;
            PlayerGameBody = playerGameBody;
            Inventory =  new Inventory(PlayerGameBody.AbilityCollection);
        }
        public PlayerGameBodyBehaviour PlayerGameBodyBehaviour { get; } = new PlayerGameBodyBehaviour();
        public Inventory Inventory { get; } 
        public GameBody PlayerGameBody { get; }
    }
}