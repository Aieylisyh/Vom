using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class HeroConfig : ScriptableObject
    {
        List<HeroPrototype> heroes;
    }
}