using System.Collections.Generic;
namespace vom
{
    [System.Serializable]
    public struct AttributeLayerData
    {
        public Dictionary<string, int> atbs;

        public static AttributeLayerData operator +(AttributeLayerData a) => a;

        public static AttributeLayerData operator +(AttributeLayerData a, AttributeLayerData b) => AttributeService.Merge(a, b);

        public void Init()
        {
            atbs = new Dictionary<string, int>();
        }
    }
}