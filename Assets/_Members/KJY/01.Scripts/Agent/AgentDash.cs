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
        
        private IVelocityAddable _velocityAdder;
        private IMoverPlus _mover;
        
        private CancellationTokenSource _cts;
        private Tween _dashTween;

        private float _crtCooldown;
        
        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);

            _velocityAdder = owner.GetModule<IVelocityAddable>();
            _mover = owner.GetModule<IMoverPlus>();
            _crtCooldown -= dashCooldown;
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
            _mover.StopImmediately();
            
            float applyValue = dashForce;

            _velocityAdder.SetAddVelocity(targetDir * applyValue);
            
            try
            {
                _dashTween?.Kill();
                _dashTween = DOTween.To(() => applyValue, x =>
                {
                    applyValue = x;
                    _velocityAdder.SetAddVelocity(targetDir * applyValue);
                }, 0f, dashDuration).SetEase(dashEase);
                await _dashTween.ToUniTask(cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _velocityAdder.SetAddVelocity(targetDir * applyValue);
            }
        }
    }
}
