using Framework.ServiceLocator.Signals.Interface;

namespace Game.Hero_Selection.Scripts.UI.Menu.View.Signals
{
    public class OnHeroViewSelectedSignal : ISignal<OnHeroViewSelectedSignal.HeroViewSelected>
    {
        public delegate void HeroViewSelected(HeroViewItem heroViewItem, bool isSelected);

        private event HeroViewSelected OnHeroViewSelected;

        public void RaiseOnHeroViewSelected(HeroViewItem heroViewItem, bool isSelected)
        {
            OnHeroViewSelected?.Invoke(heroViewItem, isSelected);
        }

        public void AddListener(HeroViewSelected action)
        {
            OnHeroViewSelected += action;
        }

        public void RemoveListener(HeroViewSelected action)
        {
            OnHeroViewSelected -= action;
        }
    }
}