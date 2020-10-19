using UnityEngine;

namespace GreatClock.Common.Tweens {


	[TweenerData(-859, "Graphic", "Alpha", "Alpha")]
	public class SpriteRendererAlphaTweener : TweenerBase<SpriteRenderer, float, FloatTween> {
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

	[TweenerData(-858, "Graphic", "Color", "Color")]
	public class SpriteRendererColorTweener : TweenerBase<SpriteRenderer, Color, ColorTween> {
		protected override void OnInit() {
			if (Target != null) { Current = Target.color; }
		}
		protected override void OnApply(Color value) {
			if (Target != null) { Target.color = value; }
		}
	}
	
}