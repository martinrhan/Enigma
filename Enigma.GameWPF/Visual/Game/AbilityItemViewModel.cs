﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public class AbilityItemViewModel : ManualNotifyChangedViewModel {
        public AbilityItemViewModel() {
        }

        public string ItemName { get; private set; }
        public string Description { get; private set; }
        public bool IsSlotEmpty { get; private set; }

        public void UpdateDataFromModel(AbilityItem model) {
            if (model == null) {
                IsSlotEmpty = true;
                ItemName = "SlotEmpty";
            } else {
                IsSlotEmpty = false;
                ItemName = model.Template.Id;
            }
        }

        public override void NotifyChanged() {
            NotifyPropertyChanged(null);
        }
    }
}
