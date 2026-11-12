using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Members.JJW._01.Code
{
    [Serializable]
    [FeedbackPath("JJW/Map Color")]
    [FeedbackHelp("맵 머티리얼의 Float 값을 조절합니다. 파괴된 렌더러는 건너뜁니다.")]
    public class MMF_MapColorJJW : MMF_MaterialSetProperty
    {
        protected override void SetFloat(float newFloat)
        {
            SetRendererFloat(TargetRenderer, MaterialID, newFloat);
            if (ExtraTargetRenderers == null) return;

            foreach (ExtraRendererData data in ExtraTargetRenderers)
            {
                if (data != null) SetRendererFloat(data.TargetRenderer, data.MaterialID, newFloat);
            }
        }

        private void SetRendererFloat(Renderer target, int materialIndex, float value)
        {
            // 씬 종료 시에는 대상이 MMF Player보다 먼저 파괴될 수 있다.
            if (target == null) return;

            Material[] materials = target.materials;
            if (materialIndex < 0 || materialIndex >= materials.Length) return;

            Material material = materials[materialIndex];
            if (material != null && material.HasProperty(_propertyID))
                material.SetFloat(_propertyID, value);
        }
    }
}
