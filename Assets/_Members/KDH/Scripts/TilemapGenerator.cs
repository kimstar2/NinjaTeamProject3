using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.Tilemaps;

namespace _Members.KDH.Scripts
{
    public class TilemapGenerator : MonoBehaviour
    {
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        [field: SerializeField] public Tilemap Wallmap { get; private set; }
        [SerializeField] private TileBase _roadBase;
        [SerializeField] private TileBase _roomBase;
        [SerializeField] private TileBase _wallBase;

        private HashSet<Vector2Int> _tiles = new();

        public void GenerateTiles(BSPMapGenerator mapGenerator)
        {
            _tiles.Clear();
            Tilemap.ClearAllTiles();
            Wallmap.ClearAllTiles();

            DrawRoomTiles(mapGenerator.RootNode);
            DrawRoadTiles(mapGenerator);
            DrawWallTiles(mapGenerator);
        }

        private void DrawRoadTiles(BSPMapGenerator mapGenerator)
        {
            foreach (var road in mapGenerator.Roads)
            {
                if (_tiles.Contains(road))
                {
                    continue;
                }

                Tilemap.SetTile((Vector3Int)road, _roadBase);
                _tiles.Add(road);
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

                        if (_tiles.Contains(new Vector2Int(x, y)))
                        {
                            continue;
                        }

                        Tilemap.SetTile(new Vector3Int(x, y, 0), _roomBase);
                        _tiles.Add(new Vector2Int(x, y));
                    }
                }
            }
            else
            {
                DrawRoomTiles(node.LeftNode); // 더 이상 뻗어나갈 길이 없으면 거기까지 생성
                DrawRoomTiles(node.RightNode); // 더 이상 뻗어나갈 길이 없으면 거기까지 생성
            }
        }

        private void DrawWallTiles(BSPMapGenerator mapGenerator)
        {
            foreach (var wall in mapGenerator.Walls)
            {
                if (_tiles.Contains(wall))
                {
                    continue;
                }

                Wallmap.SetTile((Vector3Int)wall, _wallBase);
                _tiles.Add(wall);         
            }
        }
    }
}
