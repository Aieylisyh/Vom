using UnityEngine;
//using DG.Tweening;

namespace game
{
    public class BoatLevelupPartViewBehaviour : MonoBehaviour
    {
        public Transform target;
        public Vector3 goodScale = Vector3.one;
        public float amplitude = 0.2f;
        public float freq = 1;
        public ParticleSystem ps;
        private bool _isAnimating;

        public int correspondingBoatLevel;
        private Material[] _mats;
        private MeshRenderer _mr;
        public void Init()
        {
            if (target == null)
            {
                target = transform;
            }

            _mr = GetComponentInChildren<MeshRenderer>();
            _mats = _mr.materials;
        }

        void Update()
        {
            if (_isAnimating && target.gameObject.activeSelf)
            {
                var deltaSize = 1 + Mathf.Sin(Time.time * freq) * amplitude;
                target.localScale = goodScale * deltaSize;
            }
        }

        public void Show()
        {
            target.gameObject.SetActive(true);
        }

        public void Hide()
        {
            target.gameObject.SetActive(false);
        }

        public void StartAnimate()
        {
            //_isAnimating = true;
            var materials = _mr.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = FishingBoatBehaviour.instance.mmt.material;
            }
            _mr.materials = materials;

            if (ps != null)
            {
                ps.transform.position = transform.position;
                ps.Play(true);
            }
        }

        public void StopAnimate()
        {
            //_isAnimating = false;
            var materials = _mr.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _mats[i];
            }
            _mr.materials = materials;

            ps?.Stop(true);
        }
    }
}
