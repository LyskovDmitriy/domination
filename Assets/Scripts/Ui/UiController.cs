using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;
using Utils.Ui;


namespace Domination.Ui
{
    public static class UiController
    {
        private static readonly Type[] MessagesToUpdateUi = new Type[]
        {
            typeof(PlayerSettlmentChangedMessage),
            typeof(PlayerCoinsCountUpdateMessage)
        };

        private static readonly Dictionary<ScreenType, string> ScreenToPath = new Dictionary<ScreenType, string>()
        {
            { ScreenType.SettlmentViewScreen, "Ui/SettlmentViewScreen"},
            { ScreenType.ChooseBuildingScreen, "Ui/ChooseBuildingScreen"},
            { ScreenType.LevelUi, "Ui/LevelUi"},
            { ScreenType.MarchScreen, "Ui/MarchScreen"},
        };


        private static Dictionary<ScreenType, UiScreen> createdScreens = new Dictionary<ScreenType, UiScreen>();
        private static List<UiScreen> shownScreens = new List<UiScreen>();


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            foreach (var messageType in MessagesToUpdateUi)
            {
                EventsAggregator.Subscribe(messageType, SendUpdateUiMessage);
            }

            EventsAggregator.Subscribe(typeof(ShowUiMessage), ShowUi);
            EventsAggregator.Subscribe(typeof(HideUiMessage), HideUi);
        }


        public static void SendUpdateUiMessage(IMessage _) => EventsAggregator.TriggerEvent(new UpdateUiMessage());

        public static void ShowUi(IMessage message)
        {
            ShowUiMessage showUiMessage = (ShowUiMessage)message;
            ScreenType type = showUiMessage.ScreenType;

            if (!createdScreens.TryGetValue(type, out UiScreen screen))
            {
                UiScreen prefab = Resources.Load<UiScreen>(ScreenToPath[type]);
                screen = GameObject.Instantiate(prefab);

                createdScreens.Add(type, screen);
            }

            if (showUiMessage.OnInit == null)
            {
                screen.Show();
            }
            else
            {
                showUiMessage.OnInit(screen);
            }

            screen.OnHidden += OnScreenHidden;
            shownScreens.Add(screen);
        }

        public static void HideUi(IMessage message)
        {
            HideUiMessage hideUiMessage = (HideUiMessage)message;
            ScreenType type = hideUiMessage.ScreenType;

            UiScreen screen = shownScreens.Find((s) => s.Type == type);
            screen.Hide();
        }

        private static void OnScreenHidden(UiUnit screen)
        {
            shownScreens.Remove(screen as UiScreen);
            screen.OnHidden -= OnScreenHidden;
        }
    }
}
