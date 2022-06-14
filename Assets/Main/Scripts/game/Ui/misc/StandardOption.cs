using UnityEngine;

namespace game
{
    public class StandardOption : MonoBehaviour
    {
        public GameObject ban;
        public void SetBan(bool isBanned)
        {
            ban.SetActive(isBanned);
        }
    }
}