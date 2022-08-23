using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public abstract class Buff {
        public void Start() {

        }
        public void Update() {

        }
    }

    public class GameBodyModifer {

    }

    public abstract class Trigger {
        public abstract bool Predicate(GameBody gameBody);

    }
}
