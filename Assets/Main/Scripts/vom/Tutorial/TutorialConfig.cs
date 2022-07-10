using UnityEngine;

namespace vom
{
    [CreateAssetMenu]
    public class TutorialConfig : ScriptableObject
    {
        public MinLevelIndexEnableFunctionsData minLevelIndexEnableFunctionsData;

        public MinRftIndexEnableFunctionsData minRftIndexEnableFunctionsData;

        [System.Serializable]
        public struct MinRftIndexEnableFunctionsData
        {
            public int levelup;
            public int fab;
        }

        [System.Serializable]
        public struct MinLevelIndexEnableFunctionsData
        {
            public int merchant;
            public int fishing;
            public int workshop;
            public int town;
            public int airship;//TODO
            public int map;//TODO
            public int sabTab;

            public int mail;
            public int inv;
            public int rank;//TODO
            public int activity;//TODO

            public int raid;
            public int revive;
            public int playCountLimit;

            public int mapIdle;
            public int boss;//TODO
            public int abysse;//TODO
            public int wiki;//TODO

            public int mission_ml;//TODO
            public int mission_dl;//TODO
        }
    }
}