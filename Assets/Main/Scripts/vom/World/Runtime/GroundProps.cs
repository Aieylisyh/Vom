using UnityEngine;
using System.Collections;

public class GroundProps : MonoBehaviour
{
    public bool toSync;
    public float offset = 0;

    public void Sync()
    {
        Vector3 origin = transform.position;
        RaycastHit hitInfo;
        //public static bool Raycast(Vector3 origin, Vector3 direction, [Internal.DefaultValue("Mathf.Infinity")] float maxDistance, [Internal.DefaultValue("DefaultRaycastLayers")] int layerMask, [Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction);
        var res = Physics.Raycast(transform.position, Vector3.down, out hitInfo, 10, 1 << 3);
        if (res)
        {
            transform.position = hitInfo.point + offset * Vector3.up;
        }
    }

    private void Update()
    {
        if (toSync)
        {
            Sync();
            toSync = false;
        }
    }
}
