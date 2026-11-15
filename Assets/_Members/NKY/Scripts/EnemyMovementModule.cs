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
    public class EnemyMovementModule : MonoModule, IAstarMoveable
    {
        [field:SerializeField] public float MoveSpeed { get; private set; }
        public Rigidbody2D RbCompo { get; private set; }
        public Vector2 MoveDir { get; private set; }
        public PathAgent PathCompo { get; private set; }
        private Vector3 currentPoint;
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

        public async UniTask<bool> StartAstarMove(Vector2 destination)
        {
            List<Vector3> currentPath = new List<Vector3>();
            int cornerCount = await PathCompo.GetPath(PathAgent.WorldToCell(RbCompo.position)
                , PathAgent.WorldToCell(destination), currentPath);
            
            if(cornerCount <= 0)
                return false;

            followToDest = true;
            foreach (Vector3 point in currentPath)
            {
                currentPoint = point;
                await UniTask.WaitUntil(() => Vector2.Distance(point, RbCompo.position) <= 0.1f);
                RbCompo.linearVelocity = Vector2.zero;
                RbCompo.position = point;
            }
            return true;
        }

        public void StopAstarMove()
        {
            PathCompo.CancelPath();
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
