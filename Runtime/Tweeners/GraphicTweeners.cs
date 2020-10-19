using UnityEngine;
using UnityEngine.UI;

namespace GreatClock.Common.Tweens {

	[TweenerData(-899, "Graphic", "Alpha", "Alpha")]
	public class CanvasGroupAlphaTweener : TweenerBase<CanvasGroup, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.alpha; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.alpha = value; }
		}
	}

	[TweenerData(-898, "Graphic", "Alpha", "Alpha")]
	public class GraphicAlphaTweener : TweenerBase<Graphic, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.color.a; }
		}
		protected override void OnApply(float value) {
			if (Target != null) {
				Color color = Target.color;
				color.a = value;
				Target.color = color;
			}
		}
	}

	[TweenerData(-897, "Graphic", "Color", "Color")]
	public class GraphicColorTweener : TweenerBase<Graphic, Color, ColorTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.color; }
		}
		protected override void OnApply(Color value) {
			if (Target != null) { Target.color = value; }
		}
	}

	[TweenerData(-896, "Graphic", "CanvasRendererAlpha", "Alpha")]
	public class CanvasRendererAlpha : TweenerBase<CanvasRenderer, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.GetAlpha(); }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.SetAlpha(value); }
		}
	}

	[TweenerData(-895, "Graphic", "CanvasRendererColor", "Color")]
	public class CanvasRendererColor : TweenerBase<CanvasRenderer, Color, ColorTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.GetColor(); }
		}
		protected override void OnApply(Color value) {
			if (Target != null) { Target.SetColor(value); }
		}
	}

	[TweenerData(-894, "Graphic", "ImageFill", "Image Fill")]
	public class ImageFillTweener : TweenerBase<Image, float, FloatTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.fillAmount; }
		}
		protected override void OnApply(float value) {
			if (Target != null) { Target.fillAmount = value; }
		}
	}
	
}