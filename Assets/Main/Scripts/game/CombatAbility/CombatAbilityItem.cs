
namespace game
{
    [System.Serializable]
    public class CombatAbilityItem
    {
        public string id;
        public CombatAbilitySaveData saveData;

        public CombatAbilityItem(string pId)
        {
            id = pId;
            saveData = new CombatAbilitySaveData();
        }
    }

    [System.Serializable]
    public class CombatAbilitySaveData
    {
        public int count;
        public CombatAbilitySaveData()
        {
            count = 1;
        }
    }
}