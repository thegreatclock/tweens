using GreatClock.Common.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreatClock.Common.Tweens {

	[TweenerData(-799, "Rigidbody", "RigidbodyPosition", "Position")]
	public class RigidbodyPositionTweener : TweenerBase<Rigidbody, Vector3, Vector3Tween> {
		public RigidbodyPositionTweener() : base(GameUpdater.fixed_updater) { }
		private bool mParentInited = false;
		private Transform mParent;
		protected override void OnInit() {
			if (Target != null) { Current = Target.transform.localPosition; }
		}
		protected override void OnApply(Vector3 value) {
			if (Target == null) { return; }
			if (!mParentInited) {
				mParentInited = true;
				mParent = Target.transform.parent;
			}
			Target.MovePosition(mParent == null ? value : mParent.TransformPoint(value));
		}
		protected override void OnClear() {
			mParentInited = false;
			mParent = null;
		}
	}

	[TweenerData(-798, "Rigidbody", "RigidbodyRotation", "Rotation")]
	public class RigidbodyRotationTweener : TweenerBase<Rigidbody, Quaternion, QuaternionTween> {
		public RigidbodyRotationTweener() : base(GameUpdater.fixed_updater) { }
		private bool mParentInited = false;
		private Transform mParent;
		protected override void OnInit() {
			if (Target != null) { Current = Target.transform.localRotation; }
		}
		protected override void OnApply(Quaternion value) {
			if (Target == null) { return; }
			if (!mParentInited) {
				mParentInited = true;
				mParent = Target.transform.parent;
			}
			Quaternion rot = mParent != null ? mParent.rotation * value : value;
			Target.MoveRotation(rot);
		}
		protected override void OnClear() {
			mParentInited = false;
			mParent = null;
		}
	}

	[TweenerData(-797, "Rigidbody", "RigidbodyRotationEuler", "EulerAngles")]
	public class RigidbodyRotationEulerTweener : TweenerBase<Rigidbody, Vector3, Vector3Tween> {
		public RigidbodyRotationEulerTweener() : base(GameUpdater.fixed_updater) { }
		private bool mParentInited = false;
		private Transform mParent;
		protected override void OnInit() {
			if (Target != null) { Current = Target.transform.localEulerAngles; }
		}
		protected override void OnApply(Vector3 value) {
			if (Target == null) { return; }
			if (!mParentInited) {
				mParentInited = true;
				mParent = Target.transform.parent;
			}
			Quaternion rot = Quaternion.Euler(value);
			if (mParent != null) {
				rot = mParent.rotation * rot;
			}
			Target.MoveRotation(rot);
		}
		protected override void OnClear() {
			mParentInited = false;
			mParent = null;
		}
	}

	[TweenerData(-789, "Rigidbody2D", "Rigidbody2DPosition", "Position")]
	public class Rigidbody2DPositionTweener : TweenerBase<Rigidbody2D, Vector2, Vector2Tween> {
		public Rigidbody2DPositionTweener() : base(GameUpdater.fixed_updater) { }
		private bool mParentInited = false;
		private Transform mParent;
		protected override void OnInit() {
			if (Target != null) { Current = Target.transform.localPosition; }
		}
		protected override void OnApply(Vector2 value) {
			if (Target == null) { return; }
			if (!mParentInited) {
				mParentInited = true;
				mParent = Target.transform.parent;
			}
			Target.MovePosition(mParent == null ? value : (Vector2)mParent.TransformPoint(value));
		}
		protected override void OnClear() {
			mParentInited = false;
			mParent = null;
		}
	}

	[TweenerData(-788, "Rigidbody2D", "Rigidbody2DRotation", "Rotation")]
	public class Rigidbody2DRotationTweener : TweenerBase<Rigidbody2D, float, FloatTween> {
		public Rigidbody2DRotationTweener() : base(GameUpdater.fixed_updater) { }
		private bool mParentInited = false;
		private Transform mParent;
		protected override void OnInit() {
			if (Target != null) { Current = Target.transform.localEulerAngles.z; }
		}
		protected override void OnApply(float value) {
			if (Target == null) { return; }
			if (!mParentInited) {
				mParentInited = true;
				mParent = Target.transform.parent;
			}
			float angle = value;
			if (mParent != null) {
				Quaternion rot = mParent.rotation * Quaternion.Euler(0f, 0f, value);
				Vector3 dir = rot * Vector3.right;
				angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			}
			Target.MoveRotation(angle);
		}
		protected override void OnClear() {
			mParentInited = false;
			mParent = null;
		}
	}

}
