using UnityEngine;

namespace vom
{
    public class AttributeService
    {
        public static AttributesConfig GetConfig()
        {
            return ConfigSystem.instance.attributesConfig;
        }

        public static AttributePrototype GetPrototype(string id)
        {
            var cfg = GetConfig();
            foreach (var p in cfg.attributes)
            {
                if (p.id == id)
                    return p;
            }

            return null;
        }

        public static AttributeLayerData Merge(AttributeLayerData a, AttributeLayerData b)
        {
            var res = new AttributeLayerData();
            res.Init();

            foreach (var kv in a.atbs)
            {
                if (res.atbs.ContainsKey(kv.Key))
                {
                    res.atbs[kv.Key] = res.atbs[kv.Key] + kv.Value;
                }
                else
                {
                    res.atbs[kv.Key] = kv.Value;
                }
            }
            foreach (var kv in b.atbs)
            {
                if (res.atbs.ContainsKey(kv.Key))
                {
                    res.atbs[kv.Key] = res.atbs[kv.Key] + kv.Value;
                }
                else
                {
                    res.atbs[kv.Key] = kv.Value;
                }
            }
            return res;
        }

        public static int CherryPickAttributeFromLayer(string targetId, AttributeLayerData layer)
        {
            int add = 0;
            int percentAdd = 0;

            foreach (var kv in layer.atbs)
            {
                var proto = GetPrototype(kv.Key);
                foreach (var m in proto.applyMethods)
                {
                    if (m.tId != targetId)
                        continue;
                    switch (m.method)
                    {
                        case AttributeApplyMethod.Method.Add:
                            add += kv.Value;
                            break;

                        case AttributeApplyMethod.Method.Minus:
                            add -= kv.Value;
                            break;

                        case AttributeApplyMethod.Method.Add_Multiply:
                            percentAdd += kv.Value;
                            break;

                        case AttributeApplyMethod.Method.Minus_Multiply:
                            percentAdd -= kv.Value;
                            break;
                    }
                }
            }

            return Mathf.CeilToInt(add * (100f + percentAdd) / 100f);
        }
    }
}