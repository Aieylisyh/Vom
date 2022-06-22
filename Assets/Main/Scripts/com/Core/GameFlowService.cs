using game;
using UnityEngine;
namespace com
{
    public interface IGameFlow
    {
        void OnWindowState(GameFlowService.WindowState state);
        void OnPausedState(GameFlowService.PausedState state);
    }

    public class GameFlowService : MonoBehaviour
    {
        public static GameFlowService instance { get; private set; }

        void Awake()
        {
            instance = this;
            windowState = WindowState.None;
            pausedState = PausedState.Normal;
            gameFlowEvent = GameFlowEvent.None;
        }

        private void Start()
        {
            StartLoading();
        }

        private void StartLoading()
        {
            EnqueueEvent(GameFlowEvent.Loading, false);

            Debug.Log("TODO do some real loading staff");
            EndLoading();
        }

        private void EndLoading()
        {
            TriggerEvent();
            EnqueueEvent(GameFlowEvent.Login, true);
        }

        public enum WindowState
        {
            None,//&initilizing
            Main,
            Gameplay,
        }
        public enum PausedState
        {
            Normal,
            Paused,
            Speed2,
            Speed3,
        }


        public WindowState windowState { get; private set; }
        public PausedState pausedState { get; private set; }

        public void SetWindowState(WindowState state)
        {
            if (windowState == state)
            {
                return;
            }
            switch (windowState)//exit
            {
                case WindowState.None:
                    break;
            }
            switch (state)//enter
            {
                case WindowState.None:
                    break;
            }
            windowState = state;
            //Debug.Log("WindowState -> " + state);
        }

        public void SetPausedState(PausedState state)
        {
            pausedState = state;
            //Debug.Log("PausedState  -> " + state);
            if (state == PausedState.Paused)
            {
                com.GameTime.timeScale = 0;
            }
            else if (state == PausedState.Normal)
            {
                com.GameTime.timeScale = 1;
            }
            else if (state == PausedState.Speed2)
            {
                com.GameTime.timeScale = 2;
            }
        }

        public bool IsPauseEnabled
        {
            get
            {
                if (windowState == WindowState.Gameplay)
                {
                    return true;
                }
                return false;
            }
        }

        //timed flow
        private void Update()
        {
            TriggerEventByTime();
        }

        //public float toCombatTime = 2;
        // public float toPortTime = 2;
        public float onPlayerDeadTime = 2;

        private float _gameFlowEventTimer;

        public GameFlowEvent gameFlowEvent { get; private set; }

        public enum GameFlowEvent
        {
            None,
            Login,
            Loading,
            GoToCombat,
            GoToPort,
            OnPlayerDead,
        }

        public void EnqueueEvent(GameFlowEvent evt, bool instantly = false)
        {
            if (gameFlowEvent == evt)
                return;

            gameFlowEvent = evt;
            if (instantly)
            {
                TriggerEvent();
                return;
            }

            _gameFlowEventTimer = 0;//default, never use timed trigger
            switch (evt)
            {
                case GameFlowEvent.OnPlayerDead:
                    _gameFlowEventTimer = onPlayerDeadTime;
                    break;
            }
        }

        private void TriggerEvent()
        {
            switch (gameFlowEvent)
            {
                case GameFlowEvent.GoToCombat:
                    LevelService.instance.StartLevel();
                    MusicService.instance.PlayCombat();
                    //show level title...
                    break;

                case GameFlowEvent.GoToPort:
                    LevelService.instance.ClearLevel();
                    MusicService.instance.PlayMenu();
                    break;

                case GameFlowEvent.Login:
                    UxService.instance.StartLogin();
                    MusicService.instance.PlayMenu();
                    break;

                case GameFlowEvent.Loading:

                    break;

                case GameFlowEvent.OnPlayerDead:
                    ReviveService.instance.DemandRevive(() => { WindowService.instance.ShowRoundEnd(false); });
                    break;
            }

            _gameFlowEventTimer = 0;
            gameFlowEvent = GameFlowEvent.None;
        }

        private void TriggerEventByTime()
        {
            if (_gameFlowEventTimer > 0)
            {
                _gameFlowEventTimer -= Time.deltaTime;
                if (_gameFlowEventTimer <= 0)
                {
                    TriggerEvent();
                    _gameFlowEventTimer = 0;
                }
            }
        }
    }
}