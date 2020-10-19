using UnityEngine;

namespace GreatClock.Common.Tweens {

	[TweenerData(-959, "RectTransform", "AnchoredPosition3D", "AnchoredPosition3D")]
	public class RectTransformAnchoredPosition3DTweener : TweenerBase<RectTransform, Vector3, Vector3Tween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.anchoredPosition3D; }
		}
		protected override void OnApply(Vector3 value) {
			if (Target != null) { Target.anchoredPosition3D = value; }
		}
	}

	[TweenerData(-958, "RectTransform", "AnchoredPosition", "AnchoredPosition")]
	public class RectTransformAnchoredPositionTweener : TweenerBase<RectTransform, Vector2, Vector2Tween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.anchoredPosition; }
		}
		protected override void OnApply(Vector2 value) {
			if (Target != null) { Target.anchoredPosition = value; }
		}
	}

	[TweenerData(-957, "RectTransform", "RectTransformLeft", "Left")]
	public class RectTransformLeftTweener : TweenerBase<RectTransform, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.offsetMin.x; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.offsetMin = new Vector2(value, Target.offsetMin.y); }
		}
	}

	[TweenerData(-956, "RectTransform", "RectTransformRight", "Right")]
	public class RectTransformRightTweener : TweenerBase<RectTransform, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = -Target.offsetMax.x; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.offsetMax = new Vector2(-value, Target.offsetMax.y); }
		}
	}

	[TweenerData(-955, "RectTransform", "RectTransformTop", "Top")]
	public class RectTransformTopTweener : TweenerBase<RectTransform, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = -Target.offsetMax.y; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.offsetMax = new Vector2(Target.offsetMax.x, -value); }
		}
	}

	[TweenerData(-954, "RectTransform", "RectTransformBottom", "Bottom")]
	public class RectTransformBottomTweener : TweenerBase<RectTransform, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.offsetMin.y; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.offsetMin = new Vector2(Target.offsetMin.x, value); }
		}
	}

	[TweenerData(-953, "RectTransform", "RectTransformWidth", "Width")]
	public class RectTransformWidthTweener : TweenerBase<RectTransform, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.sizeDelta.x; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.sizeDelta = new Vector2(value, Target.sizeDelta.y); }
		}
	}

	[TweenerData(-952, "RectTransform", "RectTransformHeight", "Height")]
	public class RectTransformHeightTweener : TweenerBase<RectTransform, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.sizeDelta.y; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.sizeDelta = new Vector2(Target.sizeDelta.x, value); }
		}
	}

	[TweenerData(-951, "RectTransform", "RectTransformSize", "Size")]
	public class RectTransformSizeTweener : TweenerBase<RectTransform, Vector2, Vector2Tween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.sizeDelta; }
		}
		protected override void OnApply(Vector2 value) {
			if (Target != null) { Target.sizeDelta = value; }
		}
	}

}