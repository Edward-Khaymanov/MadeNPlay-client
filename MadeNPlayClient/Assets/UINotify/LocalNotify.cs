using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UINotify
{
    [RequireComponent(typeof(Canvas))]
    public class LocalNotify : MonoBehaviour
    {
        [SerializeField] private Notification _notificationTemplate;
        [SerializeField] private RectTransform _rightTopContainer;
        [SerializeField] private RectTransform _rightBotContainer;
        [SerializeField] private RectTransform _leftTopContainer;
        [SerializeField] private RectTransform _leftBotContainer;
        [SerializeField] private NotificationStyle _defaultStyle;
        [SerializeField] private NotificationStyle _warningStyle;
        [SerializeField] private NotificationStyle _errorStyle;

        private Dictionary<NotificationPosition, RectTransform> _positionContainers;
        private Dictionary<NotificationStyleType, NotificationStyle> _defaultStyles;

        public static LocalNotify Instance { get; private set; }

        public event Action UIUpdating;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _positionContainers = new Dictionary<NotificationPosition, RectTransform>()
            {
                { NotificationPosition.RightTop, _rightTopContainer },
                { NotificationPosition.RightBot, _rightBotContainer },
                { NotificationPosition.LeftTop, _leftTopContainer },
                { NotificationPosition.LeftBot, _leftBotContainer }
            };

            _defaultStyles = new Dictionary<NotificationStyleType, NotificationStyle>()
            {
                { NotificationStyleType.Default, _defaultStyle },
                { NotificationStyleType.Warning, _warningStyle },
                { NotificationStyleType.Error, _errorStyle }
            };

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public static void Show(
            string message,
            float visibilityDuration,
            AnimationType animation = default,
            NotificationPosition position = default,
            bool destroy = true)
        {
            Instance.ShowInternal(message, visibilityDuration, Instance._defaultStyles[default], animation, position, destroy);
        }

        public static void Show(
            string message,
            float visibilityDuration,
            NotificationStyleType type,
            AnimationType animation = default,
            NotificationPosition position = default,
            bool destroy = true)
        {
            Instance.ShowInternal(message, visibilityDuration, Instance._defaultStyles[type], animation, position, destroy);
        }

        public static void Show(
            string message,
            float visibilityDuration,
            NotificationStyle style,
            AnimationType animation = default,
            NotificationPosition position = default,
            bool destroy = true)
        {
            Instance.ShowInternal(message, visibilityDuration, style, animation, position, destroy);
        }

        private void ShowInternal(
            string message,
            float visibilityDuration,
            NotificationStyle style,
            AnimationType animation,
            NotificationPosition position,
            bool destroy)
        {
            var notification = CreateNotification(position);
            UIUpdating?.Invoke();
            notification.Init(message, style);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_positionContainers[position]);
            notification.Show(visibilityDuration, animation, destroy);
        }

        private Notification CreateNotification(NotificationPosition position)
        {
            var notification = Instantiate(_notificationTemplate, _positionContainers[position]);
            notification.transform.SetAsFirstSibling();
            return notification;
        }
    }
}