
namespace game
{
    [System.Serializable]
    public class Item
    {
        public Item(int pN, string pId)
        {
            n = pN;
            id = pId;
        }
        public int n;
        public string id;
        //public Date time; //temp item
    }
}
