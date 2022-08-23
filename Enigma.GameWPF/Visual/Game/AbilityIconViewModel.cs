using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public abstract class AbilityIconViewModel : ManualNotifyChangedViewModel {
        public abstract void UpdateDataFromModel(Ability model);
    }
}
