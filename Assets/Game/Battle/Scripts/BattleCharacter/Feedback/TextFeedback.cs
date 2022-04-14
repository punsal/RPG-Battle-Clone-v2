using System.Collections;
using Framework.ServiceLocator.Systems;
using Game.Battle.Scripts.Systems.Command_System;
using Game.Battle.Scripts.Systems.Command_System.Data;
using Game.Battle.Scripts.Systems.Command_System.Type;
using TMPro;
using UnityEngine;

namespace Game.Battle.Scripts.BattleCharacter.Feedback
{
    [RequireComponent(typeof(TextMeshPro))]
    public class TextFeedback : MonoBehaviour
    {
        private TextMeshPro _feedback;

        private TextMeshPro Feedback => _feedback == null 
            ? _feedback = GetComponent<TextMeshPro>() 
            : _feedback;

        private Coroutine _coroutine;
        
        public void Show(string message, Color messageColor, float duration = 1f ,string ownerSignature = "")
        {
            var commandSystem = SystemServices.GetSystem<CommandSystem>();
            var commandName = string.IsNullOrEmpty(ownerSignature) ? "Feedback" : $"{ownerSignature} Feedback";
            commandSystem.CreateCommand(commandName, duration, new[]
            {
                new CommandActionData()
                {
                    CommandType = CommandActionType.OnStart,
                    CommandAction = () =>
                    {
                        ResetFeedback();
                        SetMessage(message);
                        SetColor(new Color(messageColor.r, messageColor.g, messageColor.b, 0f));
                        
                        if (_coroutine != null)
                        {
                            StopCoroutine(_coroutine);
                            _coroutine = null;
                        }

                        _coroutine = StartCoroutine(Appear(duration));
                    }
                },
                new CommandActionData()
                {
                    CommandType = CommandActionType.OnEnd,
                    CommandAction = Hide
                }
            });
        }

        public void Hide()
        {
            Feedback.text = string.Empty;
            ResetFeedback();
        }

        private IEnumerator Appear(float duration)
        {
            var timer = 0f;
            while (timer <= duration)
            {
                var nextLocalPosition = Vector3.Lerp(Vector3.zero, Vector3.up, timer / (duration * 0.75f));
                var nextLocalScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.05f, timer / (duration * 0.25f));
                var nextColorAlpha = Mathf.Lerp(0f, 1f, timer / (duration * 0.5f));

                Feedback.transform.localPosition = nextLocalPosition;
                Feedback.transform.localScale = nextLocalScale;
                var color = Feedback.color;
                color.a = nextColorAlpha;
                Feedback.color = color;

                timer += Time.deltaTime;

                yield return null;
            }
        }

        private void ResetFeedback()
        {
            var feedbackTransform = transform;
            feedbackTransform.localPosition = Vector3.zero;
            feedbackTransform.localScale = Vector3.zero;
        }

        private void SetMessage(string message)
        {
            Feedback.text = message;
        }

        private void SetColor(Color color)
        {
            Feedback.color = color;
        }
    }
}
