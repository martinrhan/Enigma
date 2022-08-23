using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public abstract class UpgradeItem {
        public string Id { get; set; }
        public abstract void OnPurchase();
    }
}
