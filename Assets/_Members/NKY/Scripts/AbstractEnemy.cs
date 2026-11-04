using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.NKY.Scripts
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        private CancellationTokenSource _cts;

        private async void Start()
        {
            _cts = new CancellationTokenSource();

            try
            {
                // 1. Delay에 CancellationToken 전달
                await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: _cts.Token);
                Debug.Log("딜레이 완료");
            }
            (OperationCanceledException)
            {
                Debug.Log("딜레이가 취소됨 (삭제)");
            }
        }

        public void CancelDelay()
        {
            // 2. 외부에서 취소(삭제)하고 싶을 때 호출
            _cts?.Cancel();
        }

        private void OnDestroy()
        {
            // 메모리 누수 방지를 위해 해제
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
