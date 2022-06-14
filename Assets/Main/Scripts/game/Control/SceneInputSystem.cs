using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SceneInputSystem : MonoBehaviour
{
    public static SceneInputSystem instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public void InputPanelDown(PointerEventData eventData)
    {
        Debug.Log("InputPanelDown");
    }

    public void InputPanelRelease(PointerEventData eventData)
    {
        Debug.Log("InputPanelRelease");
    }

    public void InputPanelClick(PointerEventData eventData)
    {
        Debug.Log("InputPanelClick");
    }
}
