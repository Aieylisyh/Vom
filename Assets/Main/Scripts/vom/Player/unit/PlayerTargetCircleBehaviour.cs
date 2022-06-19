using UnityEngine;

namespace vom
{
    public class PlayerTargetCircleBehaviour : MonoBehaviour
    {
        public GameObject circle;

        public void Hide()
        {
            circle.transform.SetParent(transform);
            circle.SetActive(false);
        }

        public void Show(EnemyBehaviour e)
        {
            Transform parent = e.circleTrans == null ? e.transform : e.circleTrans;

            circle.transform.SetParent(parent);
            circle.transform.localPosition = Vector3.zero;
            circle.transform.localScale = Vector3.one;
            circle.transform.localEulerAngles = new Vector3(90, 0, 0);
            circle.SetActive(true);
        }
    }
}