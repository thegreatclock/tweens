using UnityEngine;

namespace GreatClock.Common.Tweens {

	[TweenerData(-999, "Transform", "Position", "Position")]
	public class PositionTweener : TweenerBase<Transform, Vector3, Vector3Tween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.localPosition; }
		}
		protected override void OnApply(Vector3 value) {
			if (Target != null) { Target.localPosition = value; }
		}
	}

	[TweenerData(-998, "Transform", "PositionX", "Position.x")]
	public class PositionXTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.px; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.px = value; }
		}
	}

	[TweenerData(-997, "Transform", "PositionY", "Position.y")]
	public class PositionYTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.py; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.py = value; }
		}
	}

	[TweenerData(-996, "Transform", "PositionZ", "Position.z")]
	public class PositionZTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.pz; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.pz = value; }
		}
	}

	[TweenerData(-989, "Transform", "Rotation", "Rotation")]
	public class RotationTweener : TweenerBase<Transform, Quaternion, QuaternionTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.localRotation; }
		}
		protected override void OnApply(Quaternion value) {
			if (Target != null) { Target.localRotation = value; }
		}
	}

	[TweenerData(-988, "Transform", "RotationEuler", "EulerAngles")]
	public class RotationEulerTweener : TweenerBase<Transform, Vector3, Vector3Tween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.localEulerAngles; }
		}
		protected override void OnApply(Vector3 value) {
			if (Target != null) { Target.localEulerAngles = value; }
		}
	}

	[TweenerData(-987, "Transform", "RotationX", "Rotation.x")]
	public class RotationXTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.rx; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.rx = value; }
		}
	}

	[TweenerData(-986, "Transform", "RotationY", "Rotation.y")]
	public class RotationYTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.ry; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.ry = value; }
		}
	}

	[TweenerData(-985, "Transform", "RotationZ", "Rotation.z")]
	public class RotationZTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.rz; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.rz = value; }
		}
	}

	[TweenerData(-979, "Transform", "Scale", "Scale")]
	public class ScaleTweener : TweenerBase<Transform, Vector3, Vector3Tween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.localScale; }
		}
		protected override void OnApply(Vector3 value) {
			if (Target != null) { Target.localScale = value; }
		}
	}

	[TweenerData(-978, "Transform", "ScaleX", "Scale.x")]
	public class ScaleXTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.sx; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.sx = value; }
		}
	}

	[TweenerData(-977, "Transform", "ScaleY", "Scale.y")]
	public class ScaleYTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.sy; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.sy = value; }
		}
	}

	[TweenerData(-976, "Transform", "ScaleZ", "Scale.z")]
	public class ScaleZTweener : TweenerBase<AxesTransformSupport, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.sz; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.sz = value; }
		}
	}

}
