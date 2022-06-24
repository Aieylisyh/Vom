namespace vom
{
    public class MapVolumeTypeMerger
    {
        public static VolumeSetter Merge(VolumeSetter newVs, VolumeSetter oldVs)
        {
            VolumeSetter vs = new VolumeSetter();

            vs.core = Compare(newVs.core, oldVs.core) > 0 ? newVs.core : oldVs.core;

            vs.terrain = Compare(newVs.terrain, oldVs.terrain) > 0 ? newVs.terrain : oldVs.terrain;

            if (Compare(newVs.ground, oldVs.ground) > 0)
            {
                vs.ground = newVs.ground;
                vs.groundPercentage = newVs.groundPercentage;
            }
            else if (Compare(newVs.ground, oldVs.ground) == 0)
            {
                vs.ground = oldVs.ground;
                vs.groundPercentage = UnityEngine.Mathf.Max(oldVs.groundPercentage, newVs.groundPercentage);
            }
            else
            {
                vs.ground = oldVs.ground;
                vs.groundPercentage = oldVs.groundPercentage;
            }

            if (Compare(newVs.obstacle, oldVs.obstacle) > 0)
            {
                vs.obstacle = newVs.obstacle;
                vs.obstaclePercentage = newVs.obstaclePercentage;
            }
            else if (Compare(newVs.obstacle, oldVs.obstacle) == 0)
            {
                vs.obstacle = oldVs.obstacle;
                vs.obstaclePercentage = UnityEngine.Mathf.Max(oldVs.obstaclePercentage, newVs.obstaclePercentage);
            }
            else
            {
                vs.obstacle = oldVs.obstacle;
                vs.obstaclePercentage = oldVs.obstaclePercentage;
            }

            return vs;
        }

        public static int Compare(VolumeCoreType a, VolumeCoreType b)
        {
            if (a > b)
                return 1;
            if (a < b)
                return -1;

            return 0;
        }

        public static int Compare(VolumeGroundType a, VolumeGroundType b)
        {
            if (a > b)
                return 1;
            if (a < b)
                return -1;

            return 0;
        }


        public static int Compare(VolumeTerrainType a, VolumeTerrainType b)
        {
            if (a > b)
                return 1;
            if (a < b)
                return -1;

            return 0;
        }

        public static int Compare(VolumeObstacleType a, VolumeObstacleType b)
        {
            if (a > b)
                return 1;
            if (a < b)
                return -1;

            return 0;
        }
    }
}
