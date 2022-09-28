using Enigma.Game;
using Enigma.GameWPF.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Enigma.GameWPF.Visual.Game {
    public class EquipmentAbilityItemViewModel : ManualNotifyChangedViewModel {
        public EquipmentAbilityItemViewModel() {

        }

        public string ItemName { get; private set; }
        public KeyOrMouseButton? BindedInput { get; private set; }
        public bool IsSlotEmpty { get; private set; }

        public bool IsSelected { get; set; }

        private ContentControl iconView;
        public ContentControl IconView => iconView;

        public void UpdateDataFromModel(Ability model, in KeyOrMouseButton? input) {
            if (model == null) {
                IsSlotEmpty = true;
                ItemName = "SlotEmpty";
            } else {
                IsSlotEmpty = false;
                ItemName = model.Template.Id;
                BindedInput = input;
                AbilityIconView.Set(ref iconView, model.Template.Id);
            }

        }

        public override void NotifyChanged() {
            NotifyPropertyChanged(null);
        }
    }
}
