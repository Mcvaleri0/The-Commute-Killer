using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;


namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    public class DynamicArrive : DynamicVelocityMatch
    {
        public float MaxSpeed { get; set; }
        public float StopRadius { get; set; }
        public float SlowRadius { get; set; }
        public KinematicData DesiredTarget { get; set; }

        public GameObject DebugTarget { get; set; }


        public DynamicArrive()
        {
            this.Target = new KinematicData();
            this.DebugTarget = new GameObject();
        }

        public override string Name
        {
            get { return "Arrive"; }
        }


        public override Vector3 GetDesired()
        {
            return DesiredTarget.Position;
        }


        public override MovementOutput GetMovement()
        {
            var Direction = this.DesiredTarget.Position - this.Character.Position;
            var Distance  = Mathf.Sqrt(Direction.magnitude);

            var DesiredSpeed = 0.0f;
            if (Distance > this.SlowRadius)
            {
                DesiredSpeed = this.MaxSpeed;
            } 
            else if (Distance > StopRadius)
            {
                DesiredSpeed = (float) (this.MaxSpeed * (Distance / SlowRadius));
            }

            Target.velocity = Direction.normalized * DesiredSpeed;

            if (this.DebugTarget != null)
            {
                this.DebugTarget.transform.position = this.Target.Position;
            }

            return base.GetMovement();
        }
    }
}
