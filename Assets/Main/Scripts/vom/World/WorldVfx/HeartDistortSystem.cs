using UnityEngine;
using DG.Tweening;

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
        Destroy(go, 1.5f);
        go.transform.position = pos;
        go.transform.DOScale(size, duration).SetEase(Ease.OutCubic);
        go.SetActive(true);
    }

    public void Create(Transform trans, float size = 25, float duration = 1.25f)
    {
        var go = Instantiate(heartDistort, trans);
        Destroy(go, 1.5f);
        go.transform.localPosition = Vector3.zero;
        go.transform.DOScale(size, duration).SetEase(Ease.OutCubic);
        go.SetActive(true);
    }
}
