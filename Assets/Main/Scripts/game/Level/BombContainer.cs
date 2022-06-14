using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombContainer : MonoBehaviour
{
    public List<Image> bombList;
    public static BombContainer instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetBombRestNum(int num)
    {
        int i = num;
        foreach (var b in bombList)
        {
            b.color = (i > 0 ? Color.white : new Color(1, 1, 1, 0.4f));
            i--;
        }
    }

    public void SetBombNum(int num)
    {
        int i = num;
        foreach (var b in bombList)
        {
            b.gameObject.SetActive(i > 0);
            i--;
        }
    }
}
