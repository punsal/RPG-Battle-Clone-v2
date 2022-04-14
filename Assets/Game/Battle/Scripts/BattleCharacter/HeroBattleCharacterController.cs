using System.Collections;
using Game.Battle.Scripts.BattleCharacter.Abstract;
using Game.Battle.Scripts.BattleCharacter.Attributes;
using Game.Hero_Selection.Scripts.Character.Model;
using UnityEngine;

namespace Game.Battle.Scripts.BattleCharacter
{
    public class HeroBattleCharacterController : BattleCharacterController
    { 
        public override IEnumerator SurviveBattle()
        {
            var isLevelUp = Model.CharacterModel.CurrentLevel.AddExperience();
            Feedback.Show("EXP: +1", Color.green, 1f, Model.CharacterModel.CharacterName + " EXP");
            yield return new WaitForSeconds(1.5f);
            if (!isLevelUp) yield break;
            var heroCharacterModel = (HeroCharacterModel) Model.CharacterModel;
            var healthIncrease = heroCharacterModel.IncreaseStartingHealth();
            var attackIncrease = heroCharacterModel.IncreaseAttackPower();
            Feedback.Show("LVL: +1", Color.white, 1f, Model.CharacterModel.CharacterName + " LVL");
            yield return new WaitForSeconds(2f);
            Feedback.Show("HP: +" + healthIncrease.ToString("F2"), Color.white, 1f, Model.CharacterModel.CharacterName + " HP");
            yield return new WaitForSeconds(2f);
            Feedback.Show("ATT: +" + attackIncrease.ToString("F2"), Color.white, 1f, Model.CharacterModel.CharacterName + " ATT");
            yield return new WaitForSeconds(2f);
        }

        [TestMethodButton("Add Experience")]
        private void TestAddExperience()
        {
            Model.CharacterModel.CurrentLevel.AddExperience();
        }
    }
}