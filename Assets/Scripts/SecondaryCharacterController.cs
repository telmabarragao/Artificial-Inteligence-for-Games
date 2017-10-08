<<<<<<< HEAD
﻿using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCharacterController : MonoBehaviour {

	public const float X_WORLD_SIZE = 100;
	public const float Z_WORLD_SIZE = 65.0f;
    private const float MAX_ACCELERATION = 100.0f;
    private const float MAX_SPEED = 10.0f;
	private const float DRAG = 0.1f;
	private const float AVOID_MARGIN = 1000000000.0f;
	private const float MAX_LOOK_AHEAD = 100.0f;
	private const float SEPARATION_FACTOR = 500.0f;
	private const float RADIUS_SEPARATION = 20.0f;
	private const float RADIUS_COHESION = 20.0f;
	private const float RADIUS_VELOCITY_MATCHING = 10.0f;
	private const float FAN_ANGLE = 45.0f;

    public DynamicCharacter character;
	public BlendedMovement blendedMovement;

	public KinematicData target;
    GameObject[] obs;
    List<DynamicCharacter> chars;
    private bool mouseActive = false;


    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(gameObject);

		this.blendedMovement = new BlendedMovement
		{
			Character = this.character.KinematicData
		};

		character.Movement = this.blendedMovement;

    }

    // Use this for initialization
    void Start()
    {
    }

    public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
    {
		target = new KinematicData();

        foreach (var obstacle in obstacles)
        {

            //TODO: add your AvoidObstacle movement here
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {
                MaxAcceleration = MAX_ACCELERATION,
                MaxLookAhead = MAX_LOOK_AHEAD,
                AvoidMargin = AVOID_MARGIN,
                Character = this.character.KinematicData,
                DebugColor = Color.magenta
            };
			this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 10000000000000000000000.0f));
        }

		//foreach (var boid in characters)
		//{

			//TODO: add your AvoidObstacle movement here
			var separationMovement = new DynamicSeparation()
			{
				MaxAcceleration = MAX_ACCELERATION,
				Character = this.character.KinematicData,
				Radius = RADIUS_SEPARATION,
				Flock = characters,
				SeparationFactor = SEPARATION_FACTOR
			};

			this.blendedMovement.Movements.Add(new MovementWithWeight(separationMovement, 5.0f));

			var cohesionMovement = new DynamicCohesion()
			{
				MaxAcceleration = MAX_ACCELERATION,
				Character = this.character.KinematicData,
				radius = RADIUS_COHESION,
				flock = characters,
				fanAngle = FAN_ANGLE,
				Target = target
			};

			this.blendedMovement.Movements.Add(new MovementWithWeight(cohesionMovement, 0.2f));

			var velocityMatchingMovement = new FlockVelocityMatching()
			{
				MaxAcceleration = MAX_ACCELERATION,
				Character = this.character.KinematicData,
				Radius = RADIUS_VELOCITY_MATCHING,
				Flock = characters,
				FanAngle = FAN_ANGLE,
				Target = target
			};

			this.blendedMovement.Movements.Add(new MovementWithWeight(velocityMatchingMovement, 0.5f));
		//}


        this.character.Movement = this.blendedMovement;
    }


    // Update is called once per frame
    void Update () {
		MovementWithWeight mouseSeek = null;
		if (Input.GetMouseButton(0))
		{
			target.Position = new Vector3((((Input.mousePosition.x * X_WORLD_SIZE) / Screen.width) - (X_WORLD_SIZE / 2)), 0, (((Input.mousePosition.y * Z_WORLD_SIZE) / Screen.height) - (Z_WORLD_SIZE / 2)));
			Debug.Log (target.Position);
			var seekToPointMovement = new DynamicArrive()
			{
				Character = this.character.KinematicData,
				MaxAcceleration = MAX_ACCELERATION,
				Target = target
			};
			mouseSeek = new MovementWithWeight (seekToPointMovement, 0.1f);
			this.blendedMovement.Movements.Add(mouseSeek);
		}

		UpdateMovingGameObject();

		this.blendedMovement.Movements.Remove(mouseSeek);
    }

    private void UpdateMovingGameObject()
    {
        if (character.Movement != null)
        {
            character.Update();
            character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
        }
    }
}
=======
﻿using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCharacterController : MonoBehaviour {

	public const float X_WORLD_SIZE = 50;
	public const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_ACCELERATION = 100.0f;
    private const float MAX_SPEED = 10.0f;
	private const float DRAG = 0.1f;
	private const float AVOID_MARGIN = 50.0f;
	private const float MAX_LOOK_AHEAD = 100.0f;
	private const float SEPARATION_FACTOR = 500.0f;
	private const float RADIUS_SEPARATION = 20.0f;
	private const float RADIUS_COHESION = 20.0f;
	private const float RADIUS_VELOCITY_MATCHING = 10.0f;
	private const float FAN_ANGLE = 45.0f;

    public DynamicCharacter character;
	public BlendedMovement blendedMovement;

	public KinematicData target;


    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(gameObject);

		this.blendedMovement = new BlendedMovement
		{
			Character = this.character.KinematicData
		};

		character.Movement = this.blendedMovement;

    }

    // Use this for initialization
    void Start()
    {
    }

    public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
    {
		target = new KinematicData();

        foreach (var obstacle in obstacles)
        {

            //TODO: add your AvoidObstacle movement here
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {
                MaxAcceleration = MAX_ACCELERATION,
                MaxLookAhead = MAX_LOOK_AHEAD,
                AvoidMargin = AVOID_MARGIN,
                Character = this.character.KinematicData,
                DebugColor = Color.magenta
            };

			this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 10000000000000000000000.0f));
        }

		//foreach (var boid in characters)
		//{

			//TODO: add your AvoidObstacle movement here
			var separationMovement = new DynamicSeparation()
			{
				MaxAcceleration = MAX_ACCELERATION,
				Character = this.character.KinematicData,
				Radius = RADIUS_SEPARATION,
				Flock = characters,
				SeparationFactor = SEPARATION_FACTOR
			};

			this.blendedMovement.Movements.Add(new MovementWithWeight(separationMovement, 5.0f));

			var cohesionMovement = new DynamicCohesion()
			{
				MaxAcceleration = MAX_ACCELERATION,
				Character = this.character.KinematicData,
				radius = RADIUS_COHESION,
				flock = characters,
				fanAngle = FAN_ANGLE,
				Target = target
			};

			this.blendedMovement.Movements.Add(new MovementWithWeight(cohesionMovement, 0.2f));

			var velocityMatchingMovement = new FlockVelocityMatching()
			{
				MaxAcceleration = MAX_ACCELERATION,
				Character = this.character.KinematicData,
				Radius = RADIUS_VELOCITY_MATCHING,
				Flock = characters,
				FanAngle = FAN_ANGLE,
				Target = target
			};

			this.blendedMovement.Movements.Add(new MovementWithWeight(velocityMatchingMovement, 0.5f));
		//}


        this.character.Movement = this.blendedMovement;
    }


    // Update is called once per frame
    void Update () {
		MovementWithWeight mouseSeek = null;
		if (Input.GetMouseButton(0))
		{
			target.Position = new Vector3((((Input.mousePosition.x * X_WORLD_SIZE * 2) / Screen.width) - (X_WORLD_SIZE)), 0, (((Input.mousePosition.y * Z_WORLD_SIZE * 2) / Screen.height) - (Z_WORLD_SIZE)));
			Debug.Log (target.Position);
			var seekToPointMovement = new DynamicArrive()
			{
				Character = this.character.KinematicData,
				MaxAcceleration = MAX_ACCELERATION,
				Target = target
			};
			mouseSeek = new MovementWithWeight (seekToPointMovement, 0.1f);
			this.blendedMovement.Movements.Add(mouseSeek);
		}

		UpdateMovingGameObject();

		this.blendedMovement.Movements.Remove(mouseSeek);
    }

    private void UpdateMovingGameObject()
    {
        if (character.Movement != null)
        {
            character.Update();
            character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
        }
    }
}
>>>>>>> e04971abdf16c8e5f346d8cc0d53eecff6e2acbe
