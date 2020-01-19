//class adapted from the HRVO library http://gamma.cs.unc.edu/HRVO/
//adapted to IAJ classes by João Dias



using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;



namespace Assets.Scripts.IAJ.Unity.Movement.VO
{
    public class RVOMovement : DynamicMovement.DynamicVelocityMatch
    {
        public override string Name
        {
            get { return "RVO"; }
        }

        //protected List<KinematicData> Characters { get; set; }
        //protected List<StaticData> Obstacles { get; set; }

        private ObstaclesDetector Detector { get; set; }

        public float CharacterSize { get; set; }
        public float IgnoreDistance { get; set; }
        public float MaxSpeed { get; set; }
        public float ObstacleSize { get; set; }

        private const int NUM_SAMPLES = 500;
        /// <summary>
        ///     Weight that defines the importance of avoiding a potencial collision
        /// </summary>
        private const float WC = 50f;
        private const float WO = 50f;
        private float OBSTACLE_SIZE_SQR;

        protected DynamicMovement.DynamicMovement DesiredMovement { get; set; }


        public RVOMovement(DynamicMovement.DynamicMovement goalMovement, ObstaclesDetector Detector /*List<KinematicData> movingCharacters, List<StaticData> obstacles*/)
        {
            DesiredMovement = goalMovement;
            this.Detector = Detector;
            //Characters      = movingCharacters;
            //Obstacles       = obstacles;
            base.Target     = goalMovement.Target;
        }


        #region === Movement ===
        public override MovementOutput GetMovement()
        {
            //OBSTACLE_SIZE_SQR = ObstacleSize * ObstacleSize;

            // --- Calculate desired velocity ---
            MovementOutput desiredOutput = this.DesiredMovement.GetMovement();

            // if movementOutput is accelarating we need to convert it to velocity
            Vector3 desiredVelocity = this.Character.velocity + desiredOutput.linear;

            // trim velocity if bigger than max
            if (desiredVelocity.magnitude > this.MaxSpeed)
            {
                desiredVelocity = desiredVelocity.normalized * this.MaxSpeed;
            }

            // --- Generate Samples ---
            // always consider the desired velocity as a sample
            var Samples = new List<Vector3>
            {
                desiredVelocity
            };

            Samples = Samples.Concat(genSamples(NUM_SAMPLES / 2, MathConstants.MATH_2PI)).ToList();
            Samples = Samples.Concat(genLineSamples(desiredVelocity, NUM_SAMPLES / 4)).ToList();
            Samples = Samples.Concat(genRangeSamples(desiredVelocity, NUM_SAMPLES / 4, MathConstants.MATH_PI_4)).ToList();

            // --- Evaluate and get best sample ---
            base.TargetVelocity.velocity = GetBestSample(desiredVelocity, Samples);
            //base.TargetVelocity.velocity = desiredVelocity;

            // --- let the base class take care of achieving the final velocity
            return base.GetMovement();
        }


        public override bool Possible()
        {
            return this.DesiredMovement.Possible();
        }

        #endregion


        #region === Samples ===

        private Vector3 GetBestSample(Vector3 desiredVelocity, List<Vector3> samples)
        {
            // --- if all samples suck then the best option is to stay still ---
            Vector3 bestSample   = new Vector3();
            float minimumPenalty = Mathf.Infinity;

            foreach (Vector3 sample in samples)
            {
                // --- penalty based on the distance to desired velocity --
                float distancePenalty     = (desiredVelocity - sample).magnitude;
                float enemyTimePenalty    = 0f;
                float obstacleTimePenalty = 0f;

                if(distancePenalty > minimumPenalty)
                {
                    continue;
                }

                foreach (var enemy in this.Detector.Characters.Values /*this.Characters*/)
                {
                    if(sample == desiredVelocity)
                    {
                        enemyTimePenalty = 0f;
                    }

                    var timePenalty = characterPenalty(enemy, sample);

                    // --- supostamente isto pode ser otimizado ---
                    if (timePenalty > enemyTimePenalty)
                    {
                        enemyTimePenalty = timePenalty;
                    }

                    if (enemyTimePenalty >= minimumPenalty)
                    {
                        break;
                    }
                    // --------------------------------------------
                }

                if (distancePenalty + enemyTimePenalty >= minimumPenalty)
                {
                    continue;
                }


                foreach (var obstacle in this.Detector.Obstacles.Values)
                {
                    var timePenalty = obstaclePenalty(obstacle, sample);

                    // --- supostamente isto pode ser otimizado ---
                    if (timePenalty > obstacleTimePenalty)
                    {
                        obstacleTimePenalty = timePenalty;
                    }

                    if (distancePenalty + enemyTimePenalty + obstacleTimePenalty > minimumPenalty)
                    {
                        break;
                    }
                    // --------------------------------------------
                }

                var penalty = distancePenalty + enemyTimePenalty  + obstacleTimePenalty;

                // --- supostamente isto pode ser otimizado ---
                if (penalty < minimumPenalty)
                {
                    minimumPenalty = penalty;
                    bestSample     = sample;
                }

                if (penalty == 0)
                {
                    break;
                }
                // --------------------------------------------
            }
            
            return bestSample;
        }


