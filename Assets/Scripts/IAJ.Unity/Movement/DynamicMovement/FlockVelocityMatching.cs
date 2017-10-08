using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class FlockVelocityMatching : DynamicVelocityMatch
	{
		public List<DynamicCharacter> Flock;
		public float Radius;
		public float FanAngle;


		public FlockVelocityMatching ()
		{
		}

		public override MovementOutput GetMovement() {

			Vector3 averageVelocity = new Vector3 ();
			int closeBoids = 0;
			foreach (DynamicCharacter boid in this.Flock) {
				if (boid.KinematicData != this.Character) {
					
					Vector3 direction = boid.KinematicData.Position - this.Character.Position;

					if (direction.magnitude < this.Radius) {
						float angle = Vector3.Angle (this.Character.velocity, direction);


						if (Mathf.Abs (angle) <= FanAngle) {
							averageVelocity += boid.KinematicData.velocity;
							closeBoids++;
						}
					}
				
				}

			
			}

			if (closeBoids == 0)
				return new MovementOutput ();

			averageVelocity /= closeBoids;

			this.Target.velocity = averageVelocity;

			return base.GetMovement ();
		}
	}
}

