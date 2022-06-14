using UnityEngine;

namespace game
{
    public class GlobalBehaviour : MonoBehaviour
    {
        public string Version
        {
            get
            {
                return "" + Version_main + "." + Version_sub + "." + Version_patch;
            }
        }
        public string VersionWithChannel
        {
            get
            {
                return Version + "_" + channel.ToString();
            }
        }

        public int Version_main;
        public int Version_sub;
        public int Version_patch;
        public Channel channel;

        public enum Channel
        {
            Prime,
            Pc,
            Apple,
            Gp,
        }
    }
}
