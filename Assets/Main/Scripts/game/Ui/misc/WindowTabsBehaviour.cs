using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class WindowTabsBehaviour : MonoBehaviour
    {
        public List<WindowTabBehaviour> tabs;
        public bool hasSound = true;
        private int _currentTab;

        public float tabWidth_1 = 300;
        public float tabWidth_2 = 240;
        public float tabWidth_3 = 210;
        public float offsetX_2 = 150;
        public float offsetX_3 = 210;

        private void SetTabRectPos(RectTransform rect, float x, float w)
        {
            rect.gameObject.SetActive(true);
            rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
            rect.sizeDelta = new Vector2(w, rect.sizeDelta.y);
        }

        public void HideAll()
        {
            foreach (var tab in tabs)
            {
                tab.gameObject.SetActive(false);
            }
        }

        public void SetTo1TabLayout(int active1Index)
        {
            HideAll();
            SetTabRectPos(tabs[active1Index].GetComponent<RectTransform>(), 0, tabWidth_1);
        }

        public void SetTo2TabsLayout(int active1Index, int active2Index)
        {
            HideAll();
            SetTabRectPos(tabs[active1Index].GetComponent<RectTransform>(), -offsetX_2, tabWidth_2);
            SetTabRectPos(tabs[active2Index].GetComponent<RectTransform>(), offsetX_2, tabWidth_2);
        }

        public void SetTo3TabsLayout(int active1Index, int active2Index, int active3Index)
        {
            HideAll();
            SetTabRectPos(tabs[active1Index].GetComponent<RectTransform>(), -offsetX_3, tabWidth_3);
            SetTabRectPos(tabs[active2Index].GetComponent<RectTransform>(), 0, tabWidth_3);
            SetTabRectPos(tabs[active3Index].GetComponent<RectTransform>(), offsetX_3, tabWidth_3);
        }

        public void OnClickTab1()
        {
            Sound();
            SetTab(0);
        }
        public void OnClickTab2()
        {
            Sound();
            SetTab(1);
        }
        public void OnClickTab3()
        {
            Sound();
            SetTab(2);
        }

        public void SetTab(int index = 0)
        {
            _currentTab = index;
            foreach (var t in tabs)
            {
                if (t == tabs[index])
                    t.On();
                else
                    t.Off();
            }
        }
        public int GetCurrentTab()
        {
            return _currentTab + 1;
        }
        public void Off()
        {
            foreach (var t in tabs)
            {
                t.Off();
            }
        }

        private void Sound()
        {
            SoundService.instance.Play("tap");
        }
    }
}
