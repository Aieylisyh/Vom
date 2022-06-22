using UnityEngine;
using com;
using DG.Tweening;

public class HeatDistortBehaviour : MonoBehaviour
{
    public Renderer r;
    public float distortion = 4096;
    public float time;
    float _timer;

    public void Init(float t, float size)
    {
        time = t;
        _timer = t;

        transform.DOScale(size, t).SetEase(Ease.OutQuad);
        gameObject.SetActive(true);
    }

    void Update()
    {
        _timer -= GameTime.deltaTime;
        if (_timer < 0)
        {
            Destroy(gameObject);
            return;
        }

        r.material.SetFloat("_BumpAmt", _timer / time * distortion);
    }
}
