using com;
using UnityEngine;

namespace game
{
    public class LevelTutorialService : MonoBehaviour
    {
        public static LevelTutorialService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public enum TutoType
        {
            None,
            Move,
            Bomb,
            TorTrace,
            TorDir,
        }

        private bool _movedLeft;
        private bool _movedRight;
        private int _bombCount;
        private bool _torTraceUsed;
        private bool _torDirUsed;

        private TutoType _currentTuto;

        public void SetTuto(TutoType t)
        {
            Flush();
            _currentTuto = t;
        }

        void Flush()
        {
            _movedLeft = false;
            _movedRight = false;
            _bombCount = 0;
            _torTraceUsed = false;
            _torDirUsed = false;
        }

        public bool HasPendingTuto()
        {
            return _currentTuto != TutoType.None;
        }

        public void ProgressTuto_MoveLeft()
        {
            _movedLeft = true;
        }

        public void ProgressTuto_MoveRight()
        {
            _movedRight = true;
        }

        public void ProgressTuto_Bomb()
        {
            _bombCount++;
        }

        public void ProgressTuto_TorTrace()
        {
            _torTraceUsed = true;
        }

        public void ProgressTuto_TorDir()
        {
            _torDirUsed = true;
        }

        public bool TryEndTuto()
        {
            bool res = false;
            switch (_currentTuto)
            {
                case TutoType.None:
                    res = true;
                    break;
                case TutoType.Move:
                    res = _movedLeft && _movedRight;
                    break;
                case TutoType.Bomb:
                    res = _bombCount > 2;
                    break;
                case TutoType.TorTrace:
                    res = _torTraceUsed;
                    break;
                case TutoType.TorDir:
                    res = _torDirUsed;
                    break;
            }

            if (res)
                SetTuto(TutoType.None);

            return res;
        }
    }
}