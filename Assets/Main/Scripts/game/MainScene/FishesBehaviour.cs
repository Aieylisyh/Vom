using UnityEngine;

namespace game
{
    public class FishesBehaviour : MonoBehaviour
    {
        public Animator animatorFish;
        public Animator animatorWater;
        public Renderer fishRenderer;
        public Material matFish1;
        public Material matFish2;

        public Transform[] trans;

        [Range(1.5f, 3)]
        public float intervalMax;
        [Range(0.2f, 1.5f)]
        public float intervalMin;

        private float timer;
        private int lastIndex;
        bool _lastIsFish1;

        void Start()
        {
            timer = 0;
            lastIndex = -1;
            _lastIsFish1 = false;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                var index = Mathf.FloorToInt(Random.value * trans.Length);
                if (lastIndex == index)
                {
                    return;
                }

                timer = Random.Range(intervalMin, intervalMax);
                var a = trans[index];
                lastIndex = index;
                SetFish(_lastIsFish1, a);
                _lastIsFish1 = !_lastIsFish1;
            }
        }

        private void SetFish(bool fish1, Transform a)
        {
            fishRenderer.material = fish1 ? matFish1 : matFish2;
            animatorFish.transform.position = a.position;
            animatorFish.transform.rotation = a.rotation;
            animatorFish.SetTrigger("f");
            animatorWater.SetTrigger("f");
        }
    }
}

