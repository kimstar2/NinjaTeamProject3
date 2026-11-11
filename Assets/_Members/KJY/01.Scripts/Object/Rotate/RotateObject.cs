using UnityEngine;

namespace _Members.KJY._01.Scripts.Object.Rotate
{
    public abstract class RotateObject : MonoBehaviour ,IXRotatable , IYRotatable , IZRotatable
    {
        protected float X => transform.localEulerAngles.x;
        protected float Y => transform.localEulerAngles.y;
        protected float Z => transform.localEulerAngles.z;
        
        
        public virtual void SetXAngle(float xAngle)
            => transform.localEulerAngles = new Vector3(xAngle, Y, Z);

        public virtual void SetYAngle(float yAngle) 
            => transform.localEulerAngles = new Vector3(X, yAngle, Z);

        public virtual void SetZAngle(float zAngle) 
            => transform.localEulerAngles = new Vector3(X, Y, zAngle);
    }
}