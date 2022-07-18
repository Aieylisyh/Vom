using UnityEngine;
using vom;
using game;

namespace com
{
    public class MmoCameraCinematicSystem : MonoBehaviour
    {
        public static MmoCameraCinematicSystem instance { get; private set; }

        public MmoCameraBehaviour cam;

        private void Awake()
        {
            instance = this;
        }

        void DisablePlayerControl()
        {
            InputPanel.instance.DisableInput();
        }

        void EnablePlayerControl()
        {
            InputPanel.instance.EnableInput();
        }

        void DisablePlayerCamera()
        {
            cam.enabled = false;
        }

        void EnablePlayerCamera()
        {
            cam.enabled = true;
        }

        public void LitBlessing(Transform player, Transform other, BlessingBehaviour bb)
        {
            var cinematic = CinematicCameraService.instance;

            cinematic.ResetEvents();
            CinematicEventPrototype e1 = new CinematicEventPrototype();
            e1.TimeToNext = 0;
            e1.type = CinematicActionTypes.CallFunc;
            e1.action = () =>
            {
                DisablePlayerCamera();
                DisablePlayerControl();
                PlayerBehaviour.instance.move.Rotate(other.position - player.position);
            };

            CinematicEventPrototype e2 = new CinematicEventPrototype();
            e2.TimeToNext = 2.2f;
            e2.duration = 2.2f;
            e2.ease = DG.Tweening.Ease.InOutCubic;
            e2.type = CinematicActionTypes.TweenPositionAndRotation;
            e2.usePositionAndRotation = true;
            var camPos = cinematic.target.position;
            var offset = camPos - player.position;
            var centerPos = (player.position + other.position) * 0.5f;
            offset.x = offset.x * 0.4f;
            offset.y = offset.y * 0.2f;
            offset.z = offset.z * 0.4f;
            var pendDir = Vector3.Cross(Vector3.up, other.position - player.position);


            var goodPos = centerPos + offset + pendDir.normalized * ((other.position.x - player.position.x > 0) ? 3.1f : -3.1f);
            e2.position = goodPos + Vector3.up * 0.3f;
            e2.rotation = Quaternion.LookRotation(centerPos - goodPos);

            CinematicEventPrototype e3 = new CinematicEventPrototype();
            e3.TimeToNext = 0;
            e3.type = CinematicActionTypes.CallFunc;
            e3.action = () =>
            {
                PlayerBehaviour.instance.LitMovement();
                bb.DoLit(cinematic.target.position);
            };

            CinematicEventPrototype e4 = new CinematicEventPrototype();
            e4.TimeToNext = 5.7f;
            e4.duration = 5.5f;
            e4.ease = DG.Tweening.Ease.InOutCubic;
            e4.type = CinematicActionTypes.TweenPositionAndRotation;
            e4.usePositionAndRotation = true;

            e4.position = e2.position + Vector3.up * 1.2f + offset * 0.2f;
            e4.rotation = Quaternion.LookRotation(centerPos - e2.position);

            CinematicEventPrototype e5 = new CinematicEventPrototype();
            e5.TimeToNext = 0;
            e5.type = CinematicActionTypes.CallFunc;
            e5.action = () =>
            {
                EnablePlayerCamera();
                EnablePlayerControl();
            };

            cinematic.AddEvents(e1);
            cinematic.AddEvents(e2);
            cinematic.AddEvents(e3);
            cinematic.AddEvents(e4);
            cinematic.AddEvents(e5);
            cinematic.StartService();
        }

        public void NpcView(Transform player, NpcBehaviour other)
        {
            var cinematic = CinematicCameraService.instance;

            var startingPos = Vector3.zero;
            var startingRot = Quaternion.identity;

            cinematic.ResetEvents();
            CinematicEventPrototype e0 = new CinematicEventPrototype();
            e0.TimeToNext = 0.2f;
            e0.type = CinematicActionTypes.CallFunc;
            e0.action = () =>
            {
                DisablePlayerCamera();
                DisablePlayerControl();
                other.Rotate(player.position - other.transform.position);
            };

            CinematicEventPrototype e1 = new CinematicEventPrototype();
            e1.TimeToNext = 0.0f;
            e1.type = CinematicActionTypes.CallFunc;
            e1.action = () =>
            {
                PlayerBehaviour.instance.move.Rotate(other.transform.position - player.position);
            };

            CinematicEventPrototype e2 = new CinematicEventPrototype();
            e2.TimeToNext = 1.5f;
            e2.duration = 1.5f;
            e2.ease = DG.Tweening.Ease.InOutCubic;
            e2.type = CinematicActionTypes.TweenPositionAndRotation;
            e2.usePositionAndRotation = true;
            var camPos = cinematic.target.position;
            var offset = camPos - player.position;
            var centerPos = (player.position + other.transform.position) * 0.5f;
            offset.x = offset.x * 0.75f;
            offset.y = offset.y * 0.6f;
            offset.z = offset.z * 0.75f;
            var pendDir = Vector3.Cross(Vector3.up, other.transform.position - player.position);

            var goodPos = centerPos + offset + pendDir.normalized * ((other.transform.position.x - player.position.x > 0) ? 0.3f : -0.3f);
            e2.position = goodPos + Vector3.up * 0.3f;
            e2.rotation = Quaternion.LookRotation(centerPos - goodPos);

            CinematicEventPrototype e3 = new CinematicEventPrototype();
            e3.TimeToNext = 1.5f;
            e3.type = CinematicActionTypes.CallFunc;
            e3.action = () =>
            {
                other.OnInteract();

                CinematicEventPrototype e5 = new CinematicEventPrototype();
                e5.TimeToNext = 1.5f;
                e5.duration = 1.5f;
                e5.ease = DG.Tweening.Ease.InOutCubic;
                e5.type = CinematicActionTypes.TweenPositionAndRotation;
                e5.usePositionAndRotation = true;
                cam.SetPosAndRot(ref startingPos, ref startingRot);
                e5.position = startingPos;
                e5.rotation = startingRot;

                CinematicEventPrototype e6 = new CinematicEventPrototype();
                e6.TimeToNext = 0;
                e6.type = CinematicActionTypes.CallFunc;
                e6.action = () =>
                {
                    EnablePlayerCamera();
                    EnablePlayerControl();
                };

                cinematic.AddEvents(e5);
                cinematic.AddEvents(e6);
            };

            CinematicEventPrototype e4 = new CinematicEventPrototype();
            e4.TimeToNext = 0.0f;
            e4.type = CinematicActionTypes.CallFunc;
            e4.action = () =>
            {
                //just to make cinematic not stop
            };

            cinematic.AddEvents(e0);
            cinematic.AddEvents(e1);
            cinematic.AddEvents(e2);
            cinematic.AddEvents(e3);
            cinematic.AddEvents(e4);

            cinematic.StartService();
        }
    }
}