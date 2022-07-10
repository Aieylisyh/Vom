using UnityEngine;
using com;

namespace vom
{
    public class PhysicsPropsBehaviour : Ticker
    {
        Vector3 _toPlayerDir;
        public Rigidbody rb;

        public bool IsInPlayerView()
        {
            var dX = _toPlayerDir.x;
            var dZ = _toPlayerDir.z;
            //Debug.Log(dX + " " + dZ + " | " + MapSystem.instance.tileNumRight + " " + MapSystem.instance.tileNumForward + " " + MapSystem.instance.tileNumBackward);
            if (Mathf.Abs(dX) < MapSystem.instance.tileNumRight)
            {
                if (-dZ < MapSystem.instance.tileNumForward && dZ < MapSystem.instance.tileNumBackward)
                {
                    return true;
                }
            }

            return false;
        }

        // Update is called once per frame
        protected override void Tick()
        {
            base.Tick();
            if (PlayerBehaviour.instance == null)
                return;

            var playerPos = PlayerBehaviour.instance.transform.position;
            _toPlayerDir = playerPos - transform.position;
            var inView = IsInPlayerView();
            if (inView)
            {
                if (!rb.useGravity)
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.None;
                }
            }
            else
            {
                if (rb.useGravity)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }
}
