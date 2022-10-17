using Enigma.Game;
using Enigma.GameWPF.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Enigma.GameWPF.Visual.Game {
    public class AbilityViewModel : ManualNotifyChangedViewModel {
        public AbilityViewModel() {
        }
        public string Name { get; private set; }
        public KeyOrMouseButton? BindedInput { get; private set; }
        public bool IsSelected { get; private set; }

        public double AbilityPhaseStageProgressProportion { get; private set; }

        private ContentControl iconView;
        public ContentControl IconView => iconView;

        public void UpdateDataFromModel(Ability model, in KeyOrMouseButton? input, bool isSelected) {
            if (model == null) {
                IsEmpty = true;
            } else {
                IsEmpty = false;
                Name = model.Template.Id;
                BindedInput = input;
                IsSelected = isSelected;
                AbilityPhaseStageProgressProportion = 0;
                AbilityIconView.Set(ref iconView, model.Template.Id);
            }
        }

        public override void NotifyChanged() {
            NotifyPropertyChanged(nameof(IsEmpty));
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(BindedInput));
            NotifyPropertyChanged(nameof(IsSelected));
            NotifyPropertyChanged(nameof(AbilityPhaseStageProgressProportion));
            NotifyPropertyChanged(nameof(IconView));
        }
    }
}
