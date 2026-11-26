using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace _TevLib.TileAstar
{
    public class PathAgent : MonoBehaviour
    { 
        [SerializeField] private PathBakeDataSO bakedData;

        private readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);
        private CancellationTokenSource _cts;
        private bool _isCalculating;
        private Vector3Int _lastDestination;

        public bool PathPending => _isCalculating;
        public bool HasPath { get; private set; }
        public bool IsPathStale { get; private set; }

        private (List<Vector3> waypoints, bool isSuccess) CalculatePathWorker(
            PathBakeDataSO mapData,
            Vector3Int startPosition,
            Vector3Int destination,
            CancellationToken ct)
        {
            List<Vector3> resultPoints = new List<Vector3>();

            if (mapData == null)
                return (resultPoints, false);

            if (!mapData.GetNodeIfExist(startPosition, out NodeData startNode))
                return (resultPoints, false);

            if (!mapData.GetNodeIfExist(destination, out NodeData destNode))
                return (resultPoints, false);
            
            Stack<AstarNode> localPool = new Stack<AstarNode>();
            AstarNode RentLocal() => localPool.Count > 0 ? localPool.Pop() : new AstarNode();

            PriorityQueue<AstarNode> openList = new PriorityQueue<AstarNode>();
            HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
            AstarNode foundNode = null;

            AstarNode startAstar = RentLocal();
            startAstar.nodeData = startNode;
            startAstar.cellPosition = startNode.cellPosition;
            startAstar.worldPosition = startNode.worldPosition;
            startAstar.parentNode = null;
            startAstar.G = 0f;
            startAstar.F = CalcH(startNode.cellPosition, destNode.cellPosition);
            openList.Push(startAstar);

            while (openList.Count > 0)
            {
                if (ct.IsCancellationRequested)
                    return (resultPoints, false);

                AstarNode currentNode = openList.Pop();

                if (closedSet.Contains(currentNode.cellPosition))
                {
                    localPool.Push(currentNode);
                    continue;
                }

                closedSet.Add(currentNode.cellPosition);

                if (currentNode.nodeData == destNode)
                {
                    foundNode = currentNode;
                    break;
                }

                foreach (LinkData link in currentNode.nodeData.neighbors)
                {
                    if (ct.IsCancellationRequested)
                        return (resultPoints, false);

                    if (closedSet.Contains(link.endCellPosition))
                        continue;

                    if (!mapData.GetNodeIfExist(link.endCellPosition, out NodeData nextNode))
                        continue;

                    float newG = currentNode.G + link.cost;

                    AstarNode nextAstar = RentLocal();
                    nextAstar.nodeData = nextNode;
                    nextAstar.cellPosition = nextNode.cellPosition;
                    nextAstar.worldPosition = nextNode.worldPosition;
                    nextAstar.parentNode = currentNode;
                    nextAstar.G = newG;
                    nextAstar.F = newG + CalcH(nextNode.cellPosition, destNode.cellPosition);

                    AstarNode existInOpenNode = openList.Contains(nextAstar);
                    if (existInOpenNode != null)
                    {
                        if (nextAstar.G < existInOpenNode.G)
                        {
                            existInOpenNode.G = nextAstar.G;
                            existInOpenNode.F = nextAstar.F;
                            existInOpenNode.parentNode = nextAstar.parentNode;
                            openList.DecreaseKey(existInOpenNode);
                        }
                        localPool.Push(nextAstar);
                    }
                    else
                    {
                        openList.Push(nextAstar);
                    }
                }
            }

            if (foundNode == null)
                return (resultPoints, false);

            List<AstarNode> rawPath = new List<AstarNode>();
            AstarNode trace = foundNode;
            while (trace != null)
            {
                rawPath.Add(trace);
                trace = trace.parentNode;
            }
            rawPath.Reverse();

            for (int i = 1; i < rawPath.Count - 1; i++)
            {
                Vector3Int beforeDir = rawPath[i].cellPosition - rawPath[i - 1].cellPosition;
                Vector3Int nextDir = rawPath[i + 1].cellPosition - rawPath[i].cellPosition;
                if (beforeDir != nextDir)
                {
                    resultPoints.Add(rawPath[i].worldPosition);
                }
            }

            if (rawPath.Count > 0)
            {
                resultPoints.Add(rawPath[^1].worldPosition);
            }

            return (resultPoints, true);
        }

        public void CancelPath()
        {
            _cts?.Cancel();
        }

        public async Task<int> GetPath(Vector3Int startPosition, Vector3Int destination, List<Vector2> pointArr, LayerMask obstacleLayer)
        {
            if (pointArr == null)
                throw new ArgumentNullException(nameof(pointArr));

            // 이전 경로 탐색 취소 요청
            _cts?.Cancel();

            await _gate.WaitAsync().ConfigureAwait(true);
            try
            {
                if (HasPath && destination != _lastDestination)
                    IsPathStale = true;

                // 메인 스레드에서 먼저 최종 목적지까지 직선 이동 가능 여부 확인
                Vector2 startWorld = transform.position;
                Vector2 destWorld = CellToWorld(destination);
                if (CanMoveStraight(startWorld, destWorld, obstacleLayer))
                {
                    pointArr.Clear();
                    pointArr.Add(destWorld);
                    HasPath = true;
                    IsPathStale = false;
                    _lastDestination = destination;
                    return pointArr.Count;
                }

                _cts?.Dispose();
                _cts = new CancellationTokenSource();
                CancellationToken ct = _cts.Token;
                PathBakeDataSO mapDataSnapshot = bakedData;

                _isCalculating = true;
                var (waypoints, isSuccess) = await Task.Run(
                    () => CalculatePathWorker(mapDataSnapshot, startPosition, destination, ct),
                    ct);

                if (ct.IsCancellationRequested)
                    return 0;

                if (isSuccess && waypoints.Count > 0)
                {
                    pointArr.Clear();
                    for (int i = 0; i < waypoints.Count; i++)
                    {
                        pointArr.Add(waypoints[i]);
                    }

                    HasPath = true;
                    IsPathStale = false;
                    _lastDestination = destination;
                    return pointArr.Count;
                }

                HasPath = false;
                IsPathStale = false;
                return 0;
            }
            catch (OperationCanceledException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                HasPath = false;
                IsPathStale = false;
                return -1;
            }
            finally
            {
                _isCalculating = false;
                _gate.Release();
            }
        }

        private bool CanMoveStraight(Vector2 start, Vector2 end, LayerMask obstacleMask)
        {
            Vector2 dir = end - start;
            float distance = dir.magnitude;
            if (distance <= 0.0001f)
                return true;

            RaycastHit2D hit = Physics2D.Raycast(start, dir.normalized, distance, obstacleMask);
            return hit.collider == null;
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _gate.Dispose();
        }

        #region Helper method

        public static Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return new Vector3Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y), 0);
        }

        public static Vector3 CellToWorld(Vector3Int cellPosition)
        {
            return new Vector3(cellPosition.x, cellPosition.y, cellPosition.z);
        }

        private static float CalcH(Vector3Int startPoint, Vector3Int destinationPoint)
            => Vector3Int.Distance(startPoint, destinationPoint);

        #endregion
    }
}