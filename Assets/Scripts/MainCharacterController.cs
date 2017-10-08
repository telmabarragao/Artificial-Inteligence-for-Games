using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using System.Collections.Generic;

public class MainCharacterController : MonoBehaviour {

    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_ACCELERATION = 40.0f;
    private const float MAX_SPEED = 20.0f;
    private const float DRAG = 0.1f;
	private const float AVOID_MARGIN = 20.0f;
	private const float MAX_LOOK_AHEAD = 50.0f;

    
    


    public KeyCode stopKey = KeyCode.S;
    public KeyCode priorityKey = KeyCode.P;
    public KeyCode blendedKey = KeyCode.B;

    public GameObject movementText;
    public DynamicCharacter character;

    public PriorityMovement priorityMovement;
    public BlendedMovement blendedMovement;

    private Text movementTextText;

    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(this.gameObject);
        this.movementTextText = this.movementText.GetComponent<Text>();

        this.priorityMovement = new PriorityMovement
        {
            Character = this.character.KinematicData
        };

        this.blendedMovement = new BlendedMovement
        {
            Character = this.character.KinematicData
        };
    }

    // Use this for initialization
    void Start ()
    {
    }

    public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
    {
        foreach (var obstacle in obstacles)
        {
            //TODO: add your AvoidObstacle movement here
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {
                MaxAcceleration = MAX_ACCELERATION,
                AvoidMargin = AVOID_MARGIN,
                MaxLookAhead = MAX_LOOK_AHEAD,
                Character = this.character.KinematicData,
                DebugColor = Color.magenta
            };
            this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 5.0f));
            this.priorityMovement.Movements.Add(avoidObstacleMovement);
        }

        foreach (var otherCharacter in characters)
        {
            if (otherCharacter != this.character)
            {
                //TODO: add your AvoidCharacter movement here
                var avoidCharacter = new DynamicAvoidCharacter(otherCharacter.KinematicData)
                {
                    Character = this.character.KinematicData,
                    MaxAcceleration = MAX_ACCELERATION,
                    AvoidMargin = AVOID_MARGIN,
                    DebugColor = Color.cyan
                };

                this.priorityMovement.Movements.Add(avoidCharacter);
            }
        }

        /*
         * TODO: add your wander behaviour here!*/
        var wander = new DynamicWander
        {
            MaxAcceleration = MAX_ACCELERATION,
            WanderOffset = 0,
            WanderRadius = 2,
            WanderRate = 5,
            Character = this.character.KinematicData,
            DebugColor = Color.yellow
        };

        this.priorityMovement.Movements.Add(wander);
        this.blendedMovement.Movements.Add(new MovementWithWeight(wander,1));

        this.character.Movement = this.blendedMovement;
    }


    void Update()
    {
        if (Input.GetKeyDown(this.stopKey))
        {
            this.character.Movement = null;
        }
        else if (Input.GetKeyDown(this.blendedKey))
        {
            this.character.Movement = this.blendedMovement;
        }
        else if (Input.GetKeyDown(this.priorityKey))
        {
            this.character.Movement = this.priorityMovement;
        }

        this.UpdateMovingGameObject();
        this.UpdateMovementText();
    }

    void OnDrawGizmos()
    {
        //TODO: this code is not working, try to figure it out
        if(this.character != null && this.character.Movement!=null)
        {
			if (this.character.Movement is PriorityMovement && this.priorityMovement.LastMovementPerformed is DynamicWander) {
				var wander = this.priorityMovement.LastMovementPerformed as DynamicWander;
				if (wander != null) {
					Gizmos.color = Color.blue;
					Gizmos.DrawWireSphere (this.character.KinematicData.Position, wander.WanderRadius);
				}
			}
        }
    }

    private void UpdateMovingGameObject()
    {
        if (this.character.Movement != null)
        {
            this.character.Update();
            this.character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
        }
    }

    private void UpdateMovementText()
    {
        if (this.character.Movement == null)
        {
            this.movementTextText.text = this.name + " Movement: Stationary";
        }
        else
        {
            this.movementTextText.text = this.name + " Movement: " + this.character.Movement.Name;
        }
    } 

}
