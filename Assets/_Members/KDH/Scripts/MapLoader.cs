using UnityEngine;

namespace _Members.KDH.Scripts
{
    public class MapLoader : MonoBehaviour
    {
        [field: SerializeField] public BSPMapGenerator MapGenerator { get; private set; }
        [field: SerializeField] public TilemapGenerator TilemapGenerator { get; private set; }
        [field: SerializeField] public MapColliderGenerator ColliderGenerator { get; private set; }
        [field: SerializeField] public MapSpawnPointer SpawnPointer { get; private set; }
        [field: SerializeField] public EnemyGenerator EnemyGenerator { get; private set; }

        private void Start()
        {
            MapGenerator.GenerateMap();
            TilemapGenerator.GenerateTiles(MapGenerator);
            ColliderGenerator.SetCollider();
            SpawnPointer.PlayerSpawn(MapGenerator);
            EnemyGenerator.SpawnEnemies(MapGenerator);
        }
    }
}
