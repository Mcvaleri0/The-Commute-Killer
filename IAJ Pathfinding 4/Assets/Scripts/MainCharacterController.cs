using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Pathfinding.Path;

public class MainCharacterController : MonoBehaviour {

    private const float MAX_ACCELERATION = 40.0f;
    private const float MAX_SPEED = 20.0f;
    private const float STOP_RADIUS = 0.75f;
    private const float SLOW_RADIUS = 1.5f;
    private const float PATH_OFFSET = 1.0f;

    public bool Stopped { get; private set; }

    public DynamicCharacter Character;
    
    private DynamicFollowPath FollowPath;


    //early initialization
    void Awake()
    {
        this.Character = new DynamicCharacter(this.gameObject);
    }


    // Use this for initialization
    void Start()
    {
    }


    public void InitializeMovement(Path path)
    {
        FollowPath = new DynamicFollowPath()
        {
            Path = path,
            PathOffset = PATH_OFFSET,
            MaxSpeed = MAX_SPEED,
            StopRadius = STOP_RADIUS,
            SlowRadius = SLOW_RADIUS,
            Character = this.Character.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };
        Character.Movement = FollowPath;

        Stopped = false;
    }


    public void InitializePosition(Vector3 position)
    {
        Character.KinematicData.Position = position;
        Character.Movement = null;
        Stopped = true;
    }


    void Update()
    {
        this.UpdateMovingGameObject();
    }


    void OnDrawGizmos()
    {
    }


    private void UpdateMovingGameObject()
    {
        if (this.Character.Movement != null)
        {
            this.Character.Update();
        }
    }
}
