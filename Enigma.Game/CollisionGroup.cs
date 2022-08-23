using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class CollisionGroup {
        internal CollisionGroup() {
        }
        public string Id { get; init; }
        private readonly HashSet<CollisionGroup> collidables = new HashSet<CollisionGroup>();
        public IReadOnlySet<CollisionGroup> Collidables => collidables;
    }
}
