using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicVelocityMatch
    {
        public float MaxSpeed { get; set; }

        public float TargetRadius { get; set; }

        public float SlowRadius { get; set; }

        public override string Name
        {
            get { return "Arrive"; }
        }

        public DynamicArrive()
        {
            this.TimeToTargetSpeed = 0.1f;
            this.TargetRadius = 0.25f;
            this.SlowRadius = 1f;
        }
        
        
        public override MovementOutput GetMovement()
        {
            float targetSpeed;
            MovementOutput output = new MovementOutput();

            var direction = this.Target.position - this.Character.position;
            var distance = direction.magnitude;

            if (distance < this.TargetRadius)
            {
                targetSpeed = 0.0f;
                direction = this.Character.position;
            }
            else if (distance > this.SlowRadius)
            {
                //maximum speed
                targetSpeed = this.MaxSpeed;
            }
            else
            {
                targetSpeed = this.MaxSpeed*distance/this.SlowRadius;
            }

            this.TargetVelocity.velocity = direction.normalized * targetSpeed;

            return base.GetMovement();
        }
    }
}
