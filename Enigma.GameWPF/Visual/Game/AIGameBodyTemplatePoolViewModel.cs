using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class AIGameBodyTemplatePoolViewModel : ManualNotifyChangedViewModel {
        public AIGameBodyTemplatePoolViewModel() {

        }
        public string SetExpression { get; private set; }

        public void UpdateDataFromModel(AIGameBodyTemplatePool model) {
            SetExpression = model.SetExpression.ToString();
        }

        public override void NotifyChanged() {
            NotifyPropertyChanged(nameof(SetExpression));
        }
    }
}
