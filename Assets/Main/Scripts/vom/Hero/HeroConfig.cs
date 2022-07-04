using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class HeroConfig : ScriptableObject
    {
        public List<HeroPrototype> heroes;

        public HeroPrototype defaultHero;

    }
}