using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class CombatAbilityService : MonoBehaviour
    {
        public static CombatAbilityService instance;

        public List<string> totalPool { get; private set; }
        public List<string> selectedPool { get; private set; }
        public List<string> toSelectPool
        {
            get
            {
                List<string> res = new List<string>();
                foreach (var i in totalPool)
                {
                    if (selectedPool.IndexOf(i) < 0)
                    {
                        res.Add(i);
                    }
                }
                return res;
            }
        }

        public List<string> toSelectPoolNonBackup
        {
            get
            {
                List<string> res = new List<string>();
                foreach (var i in totalPool)
                {
                    if (selectedPool.IndexOf(i) < 0)
                    {
                        var proto = GetPrototype(i);
                        if (!proto.isBackup)
                        {
                            res.Add(i);
                        }
                    }
                }
                return res;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private List<CombatAbilityPrototype> GetPrototypes()
        {
            return ConfigService.instance.combatAbilityConfig.cabs;
        }

        public void RefreshAndReset()
        {
            RefreshTotalPool();
            ResetSelectedPool();
        }

        public void RefreshTotalPool()
        {
            //Debug.Log("RefreshTotalPool");
            var protos = GetPrototypes();
            var res = new List<string>();
            //add default
            foreach (var proto in protos)
            {
                if (proto.unlockedByDefault)
                {
                    var levelIndex = LevelService.instance.GetNextCampaignLevelIndex();
                    if (levelIndex >= proto.minAvailableLevel)
                    {
                        res.Add(proto.id);
                    }
                }
            }

            //add talent unlocked
            var talents = TalentService.instance.GetItems();
            foreach (var talent in talents)
            {
                var talentProto = TalentService.instance.GetPrototype(talent.id);
                if (!string.IsNullOrEmpty(talentProto.stringValue))
                {
                    //is talent with cab
                    if (talent.saveData.level > 0)
                    {
                        //this talent is >0 level
                        //Debug.Log(talentProto.stringValue);
                        res.Add(talentProto.stringValue);
                    }
                }
            }

            //Debug.Log(" add level unlocked combat ability");
            var cabItems = ConfigService.instance.combatAbilityConfig.cabItems;
            foreach (var cabItem in cabItems)
            {
                if (UxService.instance.GetItemAmount(cabItem.id) > 0)
                {
                    res.Add(cabItem.itemOutPut.id);
                }
            }

            totalPool = res;
        }

        private void ResetSelectedPool()
        {
            selectedPool = new List<string>();
        }

        public void AddToSelectedPool(string e)
        {
            if (selectedPool == null)
            {
                ResetSelectedPool();
            }

            if (selectedPool.IndexOf(e) < 0)
            {
                selectedPool.Add(e);
            }
        }

        public void ValidateSelected(string v)
        {
            //Debug.LogWarning("!--ValidateSelected " + v);
            CombatService.instance.RefreshPlayerAttri(PlayerAttributes.PlayerAttriLayer.Combat);
        }

        public List<string> Get3ToSelect(int insertMax = 2)
        {
            var res = new List<string>();
            var tpPool = toSelectPoolNonBackup;
            res = ListUtil.GetRandomElements(tpPool, 3);
            if (res != null)
                return res;

            tpPool = toSelectPool;
            res = ListUtil.GetRandomElements(tpPool, 3);
            if (res == null)
            {
                res = new List<string>();
                res.AddRange(tpPool);
            }

            var s = ConfigService.instance.combatAbilityConfig.backupCab.id;
            if (res.Count < 3&& insertMax > 0)
            {
                res.Add(s);
                insertMax--;
                if (res.Count < 3 && insertMax > 0)
                {
                    res.Add(s);
                    insertMax--;
                    if (res.Count < 3 && insertMax > 0)
                    {
                        res.Add(s);
                        insertMax--;
                    }
                }
            }

            if (res.Count != 3)
                return null;

            return res;
        }

        public CombatAbilityPrototype GetPrototype(string id)
        {
            foreach (var i in GetPrototypes())
            {
                if (i.id == id)
                {
                    return i;
                }
            }

            return null;
        }
    }
}