        private List<Vector3> genSamples(int count, float maxAngle)
        {
            List<Vector3> samples = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                // random angle between 0 and maxAngle
                float angle = RandomHelper.RandomBinomial(maxAngle);

                // random magnitude between 0 and maxSpeed
                float magnitude = RandomHelper.RandomBinomial(this.MaxSpeed);

                Vector3 velocitySample = MathHelper.ConvertOrientationToVector(angle) * magnitude;
                samples.Add(velocitySample);
            }

            return samples;
        }


        private List<Vector3> genRangeSamples(Vector3 desired, int count, float maxAngle)
        {
            List<Vector3> samples = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                // random angle between 0 and maxAngle
                float angle = RandomHelper.RandomBinomial(maxAngle);

                // random magnitude between 0 and maxSpeed
                float magnitude = this.MaxSpeed / 2 + RandomHelper.RandomBinomial(this.MaxSpeed / 2);

                Vector3 velocitySample = desired + MathHelper.ConvertOrientationToVector(angle) * desired.magnitude;
                samples.Add(velocitySample);
            }

            return samples;
        }


        private List<Vector3> genLineSamples(Vector3 desired, int count)
        {
            List<Vector3> samples = new List<Vector3>();

            // Direction of the desired velocity
            Vector3 direction = desired.normalized;

            for (int i = 0; i < count; i++)
            {
                // random magnitude between 0 and maxSpeed
                float magnitude = this.MaxSpeed / 2 + RandomHelper.RandomBinomial(this.MaxSpeed / 2);

                Vector3 velocitySample = direction * magnitude;
                samples.Add(velocitySample);
            }

            return samples;
        }

        #endregion

        #region === Penalties ===
        private float characterPenalty(KinematicData enemy, Vector3 sample)
        {
            float timePenalty = 0f; // default value for no collision

            Vector3 deltaP = enemy.position - this.Character.position;

            // --- we can safely ignore this if it's too far away ---
            // if magnitude is 0 then the enemy is the character itself
            if (deltaP.magnitude > this.IgnoreDistance)
            {
                return timePenalty;
            }

            // --- test the collision of the ray with the circle ---
            Vector3 rayVector = 2 * sample - this.Character.velocity - enemy.velocity;
            float tc = MathHelper.TimeToCollisionBetweenRayAndCircle(this.Character.position,
                                                                   rayVector,
                                                                   enemy.position,
                                                                   2*this.CharacterSize);

            if (tc > 0) // future collision
            {
                timePenalty = WC / tc;
            }
            else if (tc == 0)   // immediate collision
            {
                timePenalty = Mathf.Infinity;
            }

            return timePenalty;
        }


        private float obstaclePenalty(StaticData obstacle, Vector3 sample)
        {
            float timePenalty = 0f; // default value for no collision

            /* vector between character and center */
            Vector3 Vco = obstacle.position - Character.position;

            // --- we can safely ignore this if it's too far away ---
            if (Vco.magnitude > this.IgnoreDistance)
            {
                return timePenalty;
            }

            /* Distance between Character em center's projection in sampler vector */
            var normVcp = Vector3.Dot(sample, Vco) / sample.magnitude;

            /* Center's projection in sample vector */
            var P = Character.position +  sample.normalized * normVcp;

            /* Distance between center and it's projection*/
            var OPsqr = (obstacle.position - P).sqrMagnitude;

            /* Radius squared */
            var IOsqr = OBSTACLE_SIZE_SQR /*25.0f*/;

            /* if the distance between center and it's projection is less than the radius 
             * then there is a possible collisiion */
            if (OPsqr < IOsqr)
            {
                /* Distance between the Center's projection and the intersection point */
                var IP = Mathf.Sqrt(IOsqr + OPsqr);

                /* Distance between character and intersection */
                var CI = normVcp - IP;

                /* Imminent collision */
                if (sample.magnitude >= CI)
                {
                    timePenalty = WO;
                }
                /* There is no collison yet*/
                else
                {
                    timePenalty = (WO * MaxSpeed) / (CI - sample.magnitude);
                }
            }

            /* if the distance between center and it's projection is equal to the radius 
             * then there is an immidiate collisiion */
            else if (OPsqr == IOsqr)
            {
                timePenalty = Mathf.Infinity;
            }

            /* Otherwise the sanmple is safe */

            
            return timePenalty;
        }

        #endregion

    }
}
