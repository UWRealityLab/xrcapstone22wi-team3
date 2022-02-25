using System;
using UnityEngine;

namespace Obstacles
{ public abstract class VoiceObstacle : Obstacle
    {
        public override float GetSpawnOffsetLowerBound()
        {
            throw new NotSupportedException();
        }

        public override float GetSpawnOffsetUpperBound()
        {
            throw new NotSupportedException();

        }

        public override Quaternion GetSpawnRotation()
        {
            throw new NotSupportedException();
        }
    }
}