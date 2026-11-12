using System.Collections.Generic;
using UnityEngine;

namespace _Members.KDH.Scripts
{
    public class BSPMapGenerator : MonoBehaviour // BSP로 어떻게 유니티에 적용하는 지에 대해 물어봄
    {
        [Header("생성 세팅")]
        public int mapWidth;
        public int mapHeight;
        public int mapDepth;
        public int roadDepth;
        [Min(0.3f)] public float splitMinRatio; // 이진 분열할 때 비율 (최소한 0.3로 잡는 게 좋음)

        [field: SerializeField, Range(0, 0.5f)] public float MinRandomSize { get; private set; } = default;
        [field: SerializeField, Range(0.5f, 1)] public float MaxRandomSize { get; private set; } = 0.5f;

        [Header("시드 세팅")]
        public int seed; // 시드 값
        public bool useRandomSeed; // 랜덤 시드을 물어보는 BOOL

        public List<BSPNode> rooms = new(); // 노드
        public List<Vector2Int> roads = new(); // 노드
        public List<Vector2Int> walls = new();
        public BSPNode currentNode;

        private void Start()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            RandomSeed(); // 랜덤으로 굴림

            rooms.Clear(); // 혹시 남아있던 거 있을 수도 있으니 초기화
            roads.Clear();
            walls.Clear();

            BSPNode startNode = new(new RectInt(0, 0, mapWidth, mapHeight)); // 시작 점 찍기

            Split(startNode, mapDepth); // 분열 시작
            CreateRooms(startNode); // 만들고
            ConnectRoom(startNode); // 연결
            CreateWalls();
        }

        private void RandomSeed()
        {
            if (useRandomSeed)
            {
                seed = Random.Range(0, int.MaxValue); // 그보단 적은 범위
                //seed = Random.Range(int.MinValue, int.MaxValue); // 좀 더 넓은 범위에서
            }

            Random.InitState(seed);
        }

        private void Split(BSPNode node, int mapDepth) // 공간 분할
        {
            if (mapDepth <= 0 || node == null)
            {
                return;
            }

            if (node.Room.width < node.Room.height) // 이 때는 세로로 긴 공간
            {
                int applyLength = Mathf.RoundToInt(Random.Range(splitMinRatio * node.Room.height, (1 - splitMinRatio) * node.Room.height));
                node.LeftNode = new BSPNode(new RectInt(node.Room.x, node.Room.y, node.Room.width, applyLength));
                node.RightNode = new BSPNode(new RectInt(node.Room.x, node.Room.y + applyLength, node.Room.width, node.Room.height - applyLength));
            }
            else // 이 때는 가로로 긴 공간
            {
                int applyLength = Mathf.RoundToInt(Random.Range(splitMinRatio * node.Room.width, (1 - splitMinRatio) * node.Room.width));
                node.LeftNode = new BSPNode(new RectInt(node.Room.x, node.Room.y, applyLength, node.Room.height));
                node.RightNode = new BSPNode(new RectInt(node.Room.x + applyLength, node.Room.y, node.Room.width - applyLength, node.Room.height));
            }

            node.LeftNode.ParentNode = node;
            node.RightNode.ParentNode = node;

            rooms.Add(node.LeftNode);
            rooms.Add(node.RightNode);

            Split(node.LeftNode, mapDepth - 1); // 재귀
            Split(node.RightNode, mapDepth - 1); // 재귀
        }

        private void CreateRooms(BSPNode node) // 방 파기
        {
            if (node == null)
            {
                return;
            }

            if (node.LeftNode == null && node.RightNode == null)
            {
                int roomWidth = Mathf.RoundToInt(Random.Range(MinRandomSize, MaxRandomSize) * node.Room.width); // 사각형의 폭을 뽑기
                int roomHeight = Mathf.RoundToInt(Random.Range(MinRandomSize, MaxRandomSize) * node.Room.height); // 사각형의 높이 뽑기

                int roomX = node.Room.x + (node.Room.width - roomWidth) / 2; // 최소X
                int roomY = node.Room.y + (node.Room.height - roomHeight) / 2; // 최소Y

                node.Room = new RectInt(roomX, roomY, roomWidth, roomHeight);
            }
            else
            {
                CreateRooms(node.LeftNode); // 더 이상 뻗어나갈 길이 없으면 거기까지 생성
                CreateRooms(node.RightNode); // 더 이상 뻗어나갈 길이 없으면 거기까지 생성
            }

            currentNode = node;
        }

