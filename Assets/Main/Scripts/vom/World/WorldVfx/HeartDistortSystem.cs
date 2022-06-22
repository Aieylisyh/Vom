using UnityEngine;


public class HeartDistortSystem : MonoBehaviour
{
    public static HeartDistortSystem instance { get; private set; }

    public GameObject heartDistort;

    private void Awake()
    {
        instance = this;
    }

    public void Create(Vector3 pos, float size = 25, float duration = 1.25f)
    {
        var go = Instantiate(heartDistort, this.transform);
        go.transform.position = pos;
        go.GetComponent<HeatDistortBehaviour>().Init(duration, size);
    }

    public void Create(Transform trans, float size = 25, float duration = 1.25f)
    {
        var go = Instantiate(heartDistort, trans);
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<HeatDistortBehaviour>().Init(duration, size);
    }
}
