using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Members.NKY.Scripts.Interface;
using _TevLib.ModuleSystem;
using _TevLib.TileAstar;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.NKY.Scripts
{
    public class AstarMovementModule : MonoModule, IAstarMoveable
    {
        [field:SerializeField] public float MoveSpeed { get; private set; }
        [SerializeField] private LayerMask obstacleMask;
        public Rigidbody2D RbCompo { get; private set; }
        public Vector2 MoveDir { get; private set; }
        public PathAgent PathCompo { get; private set; }
        private Vector2 currentPoint;
        private bool followToDest;
        private Vector3 nextDir;
        

        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            RbCompo = owner.GetComponent<Rigidbody2D>();
            PathCompo = owner.GetComponent<PathAgent>();

            if (PathCompo == null)
            {
                Debug.LogError("Path composer not found");
            }

            if (RbCompo == null)
            {
                Debug.LogError("Rb composer not found");
            }
        }

        private void FixedUpdate()
        {
            if (followToDest)
            {

                Vector2 dir = (Vector2)currentPoint - RbCompo.position;
                Vector2 newDir = dir.normalized * MoveSpeed;
                RbCompo.linearVelocity = Vector2.MoveTowards(RbCompo.linearVelocity, newDir, MoveSpeed * Time.fixedDeltaTime);
            }
        }
        
        public bool CanMoveStraightOnMainThread(Vector2 start, Vector2 end)
        {
            Vector2 dir = end - start;
            float distance = dir.magnitude;
            if (distance <= 0.001f) return true;

            // 장애물 레이어에 걸리는 것이 없으면 true
            RaycastHit2D hit = Physics2D.Raycast(start, dir.normalized, distance, obstacleMask);
            return hit.collider == null;
        }

        public async UniTask<bool> StartAstarMove(Vector2 destination)
        {
            List<Vector2> currentPath = new List<Vector2>();
            Vector3 startPos = RbCompo.position;
            Vector3 endPos = destination;
            Vector3Int startCellPos = PathAgent.WorldToCell(startPos);
            Vector3Int destinationCellPos = PathAgent.WorldToCell(endPos);
            if (!CanMoveStraightOnMainThread(startPos, endPos))
            {
                int cornerCount = await PathCompo.GetPath(startCellPos, destinationCellPos, currentPath, obstacleMask);
            
                if(cornerCount <= 0)
                    return false;
            }
            else
            {
                currentPath.Add(destination);
            }

            followToDest = true;
            foreach (Vector2 point in currentPath)
            {
                currentPoint = point;
                Vector2 direction = currentPoint - RbCompo.position;
                RbCompo.linearVelocity = direction.normalized * MoveSpeed;
                await UniTask.WaitUntil(() => Vector2.Distance(point, RbCompo.position) <= 0.1f);
                RbCompo.linearVelocity = Vector2.zero;
                RbCompo.position = point;
            }
            return true;
        }

        public void StopAstarMove()
        {
            PathCompo.CancelPath();
            RbCompo.linearVelocity = Vector2.zero;
            followToDest = false;
        }
        
        public void SetDirection(Vector2 moveDir)
        {
        }

        public void SetMulti(float multi)
        {
            
        }

        public void SetSpeed(float speed)
        {
            MoveSpeed = speed;
        }
    }
}
