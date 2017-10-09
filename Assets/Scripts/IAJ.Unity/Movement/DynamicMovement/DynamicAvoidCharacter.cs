using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidCharacter : DynamicSeek
	{
		private KinematicData Other;

		public float AvoidMargin;

		public DynamicAvoidCharacter (KinematicData target)
		{
			this.Other = target;
		}

		public override MovementOutput GetMovement() {
			MovementOutput output = new MovementOutput ();

			Vector3 deltaPos = this.Other.Position - this.Character.Position;
			Vector3 deltaVel = this.Other.velocity - this.Character.velocity;
			float deltaSpeed = deltaVel.magnitude;

			if (deltaSpeed == 0)
				return output;

			float timeToClosest = -Vector3.Dot (deltaPos, deltaVel) / (deltaSpeed * deltaSpeed);

			if (timeToClosest > 3)
				return output;

			Vector3 futureDeltaPos = deltaPos + deltaVel * timeToClosest;
			float futureDistance = futureDeltaPos.magnitude;

			if (futureDistance > 2 * this.AvoidMargin)
				return new MovementOutput ();

			if (futureDistance <= 0 || deltaPos.magnitude < 2 * this.AvoidMargin)
				output.linear = this.Character.Position - this.Other.Position;
			else
				output.linear = futureDeltaPos * -1;

			output.linear = output.linear.normalized * this.MaxAcceleration;
			return output;
		}
	}
}

