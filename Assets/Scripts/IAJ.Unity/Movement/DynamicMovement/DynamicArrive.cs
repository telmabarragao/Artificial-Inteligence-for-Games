using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicArrive : DynamicVelocityMatch
	{
		public override string Name
		{
			get { return "Arrive"; }
		}

		public float maxSpeed { get; set; }
		public float targetSpeed { get; set; }
		public float stopRadius { get; set; }
		public float slowRadius { get; set; }

		public DynamicArrive() 
		{
			this.maxSpeed = 10.0f;
			this.stopRadius = 1;
			this.slowRadius = 5;
		}

		public override MovementOutput GetMovement()
		{

			Vector3 direction = this.Target.Position - this.Character.Position;

			float distance = direction.magnitude;
			if (distance < stopRadius) {
				targetSpeed = 0;
			} else if (distance > slowRadius) {
				targetSpeed = maxSpeed;
			}else{
				targetSpeed = maxSpeed *(distance/slowRadius);
			}

			this.Character.velocity = direction.normalized * targetSpeed;

			return base.GetMovement ();


		}
	}
}
