using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public static class AgentUtil
    {
        public static float GetZAngle(Vector2 dir)
        {
            if (Mathf.Approximately(dir.y, 0f) || Mathf.Approximately(dir.x, 0f))
                return 0f;
            
            float z = Mathf.Atan2(dir.y, dir.x);
            return z;
        }
    }
}