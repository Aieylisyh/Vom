using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class RuntimeLevel
    {
        public string id;
        public List<LevelEvent> events_pending;
        public List<LevelEvent> events_used;
        public LevelPrototype levelProto { get; private set; }
        public LevelItem levelItem { get; private set; }

        public List<Item> totalLoot { get; private set; }//exp is in here
        //public int exp;
        public System.DateTime startTime;
        public int currentWaveIndex;
        private int _passedSeconds;
        public List<string> cabCards { get; private set; }//id
        private int _spareEngineTriggerCount;
        public int reviveCount { get; private set; }
        private bool _levelEnd;
        public int score { get; private set; }
        public int maxScore { get; private set; }

        public RuntimeLevel(string pId)
        {
            id = pId;
            events_pending = new List<LevelEvent>();
            events_used = new List<LevelEvent>();
            totalLoot = new List<Item>();
            score = 0;
            maxScore = 0;
            _passedSeconds = 0;
            _spareEngineTriggerCount = 0;
            _levelEnd = false;

            if (id == "Pedia")
            {
                PediaService.instance.SetupPedia();
                return;
            }

            levelProto = LevelService.instance.GetPrototype(id);
            if (levelProto == null)
                Debug.LogError("LoadLevel proto null");

            levelItem = LevelService.instance.GetLevelItem(id);

            startTime = System.DateTime.Now;

            events_pending.AddRange(ConfigService.instance.levelConfig.defaultLevelEvents);

            currentWaveIndex = -1;
        }

        private static int CompareLevelEventByTime(LevelEvent x, LevelEvent y)
        {
            if (x.time > y.time)
            {
                return 1;
            }
            if (x.time < y.time)
            {
                return -1;
            }

            return 0;
        }

        public void OnRevive()
        {
            reviveCount += 1;
            _passedSeconds = 0;
            LoadWave(true);
        }

        public void CheckNextWave()
        {
            //Debug.Log("---CheckNextWave---");
            //CombatService.instance.LogUnits();
            if (_levelEnd)
                return;

            if (LevelService.instance.IsPediaLevel())
                return;

            if (CombatService.instance.playerShip.death.isDead)
                return;

            if (LevelTutorialService.instance.HasPendingTuto())
                return;

            //check next wave, check win
            if (CombatService.instance.IsLevelCleared())
            {
                //Debug.Log("wave Cleared");
                if (currentWaveIndex >= levelProto.waveCount - 1)
                {
                    WindowService.instance.ShowRoundEnd(true);
                    _levelEnd = true;
                }
                else
                {
                    currentWaveIndex += 1;
                    LoadWave(false);
                    MissionService.instance.PushDl("wave", 1, true);
                }
            }
        }

        public void AddSpareEngineTriggerCount()
        {
            _spareEngineTriggerCount++;
        }

        public int GetSpareEngineTriggerCount()
        {
            return _spareEngineTriggerCount;
        }

        public void LoadWave(bool isRevive)
        {
            //Debug.LogWarning("Load wave " + currentWaveIndex);
            LevelHudBehaviour.instance.UpdateWave(currentWaveIndex + 1, levelProto.waveCount, currentWaveIndex != 0);

            var evts = levelProto.waves[currentWaveIndex].GetEvents();
            if (currentWaveIndex == levelProto.waveCount - 1)
            {
                var evt = new LevelEvent();
                evt.evt = "final";
                evt.time = 3f;
                evts.Add(evt);
            }

            var scoreDelta = GetWaveScore(evts);
            //Debug.LogWarning("maxScore " + maxScore + " , score to add " + scoreDelta);

            maxScore += scoreDelta;

            if (!isRevive)
            {

                bool toShowCab = false;
                if (currentWaveIndex > 0)
                {
                    var disableCab = levelProto.waves[currentWaveIndex - 1].disableCab;
                    if (!disableCab)
                    {
                        toShowCab = true;
                    }
                }
                else
                {
                    bool hasItem = UxService.instance.GetItemAmount("ExtraCab") > 0;
                    if (hasItem)
                    {
                        Debug.Log("show item cab");
                        toShowCab = true;
                    }
                }

                if (toShowCab)
                {
                    var evt = new LevelEvent();
                    evt.evt = "cab";
                    evt.time = 0.7f;
                    evts.Add(evt);
                }
            }

            foreach (var e in evts)
            {
                var pEvent = e;
                pEvent.time += _passedSeconds;
                events_pending.Add(pEvent);
            }

            Sort();
        }

        public int GetWaveScore(List<LevelEvent> events)
        {
            int res = 0;
            foreach (var e in events)
            {
                if (e.evt == "ene")
                {
                    string enemyId = e.stringParam[0];
                    var eneProto = EnemyService.instance.GetPrototype(enemyId);
                    res += eneProto.dropData.score;
                    //Debug.Log(eneProto.title + ":" + eneProto.dropData.score);
                }
            }

            return res;
        }

        public void TriggerLevelEvent(float timePassed)
        {
            if (events_pending.Count <= 0)
            {
                var passedSecond = Mathf.FloorToInt(timePassed);
                if (_passedSeconds != passedSecond)
                {
                    //Debug.Log("TriggerLevelEvent "+ _passedSeconds);
                    CombatService.instance.CleanDeadUnits();
                    _passedSeconds = passedSecond;
                    CheckNextWave();
                }

                return;
            }

            var e = events_pending[0];
            if (e.time <= timePassed)
            {
                LevelService.instance.DispatchLevelEvent(e);
                events_used.Add(e);
                events_pending.RemoveAt(0);
            }
        }

        public void Sort()
        {
            events_pending.Sort(CompareLevelEventByTime);
        }

        public void AddLevelScore(int delta)
        {
            //Debug.Log("AddLevelScore " + delta);
            score += delta;
        }

        public void Log()
        {
            Debug.Log("LoadLevel " + levelProto.id + " " + levelProto.desc);
            foreach (var e in events_pending)
            {
                Debug.Log(e.time + " " + e.evt);
            }
        }

        public int GetEnemyLevel()
        {
            var res = levelProto.enemyLevel + currentWaveIndex * levelProto.waveEnemyLevelAdd;
            for (int i = 0; i <= currentWaveIndex; i++)
            {
                var wave = levelProto.waves[i];
                var delta = wave.enemyLevelExtraOffset;
                res += delta;
            }

            return res;
        }
    }
}