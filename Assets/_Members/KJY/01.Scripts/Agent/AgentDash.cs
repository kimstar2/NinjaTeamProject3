using System;
using System.Threading;
using _Members.KJY._01.Scripts.Utils;
using _TevLib.ModuleSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public class AgentDash : MonoModule, IDash
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashDuration;
        [SerializeField] private Ease dashEase;
        [SerializeField] private float dashCooldown;
        
        public bool IsDashing => _dashTween.IsActive();
        public bool CanDash => _crtCooldown + dashCooldown <= Time.time;
        
        private IVelocityAddable _mover;
        
        private CancellationTokenSource _cts;
        private Tween _dashTween;

        private float _crtCooldown;
        
        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            _mover = owner.GetModule<IVelocityAddable>();
        }


        public void Dash(Vector2 targetDir)
        {
            if (IsDashing | !CanDash) return;
            
            _crtCooldown = Time.time;
            
            _cts?.KillToken();
            _cts = new CancellationTokenSource();

            DashLogic(_cts.Token, targetDir).Forget();
        }

        private async UniTask DashLogic(CancellationToken token, Vector2 targetDir)
        {
            float applyValue = dashForce;
            _mover.SetAddVelocity(targetDir * applyValue);
            
            try
            {
                _dashTween?.Kill();
                _dashTween = DOTween.To(() => applyValue, x =>
                {
                    applyValue = x;
                    _mover.SetAddVelocity(targetDir * applyValue);
                }, 0f, dashDuration).SetEase(dashEase);
                await _dashTween.ToUniTask(cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _mover.SetAddVelocity(targetDir * applyValue);
            }
        }
    }
}
