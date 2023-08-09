using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UINotify
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _text;

        private bool _stopMessageRecived;

        private void OnDestroy()
        {
            LocalNotify.Instance.UIUpdating -= OnUIUpdating;
        }

        public void Init(string message, NotificationStyle style)
        {
            _text.text = message;
            ChangeColors(style.Text, style.Background, 0);
            LocalNotify.Instance.UIUpdating += OnUIUpdating;
        }

        public void Show(float duration, AnimationType animation, bool destroy)
        {
            duration = Math.Abs(duration);

            if (animation == AnimationType.None)
            {
                ChangeColors(_text.color, _background.color, 1);
                StartCoroutine(Show(duration, destroy));
                return;
            }

            if (animation == AnimationType.Fade)
            {
                StartCoroutine(Fade(duration, destroy));
                return;
            }

            ChangeColors(_text.color, _background.color, 1);
            StartCoroutine(Move(duration, animation, destroy));
        }

        private void OnUIUpdating()
        {
            _stopMessageRecived = true;
        }

        private void ChangeColors(Color textColor, Color backgroundColor, float alpha)
        {
            var tempColor = new Color();
            alpha = Math.Clamp(alpha, 0f, 1f);

            tempColor = textColor;
            tempColor.a = alpha;
            _text.color = tempColor;

            tempColor = backgroundColor;
            tempColor.a = alpha;
            _background.color = tempColor;
        }

        private IEnumerator Show(float duration, bool destroy)
        {
            yield return new WaitForSeconds(duration);
            if (destroy)
                Destroy(gameObject);
        }

        private IEnumerator Move(float duration, AnimationType type, bool destroy)
        {
            yield return null;
            yield return SetPosition(type, 0.2f);
            yield return new WaitForSeconds(duration);
            if (destroy)
            {
                yield return SetTransparency(0, 0.3f);
                Destroy(gameObject);
            }
        }

        private IEnumerator Fade(float duration, bool destroy)
        {
            yield return null;
            yield return SetTransparency(1, 0.3f);
            yield return new WaitForSeconds(duration);
            if (destroy)
            {
                yield return SetTransparency(0, 0.3f);
                Destroy(gameObject);
            }
        }

        private IEnumerator SetTransparency(float targetAlpha, float time)
        {
            var startBackgroundColor = _background.color;
            var targetBackgroundColor = _background.color;
            var startTextColor = _text.color;
            var targetTextColor = _text.color;
            var expiredSeconds = 0f;
            var progress = 0f;

            targetBackgroundColor.a = targetAlpha;
            targetTextColor.a = targetAlpha;

            while (progress < 1)
            {
                _background.color = Color.Lerp(startBackgroundColor, targetBackgroundColor, progress);
                _text.color = Color.Lerp(startTextColor, targetTextColor, progress);

                expiredSeconds += Time.unscaledDeltaTime;
                progress = expiredSeconds / time;
                yield return null;
            }

            yield break;
        }

        private IEnumerator SetPosition(AnimationType type, float duration)
        {
            var targetPosition = transform.position;
            var startPosition = transform.position;
            var expiredSeconds = 0f;
            var progress = 0f;

            switch (type)
            {
                case AnimationType.TopToBot:
                    startPosition.y += ((RectTransform)transform).sizeDelta.y;
                    break;
                case AnimationType.BotToTop:
                    startPosition.y -= ((RectTransform)transform).sizeDelta.y;
                    break;
                case AnimationType.LeftToRight:
                    startPosition.x += ((RectTransform)transform).sizeDelta.x;
                    break;
                case AnimationType.RightToLeft:
                    startPosition.x -= ((RectTransform)transform).sizeDelta.x;
                    break;
                default:
                    break;
            }

            var difference = targetPosition - startPosition;

            while (progress < 1 && _stopMessageRecived == false)
            {
                var newPosition = difference * progress;
                transform.position = startPosition + newPosition;

                expiredSeconds += Time.deltaTime;
                progress = expiredSeconds / duration;

                yield return null;
            }

            yield break;
        }
    }
}