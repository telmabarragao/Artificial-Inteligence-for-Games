using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicCohesion : DynamicArrive 
	{

		public override string Name
		{
			get { return "Cohesion"; }
		}

		//public Character character { get; set; }
		public List<DynamicCharacter> flock { get; set; }
		public float radius { get; set; }
		public float fanAngle { get; set; }

		public DynamicCohesion() 
		{
			this.radius = 5;
			this.fanAngle = 10;
		}

		public override MovementOutput GetMovement()
		{

			Vector3 massCenter = new Vector3 (0,0,0);
			int closeBoids = 0;
			foreach(DynamicCharacter boid in flock)
			{
				if (this.Character != boid.KinematicData)
				{
					Vector3 direction = boid.KinematicData.Position - this.Character.Position;

					if (direction.magnitude <= this.radius)
					{
						float angleDifference = Vector3.Angle (this.Character.velocity, direction);

						if (Mathf.Abs(angleDifference) <= this.fanAngle)
						{
							massCenter += boid.KinematicData.Position;
							closeBoids++;
						}
					}
				}
			}

			if (closeBoids == 0)
				return new MovementOutput ();

			massCenter /= closeBoids;
			this.Target.Position = massCenter;

			return base.GetMovement ();
		}

	}
}