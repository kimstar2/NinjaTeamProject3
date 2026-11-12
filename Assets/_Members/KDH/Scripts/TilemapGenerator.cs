using _Members.KDH.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace KDH.Scripts
{
    public class TilemapGenerator : MonoBehaviour
    {
        [SerializeField] private BSPMapGenerator _mapGenerator;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _roadBase;
        [SerializeField] private TileBase _roomBase;

        private void Update()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                DrawRoadTiles();
                DrawRoomTiles(_mapGenerator.currentNode);
            }
        }

        private void DrawRoadTiles()
        {
            foreach (var road in _mapGenerator.roads)
            {
                _tilemap.SetTile((Vector3Int)road, _roadBase);
            }
        }

        private void DrawRoomTiles(BSPNode node) // 그릴 때 기즈모랑 다르게 그려져서 ChatGPT한테 물어봄.
        {                                        // 원인: 시작값을 폭의 -, 끝나는 값을 +으로 끝냈을 때 좌우 반전이 같을 때는 운 좋게 기즈모에 맞아 떨어진 거고
            if (node == null)                    // 좌우로 +-1정도 차이나면 한 칸 정도 차이가 났던 거
            {
                return;
            }

            if (node.LeftNode == null && node.RightNode == null)
            {
                for (int x = node.Room.xMin; x < node.Room.xMax; x++)
                {
                    for (int y = node.Room.yMin; y < node.Room.yMax; y++)
                    {
                        if (x == node.Room.center.x && y == node.Room.center.y)
                        {
                            continue;
                        }

                        _tilemap.SetTile(new Vector3Int(x, y, 0), _roomBase);
                    }
                }
            }
            else
            {
                DrawRoomTiles(node.LeftNode); // 더 이상 뻗어나갈 길이 없으면 거기까지 생성
                DrawRoomTiles(node.RightNode); // 더 이상 뻗어나갈 길이 없으면 거기까지 생성
            }
        }
    }
}
