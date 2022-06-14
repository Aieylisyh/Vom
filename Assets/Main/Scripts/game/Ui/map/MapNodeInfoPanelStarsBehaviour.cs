using UnityEngine;
using DG.Tweening;
using com;

public class MapNodeInfoPanelStarsBehaviour : MonoBehaviour
{
    public RectTransform star1;
    public RectTransform star2;
    public RectTransform star3;

    public GameObject view;

    public float delay1;
    public float delay2;
    public float delay3;

    public void Hide()
    {
        view.SetActive(false);
    }

    public void ShowAndClear()
    {
        view.SetActive(true);
        star1.localScale = Vector3.zero;
        star2.localScale = Vector3.zero;
        star3.localScale = Vector3.zero;
    }

    void PlaySound()
    {
        SoundService.instance.Play("notif");
    }

    public void Show1Star()
    {
        star1.DOScale(1, 0.6f).SetEase(Ease.OutBack).SetDelay(delay1).OnPlay(PlaySound);
    }

    public void Show2Star()
    {
        Show1Star();
        star2.DOScale(1, 0.6f).SetEase(Ease.OutBack).SetDelay(delay2).OnPlay(PlaySound);
    }

    public void Show3Star()
    {
        Show2Star();
        star3.DOScale(1, 0.6f).SetEase(Ease.OutBack).SetDelay(delay3).OnPlay(PlaySound);
    }
}