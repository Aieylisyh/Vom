using UnityEngine;
using com;
namespace game
{
    public class Projectile : Unit
    {
        public bool hasBubble;
        public Transform bubbleParent;
        private BasicEffect _bubble;
        public string bubbleId = "bubble";

        public bool hasFire;
        public Transform fireParent;
        private BasicEffect _fire;
        public string fireId = "fire1";

        protected override void Start()
        {
            base.Start();

            SetBubble();
            SetFire();
        }

        public void SetBubble()
        {
            if (hasBubble)
            {
                var go = PoolingService.instance.GetInstance(bubbleId);
                _bubble = go.GetComponent<BasicEffect>();

                _bubble.transform.SetParent(bubbleParent);
                _bubble.transform.localPosition = Vector3.zero;
                _bubble.transform.localRotation = Quaternion.identity;
                _bubble.enabled = false;
            }
        }

        public void SetFire()
        {
            if (hasFire)
            {
                var go = PoolingService.instance.GetInstance(fireId);
                _fire = go.GetComponent<BasicEffect>();

                _fire.transform.SetParent(fireParent);
                _fire.transform.localPosition = Vector3.zero;
                _fire.transform.localRotation = Quaternion.identity;
                _fire.enabled = false;
            }
        }

        public override void Recycle()
        {
            DetachBubble();
            DetachFire();
            PoolingService.instance.Recycle(this);
        }

        public void DetachBubble()
        {
            if (_bubble != null)
            {
                _bubble.transform.SetParent(this.transform.parent);
                _bubble.enabled = true;
                _bubble = null;
            }
        }
        public void DetachFire()
        {
            if (_fire != null)
            {
                _fire.transform.SetParent(this.transform.parent);
                _fire.enabled = true;
                _fire = null;
            }
        }
    }
}