        private void ConnectRoom(BSPNode node) // 방끼리 길 잇기
        {
            if (node == null || node.LeftNode == null || node.RightNode == null)
            {
                return;
            }

            Vector2Int startVec = GetCenter(node.LeftNode);
            Vector2Int endVec = GetCenter(node.RightNode);

            (Vector2Int start, Vector2Int end) = (startVec, endVec); // AI한테 이게 뭔지 물어봄
                                                                     // 이건 Vector2Int를 두개를 담는 튜플임 사용할 떄는
                                                                     // temp_hallway.Item1, temp_hallway.Item2 이런식으로 사용할 수 있음.

            CreateHallway(startVec, endVec);

            ConnectRoom(node.LeftNode);
            ConnectRoom(node.RightNode);
        }

        public Vector2Int GetCenter(BSPNode node)
        {
            if (node == null)
                return Vector2Int.zero;

            if (node.LeftNode == null && node.RightNode == null)
            {
                return new Vector2Int(node.GetXCenter(), node.GetYCenter());
            }

            if (node.LeftNode != null)
            {
                return GetCenter(node.LeftNode);
            }

            return GetCenter(node.LeftNode);
        }

        private void AddRoad(Vector2Int position) // 바닥깔 때 중복 막어버렷
        {
            if (!roads.Contains(position))
            {
                roads.Add(position);
            }
        }

        private void AddWall(Vector2Int position)
        {
            if (roads.Contains(position) || walls.Contains(position))
            {
                return;
            }

            foreach (var room in rooms)
            {
                if (room.LeftNode == null && room.RightNode == null
                    && room.Room.Contains(position))
                {
                    return;
                }
            }

            walls.Add(position);
        }

        private void CreateHallway(Vector2Int start, Vector2Int end) // 길 잇는 법 AI한테 물어봄
        {
            int x = start.x;

            while (x != end.x) // 시작 점과 끝 점이 다를 때까지 
            {
                AddRoad(new Vector2Int(x, start.y)); // 시작부터 끝까지 선을 담기
                CreateHallwayDepth(new Vector2Int(x, start.y));

                x += (x < end.x) ? 1 : -1; // 시작 점이 끝 점보다 왼쪽에 있으면 그대로 한 칸 더 아니면 반대 방향으로 한 칸 더 가기 왜냐면 도착지점까지 찍어야 하니깐
            }

            AddRoad(new Vector2Int(end.x, start.y)); // 방 중심에 찍기
            CreateHallwayDepth(new Vector2Int(end.x, start.y));

            int y = start.y;

            while (y != end.y)
            {
                y += (y < end.y) ? 1 : -1; // 끝 점이 시작 점보다 왼쪽에 있으면 그대로 아니면 반대 방향으로

                AddRoad(new Vector2Int(end.x, y));  // 끝부터 시작까지 선을 담기
                CreateHallwayDepth(new Vector2Int(end.x, y));
            }
        }

        private void CreateHallwayDepth(Vector2Int position)
        {
            for (int x = -roadDepth; x <= roadDepth; x++)
            {
                for (int y = -roadDepth; y <= roadDepth; y++)
                {
                    Vector2Int nextPos = position + new Vector2Int(x, y);

                    if (nextPos == position || roads.Contains(nextPos))
                        continue;

                    AddRoad(nextPos);
                }
            }
        }

        private void CreateWalls()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    AddWall(new Vector2Int(x, y));
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var room in rooms)
            {
                if (room.LeftNode == null && room.RightNode == null)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.cyan;
                }

                Gizmos.DrawWireCube(room.Room.center, new Vector3(room.Room.width, room.Room.height, 1));
            }

            foreach (var road in roads)
            {
                Gizmos.color = Color.cyan;

                Gizmos.DrawCube(new Vector3(road.x + 0.5f, road.y + 0.5f, 0), Vector3.one);
            }

            foreach (var wall in walls)
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawCube(new Vector3(wall.x + 0.5f, wall.y + 0.5f, 0), Vector3.one);
            }
        }
    }
}