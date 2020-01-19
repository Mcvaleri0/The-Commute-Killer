using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicCharacter
    {
        public GameObject GameObject { get; protected set; }

        public KinematicData KinematicData { get; protected set; }

        private DynamicMovement movement;

        public CharacterController Controller;


        public DynamicMovement Movement
        {
            get { return this.movement; }
            set
            {
                this.movement = value;
                if (this.movement != null) this.movement.Character = this.KinematicData;
            }
        }
        public float Drag { get; set; }
        public float MaxSpeed { get; set; }


        public DynamicCharacter(GameObject gameObject)
        {
            this.KinematicData = new KinematicData(new StaticData(gameObject.transform.position));
            this.KinematicData.velocity = new Vector3(1, 1, 1);
            this.GameObject = gameObject;
        }


        // Update is called once per frame
        public void Update()
        {

            Vector3 motion = Vector3.zero;

            if (this.Movement != null)
            {
                this.KinematicData.position = this.GameObject.transform.position;

                MovementOutput steering = this.Movement.GetMovement();

                this.KinematicData.Integrate(steering, this.Drag, Time.deltaTime);
                this.KinematicData.SetOrientationFromVelocity();
                this.KinematicData.TrimMaxSpeed(this.MaxSpeed);

                motion = this.KinematicData.position - this.GameObject.transform.position;

                this.GameObject.transform.rotation = Quaternion.AngleAxis(this.KinematicData.orientation * MathConstants.MATH_180_PI, Vector3.up);
            }

            // apply gravity -> has to take in consideration the delta time
            motion += (Physics.gravity * Time.deltaTime * Time.deltaTime);

            // move controller -> has to take in consideration the delta time
            this.Controller.Move(motion);
        }
    
    
        public bool MovementPossible()
        {
            return this.Movement.Possible();
        }
    }
}
