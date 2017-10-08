using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicSeparation : DynamicMovement
	{

		public override string Name
		{
			get { return "Seek"; }
		}

		public List<DynamicCharacter> Flock;
		public float Radius;
		public float SeparationFactor;



		public DynamicSeparation ()
		{
		}

		public override MovementOutput GetMovement() {

			MovementOutput output = new MovementOutput ();

			foreach(DynamicCharacter boid in Flock) {
				if (boid.KinematicData != this.Character) {
					Vector3 direction = this.Character.Position - boid.KinematicData.Position;
					float distance = direction.magnitude;
					if (distance < Radius) {
						float separationStrength = Mathf.Min (SeparationFactor / (distance * distance), this.MaxAcceleration);
						direction.Normalize ();
						output.linear += direction * separationStrength;
					}
				}			
			}

			if (output.linear.magnitude > this.MaxAcceleration) {
				output.linear.Normalize ();
				output.linear *= this.MaxAcceleration;
			}

			return output;
		}
	}
}

