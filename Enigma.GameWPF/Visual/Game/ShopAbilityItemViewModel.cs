using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class ShopAbilityItemViewModel : ManualNotifyChangedViewModel {
        public ShopAbilityItemViewModel() {

        }

        public string ItemName { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public bool IsSold { get; private set; }

        public void UpdateDataFromModel(AbilityItem model) {
            if (model == null) {
                IsSold = true;
            } else {
                IsSold = false;
                ItemName = model.Template.Id;
                Price = model.Price;
            }
        }

        public override void NotifyChanged() {
            NotifyPropertyChanged(null);
        }
    }
}
