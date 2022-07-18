using UnityEngine;

public class WaterTileBehaviour : MonoBehaviour
{
    static float freq = 1.0f;
    static float amplitude = 0.06f;
    float _y;
    float _offset;

    void Start()
    {
        _y = transform.position.y;
        // _offset = (transform.position.z + transform.position.x) * 0.5f;
        _offset = transform.position.z * 0.35f;
    }

    void Update()
    {
        var pos = transform.position;
        pos.y = _y + amplitude * Mathf.Sin(Time.time * freq + _offset);
        transform.position = pos;
    }
}
