namespace vom
{
    [System.Serializable]
    public class ItemData
    {
        public ItemData(int pN, string pId)
        {
            n = pN;
            id = pId;
        }

        public int n;
        public string id;
    }
}