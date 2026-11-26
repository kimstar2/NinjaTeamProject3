using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Members.KDH.Scripts
{
    public class MapColliderGenerator : MonoBehaviour
    {
        [SerializeField] private TilemapCollider2D _mapTileCollider;

        public void SetCollider()
        {
            _mapTileCollider.CreateMesh(true, true);
        }
    }
}
