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

        public ContentControl IconView { get; private set; }

        public void UpdateDataFromModel(Ability model, in KeyOrMouseButton? input, bool isSelected) {
            if (model == null) {
                IsEmpty = true;
            } else {
                IsEmpty = false;
                Name = model.Template.Id;
                BindedInput = input;
                IsSelected = isSelected;
                AbilityPhaseStageProgressProportion = model.EffectAssembly.CurrentPhase.CurrentStageIndex == -1 ? 0 : Math.Min(1, model.EffectAssembly.CurrentPhase.CurrentStageElapsedTime / model.EffectAssembly.CurrentPhase.CurrentStage.MaxTime);
                if (IconView == null) {
                    IconView = AbilityIconView.New(model.Template.Id);
                } else {
                    string name = IconView.GetType().Name;
                    if (model.Template.Id != name.Substring(0, name.Length - 8)) {
                        IconView = AbilityIconView.New(model.Template.Id);
                    }
                }
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
