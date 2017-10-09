using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidObstacle : DynamicSeek
	{
		private Collider Collider;
		public float MaxLookAhead;
		public float AvoidMargin;

		public DynamicAvoidObstacle (GameObject obstacle)
		{
			this.Collider = obstacle.GetComponent<Collider> ();
			this.Target = new KinematicData ();
		}
	
		public override MovementOutput GetMovement() {
			Ray ray;
			Ray whisker1, whisker2;
			if (this.Character.velocity.magnitude == 0) {
				ray = new Ray (this.Character.Position, new Vector3(0,0,1));
				whisker1 = new Ray(this.Character.Position, (new Vector3(1,0,1)).normalized);
				whisker2 = new Ray (this.Character.Position, (new Vector3(-1,0,1)).normalized);
			} else {
				ray = new Ray(this.Character.Position, this.Character.velocity.normalized);
				whisker1 = new Ray(this.Character.Position, Quaternion.AngleAxis(45, Vector3.up) * this.Character.velocity.normalized);
				whisker2 = new Ray (this.Character.Position, Quaternion.AngleAxis(-45, Vector3.up) * this.Character.velocity.normalized);
			}
			RaycastHit hit;
			if (!this.Collider.Raycast (ray, out hit, this.MaxLookAhead)) {
				if (!this.Collider.Raycast (whisker1, out hit, 5)) {
					if (!this.Collider.Raycast (whisker2, out hit, 5)) {
						this.Output.Clear ();
						return this.Output;
					}
				}
			}

			this.Target.Position = hit.point + hit.normal * this.AvoidMargin;


			return base.GetMovement();
			}
	}
}

