using _Members.KJY._01.Scripts.Player;
using UnityEngine;

namespace _Members.KDH.Scripts
{
    public class MapSpawnPointer : MonoBehaviour
    {
        public void PlayerSpawn(BSPMapGenerator mapGenerator)
        {
            GetPlayer().transform.position = SetSpawnPointer(mapGenerator);
        }

        private Vector2 SetSpawnPointer(BSPMapGenerator mapGenerator)
        {
            return mapGenerator.Rooms[mapGenerator.Rooms.Count - 1].Room.center;
        }

        private PlayerController GetPlayer()
        {
            return FindFirstObjectByType<PlayerController>();
        }
    }
}
