﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    public const float AVOID_MARGIN = 4.0f;
    public const float MAX_SPEED = 20.0f;
    public const float MAX_ACCELERATION = 40.0f;
    public const float DRAG = 0.1f;

    public GameObject mainCharacterGameObject;
    public GameObject secondaryCharacterGameObject;

    private BlendedMovement Blended { get; set; }
    private PriorityMovement Priority { get; set; }

    private List<SecondaryCharacterController> flockControllers;

	// Use this for initialization
	void Start () 
	{
	    var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

	    this.flockControllers = this.CloneSecondaryCharacters(this.secondaryCharacterGameObject, 20, obstacles);
        this.flockControllers.Add(this.secondaryCharacterGameObject.GetComponent<SecondaryCharacterController>());

        //LINQ expression with a lambda function, returns an array with the DynamicCharacter for each secondary character controler
        var characters = this.flockControllers.Select(cc => cc.character).ToList();
        //add the character corresponding to the main character

        //initialize all secondary characters
	    foreach (var characterController in this.flockControllers)
	    {
            characterController.InitializeMovement(obstacles, characters);
	    }
	}

    private List<SecondaryCharacterController> CloneSecondaryCharacters(GameObject objectToClone,int numberOfCharacters, GameObject[] obstacles)
    {
        var characters = new List<SecondaryCharacterController>();
        for (int i = 0; i < numberOfCharacters; i++)
        {
            var clone = GameObject.Instantiate(objectToClone);
            var characterController = clone.GetComponent<SecondaryCharacterController>();
            characterController.character.KinematicData.Position = this.GenerateRandomClearPosition(obstacles);
            
            characters.Add(characterController);
        }

        return characters;
    }


    private Vector3 GenerateRandomClearPosition(GameObject[] obstacles)
    {
        Vector3 position = new Vector3();
        var ok = false;
        while (!ok)
        {
            ok = true;

            position = new Vector3(Random.Range(-X_WORLD_SIZE,X_WORLD_SIZE), 0, Random.Range(-Z_WORLD_SIZE,Z_WORLD_SIZE));

            foreach (var obstacle in obstacles)
            {
                var distance = (position - obstacle.transform.position).magnitude;

                //assuming obstacle is a sphere just to simplify the point selection
                if (distance < obstacle.transform.localScale.x + AVOID_MARGIN)
                {
                    ok = false;
                    break;
                }
            }
        }

        return position;
    }

	

    
}
