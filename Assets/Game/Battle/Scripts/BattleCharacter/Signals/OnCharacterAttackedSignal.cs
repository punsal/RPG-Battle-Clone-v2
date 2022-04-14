using Framework.ServiceLocator.Signals.Interface;

namespace Game.Battle.Scripts.BattleCharacter.Signals
{
    public class OnCharacterAttackedSignal : ISignal<OnCharacterAttackedSignal.CharacterAttacked>
    {
        public delegate void CharacterAttacked();

        private event CharacterAttacked OnCharacterAttacked;

        public void RaiseOnCharacterAttacked()
        {
            OnCharacterAttacked?.Invoke();
        }

        public void AddListener(CharacterAttacked action)
        {
            OnCharacterAttacked += action;
        }

        public void RemoveListener(CharacterAttacked action)
        {
            OnCharacterAttacked -= action;
        }
    }
}