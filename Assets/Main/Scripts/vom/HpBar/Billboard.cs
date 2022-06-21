using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    public Transform cameraTrans;

    // Use this for initialization
    void Start()
    {
        if (cameraTrans == null)
            cameraTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = cameraTrans.forward;
    }
}
