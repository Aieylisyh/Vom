using UnityEngine;

namespace vom
{
    public class NpcSpawnBehaviour : PropsSpawnBehaviour
    {
        public NpcPrototype npc;

        protected override void Spawn()
        {
            var go = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
            var npcComp = go.GetComponent<NpcBehaviour>();
            npcComp.npc = this.npc;
            npcComp.Rotate(transform.forward);
        }
    }
}