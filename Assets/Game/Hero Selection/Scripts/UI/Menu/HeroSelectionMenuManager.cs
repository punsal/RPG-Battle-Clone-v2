using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.ServiceLocator.Signals;
using Game.Hero_Selection.Scripts.UI.Menu.View;
using Game.Hero_Selection.Scripts.UI.Menu.View.Signals;
using UI_System.Scripts.Menu;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.UI.Menu
{
    public class HeroSelectionMenuManager : MenuManager
    {
        private Queue<HeroViewItem> _selectedHeroViewItems;

        private HeroSelectionMenu _heroSelectionMenu;

        private Coroutine _coroutine;
        
        public HeroSelectionMenu HeroSelectionMenu
        {
            get
            {
                if (_heroSelectionMenu != null) return _heroSelectionMenu;
                var tempHeroSelectionMenu = InstantiatedMenus
                    .FirstOrDefault(menuBase => menuBase.GetType() == typeof(HeroSelectionMenu));
                if (tempHeroSelectionMenu == null)
                {
                    throw new Exception("HeroSelectionMenu is not available in HeroSelectionMenuManager");
                }

                _heroSelectionMenu = (HeroSelectionMenu) tempHeroSelectionMenu;
                return _heroSelectionMenu;
            }
        }
        
        protected override void OnInitialized()
        {
            _selectedHeroViewItems = new Queue<HeroViewItem>();
            _coroutine = StartCoroutine(BattleButtonActivationHandler());
            SignalServices.GetEvent<OnHeroViewSelectedSignal>().AddListener(OnHeroViewSelectedHandler);
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            SignalServices.GetEvent<OnHeroViewSelectedSignal>().RemoveListener(OnHeroViewSelectedHandler);
        }

        private IEnumerator BattleButtonActivationHandler()
        {
            while (true)
            {
                try
                {
                    HeroSelectionMenu.SetBattleButtonActive(_selectedHeroViewItems.Count >= 3);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                    Debug.Log("BattleButtonActivationHandler delayed.");
                }

                yield return null;
            }
        }

        private void OnHeroViewSelectedHandler(HeroViewItem heroViewItem, bool isSelected)
        {
            if (!isSelected)
            {
                var tempQueue = new Queue<HeroViewItem>();
                var itemsCount = _selectedHeroViewItems.Count;
                for (var i = 0; i < itemsCount; i++)
                {
                    var tempHeroView = _selectedHeroViewItems.Dequeue();
                    if (tempHeroView == heroViewItem)
                    {
                        continue;
                    }
                    tempQueue.Enqueue(tempHeroView);
                }
                _selectedHeroViewItems.Clear();
                _selectedHeroViewItems = tempQueue;
            }
            else
            {
                _selectedHeroViewItems.Enqueue(heroViewItem);
            }

            if (_selectedHeroViewItems.Count <= 3) return;
            var viewItem = _selectedHeroViewItems.Dequeue();
            viewItem.Select(false);
        }
    }
}
