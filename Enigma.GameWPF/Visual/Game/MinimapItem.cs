using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class MinimapItem {
        public MinimapItem(GameBody gameBody) {
            GameBody = gameBody;
        }
        public GameBody GameBody { get; }
    }
}
