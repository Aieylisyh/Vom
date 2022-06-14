using UnityEngine;
using UnityEngine.UI;
using com;
using System.Collections.Generic;
using DG.Tweening;

namespace game
{
    public class MapNode : MonoBehaviour
    {
        public MapNodeData normal;
        public MapNodeData elite;
        public MapNodeData boss;

        public Image iconImage;
        private RectTransform _rect;

        public MapNodePrototype proto { get; private set; }

        public List<MapNodePath> paths;

        public GameObject star1;
        public GameObject star2;
        public GameObject star3;

        public GameObject view;
        public GameObject lockedView;

        public void OnClick()
        {
            SoundService.instance.Play("btn info");
            MapWindowBehaviour.instance.OnSelectNode(proto.levelId);
        }

        public void SetDisplayed(bool b)
        {
            view.SetActive(b);
            foreach (var path in paths)
            {
                path.SetDisplayed(b);
            }
        }

        public void SetUnlocked(bool b)
        {
            lockedView.SetActive(!b);
        }

        public void SetPassed(bool p)
        {
            proto.passed = p;
            SetSprite();
            foreach (var path in paths)
            {
                path.SetPassed(p);
            }
        }

        void SetSprite()
        {
            MapNodeBaseData data = new MapNodeBaseData();
            switch (proto.nodeViewType)
            {
                case MapNodePrototype.NodeViewType.Normal:
                    data = proto.passed ? normal.passed : normal.raw;
                    break;

                case MapNodePrototype.NodeViewType.Elite:
                    data = proto.passed ? elite.passed : elite.raw;
                    break;

                case MapNodePrototype.NodeViewType.Boss:
                    data = proto.passed ? boss.passed : boss.raw;
                    break;
            }

            iconImage.sprite = data.sp;
            iconImage.rectTransform.sizeDelta = new Vector2(data.width, iconImage.rectTransform.sizeDelta.y);
        }

        public void Setup(MapNodePrototype pProto)
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
            proto = pProto;
            _rect.anchoredPosition = new Vector2(proto.offsetX, proto.offsetY);
            gameObject.SetActive(true);
        }

        public void SetShake(bool b)
        {
            _rect.DOKill();
            _rect.localScale = Vector3.one;
            if (b)
                StartShake();
        }

        void StartShake()
        {
            _rect.DOPunchScale(Vector3.one * 0.20f, 1.0f, 1, 0).OnComplete(StartShake);
        }

        public void SetStars(int count = 0)
        {
            star1.SetActive(count > 0);
            star2.SetActive(count > 1);
            star3.SetActive(count > 2);
        }
    }
}