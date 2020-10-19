using System;
using UnityEngine;

namespace GreatClock.Common.Tweens {

	public static class Ease {

		public enum eEaseType {
			Linear,
			QuadIn, QuadOut, QuadInOut, QuadOutIn,
			CubicIn, CubicOut, CubicInOut, CubicOutIn,
			QuartIn, QuartOut, QuartInOut, QuartOutIn,
			QuintIn, QuintOut, QuintInOut, QuintOutIn,
			ExpoIn, ExpoOut, ExpoInOut, ExpoOutIn,
			CircIn, CircOut, CircInOut, CircOutIn,
			SineIn, SineOut, SineInOut, SineOutIn,
			ElasticIn, ElasticOut, ElasticInOut, ElasticOutIn,
			BounceIn, BounceOut, BounceInOut, BounceOutIn,
			BackIn, BackOut, BackInOut, BackOutIn
		}

		public static Func<float, float> GetEaseFunc(eEaseType ease) {
			Func<float, float> func = null;
			switch (ease) {
				case eEaseType.QuadIn:
					if (sEaseInQuad == null) { sEaseInQuad = EaseInQuad; }
					func = EaseInQuad; break;
				case eEaseType.QuadOut:
					if (sEaseOutQuad == null) { sEaseOutQuad = EaseOutQuad; }
					func = EaseOutQuad; break;
				case eEaseType.QuadInOut:
					if (sEaseInOutQuad == null) { sEaseInOutQuad = EaseInOutQuad; }
					func = EaseInOutQuad; break;
				case eEaseType.QuadOutIn:
					if (sEaseOutInQuad == null) { sEaseOutInQuad = EaseOutInQuad; }
					func = EaseOutInQuad; break;
				case eEaseType.CubicIn:
					if (sEaseInCubic == null) { sEaseInCubic = EaseInCubic; }
					func = EaseInCubic; break;
				case eEaseType.CubicOut:
					if (sEaseOutCubic == null) { sEaseOutCubic = EaseOutCubic; }
					func = EaseOutCubic; break;
				case eEaseType.CubicInOut:
					if (sEaseInOutCubic == null) { sEaseInOutCubic = EaseInOutCubic; }
					func = EaseInOutCubic; break;
				case eEaseType.CubicOutIn:
					if (sEaseOutInCubic == null) { sEaseOutInCubic = EaseOutInCubic; }
					func = EaseOutInCubic; break;
				case eEaseType.QuartIn:
					if (sEaseInQuart == null) { sEaseInQuart = EaseInQuart; }
					func = EaseInQuart; break;
				case eEaseType.QuartOut:
					if (sEaseOutQuart == null) { sEaseOutQuart = EaseOutQuart; }
					func = EaseOutQuart; break;
				case eEaseType.QuartInOut:
					if (sEaseInOutQuart == null) { sEaseInOutQuart = EaseInOutQuart; }
					func = EaseInOutQuart; break;
				case eEaseType.QuartOutIn:
					if (sEaseOutInQuart == null) { sEaseOutInQuart = EaseOutInQuart; }
					func = EaseOutInQuart; break;
				case eEaseType.QuintIn:
					if (sEaseInQuint == null) { sEaseInQuint = EaseInQuint; }
					func = EaseInQuint; break;
				case eEaseType.QuintOut:
					if (sEaseOutQuint == null) { sEaseOutQuint = EaseOutQuint; }
					func = EaseOutQuint; break;
				case eEaseType.QuintInOut:
					if (sEaseInOutQuint == null) { sEaseInOutQuint = EaseInOutQuint; }
					func = EaseInOutQuint; break;
				case eEaseType.QuintOutIn:
					if (sEaseOutInQuint == null) { sEaseOutInQuint = EaseOutInQuint; }
					func = EaseOutInQuint; break;
				case eEaseType.ExpoIn:
					if (sEaseInExpo == null) { sEaseInExpo = EaseInExpo; }
					func = EaseInExpo; break;
				case eEaseType.ExpoOut:
					if (sEaseOutExpo == null) { sEaseOutExpo = EaseOutExpo; }
					func = EaseOutExpo; break;
				case eEaseType.ExpoInOut:
					if (sEaseInOutExpo == null) { sEaseInOutExpo = EaseInOutExpo; }
					func = EaseInOutExpo; break;
				case eEaseType.ExpoOutIn:
					if (sEaseOutInExpo == null) { sEaseOutInExpo = EaseOutInExpo; }
					func = EaseOutInExpo; break;
				case eEaseType.CircIn:
					if (sEaseInCirc == null) { sEaseInCirc = EaseInCirc; }
					func = EaseInCirc; break;
				case eEaseType.CircOut:
					if (sEaseOutCirc == null) { sEaseOutCirc = EaseOutCirc; }
					func = EaseOutCirc; break;
				case eEaseType.CircInOut:
					if (sEaseInOutCirc == null) { sEaseInOutCirc = EaseInOutCirc; }
					func = EaseInOutCirc; break;
				case eEaseType.CircOutIn:
					if (sEaseOutInCirc == null) { sEaseOutInCirc = EaseOutInCirc; }
					func = EaseOutInCirc; break;
				case eEaseType.SineIn:
					if (sEaseInSine == null) { sEaseInSine = EaseInSine; }
					func = EaseInSine; break;
				case eEaseType.SineOut:
					if (sEaseOutSine == null) { sEaseOutSine = EaseOutSine; }
					func = EaseOutSine; break;
				case eEaseType.SineInOut:
					if (sEaseInOutSine == null) { sEaseInOutSine = EaseInOutSine; }
					func = EaseInOutSine; break;
				case eEaseType.SineOutIn:
					if (sEaseOutInSine == null) { sEaseOutInSine = EaseOutInSine; }
					func = EaseOutInSine; break;
				case eEaseType.ElasticIn:
					if (sEaseInElastic == null) { sEaseInElastic = EaseInElastic; }
					func = EaseInElastic; break;
				case eEaseType.ElasticOut:
					if (sEaseOutElastic == null) { sEaseOutElastic = EaseOutElastic; }
					func = EaseOutElastic; break;
				case eEaseType.ElasticInOut:
					if (sEaseInOutElastic == null) { sEaseInOutElastic = EaseInOutElastic; }
					func = EaseInOutElastic; break;
				case eEaseType.ElasticOutIn:
					if (sEaseOutInElastic == null) { sEaseOutInElastic = EaseOutInElastic; }
					func = EaseOutInElastic; break;
				case eEaseType.BounceIn:
					if (sEaseInBounce == null) { sEaseInBounce = EaseInBounce; }
					func = EaseInBounce; break;
				case eEaseType.BounceOut:
					if (sEaseOutBounce == null) { sEaseOutBounce = EaseOutBounce; }
					func = EaseOutBounce; break;
				case eEaseType.BounceInOut:
					if (sEaseInOutBounce == null) { sEaseInOutBounce = EaseInOutBounce; }
					func = EaseInOutBounce; break;
				case eEaseType.BounceOutIn:
					if (sEaseOutInBounce == null) { sEaseOutInBounce = EaseOutInBounce; }
					func = EaseOutInBounce; break;
				case eEaseType.BackIn:
					if (sEaseInBack == null) { sEaseInBack = EaseInBack; }
					func = EaseInBack; break;
				case eEaseType.BackOut:
					if (sEaseOutBack == null) { sEaseOutBack = EaseOutBack; }
					func = EaseOutBack; break;
				case eEaseType.BackInOut:
					if (sEaseInOutBack == null) { sEaseInOutBack = EaseInOutBack; }
					func = EaseInOutBack; break;
				case eEaseType.BackOutIn:
					if (sEaseOutInBack == null) { sEaseOutInBack = EaseOutInBack; }
					func = EaseOutInBack; break;
			}
			if (func != null) { return func; }
			if (sEaseLinear == null) { sEaseLinear = EaseLinear; }
			return sEaseLinear;
		}
		private static Func<float, float> sEaseLinear = null;
		private static float EaseLinear(float t) {
			return t;
		}
		private static Func<float, float> sEaseInQuad = null;
		private static float EaseInQuad(float t) {
			return t * t;
		}
		private static Func<float, float> sEaseOutQuad = null;
		private static float EaseOutQuad(float t) {
			return t * (2f - t);
		}
		private static Func<float, float> sEaseInOutQuad = null;
		private static float EaseInOutQuad(float t) {
			return t <= 0.5f ? (0.5f * EaseInQuad(t * 2f)) : (0.5f * EaseOutQuad(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInQuad = null;
		private static float EaseOutInQuad(float t) {
			return t <= 0.5f ? (0.5f * EaseOutQuad(t * 2f)) : (0.5f * EaseInQuad(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInCubic = null;
		private static float EaseInCubic(float t) {
			return t * t * t;
		}
		private static Func<float, float> sEaseOutCubic = null;
		private static float EaseOutCubic(float t) {
			return (t -= 1f) * t * t + 1f;
		}
		private static Func<float, float> sEaseInOutCubic = null;
		private static float EaseInOutCubic(float t) {
			return t <= 0.5f ? (0.5f * EaseInCubic(t * 2f)) : (0.5f * EaseOutCubic(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInCubic = null;
		private static float EaseOutInCubic(float t) {
			return t <= 0.5f ? (0.5f * EaseOutCubic(t * 2f)) : (0.5f * EaseInCubic(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInQuart = null;
		private static float EaseInQuart(float t) {
			return t * t * t * t;
		}
		private static Func<float, float> sEaseOutQuart = null;
		private static float EaseOutQuart(float t) {
			return 1f - (t -= 1f) * t * t * t;
		}
		private static Func<float, float> sEaseInOutQuart = null;
		private static float EaseInOutQuart(float t) {
			return t <= 0.5f ? (0.5f * EaseInQuart(t * 2f)) : (0.5f * EaseOutQuart(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInQuart = null;
		private static float EaseOutInQuart(float t) {
			return t <= 0.5f ? (0.5f * EaseOutQuart(t * 2f)) : (0.5f * EaseInQuart(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInQuint = null;
		private static float EaseInQuint(float t) {
			return t * t * t * t * t;
		}
		private static Func<float, float> sEaseOutQuint = null;
		private static float EaseOutQuint(float t) {
			return (t -= 1f) * t * t * t * t + 1f;
		}
		private static Func<float, float> sEaseInOutQuint = null;
		private static float EaseInOutQuint(float t) {
			return t <= 0.5f ? (0.5f * EaseInQuint(t * 2f)) : (0.5f * EaseOutQuint(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInQuint = null;
		private static float EaseOutInQuint(float t) {
			return t <= 0.5f ? (0.5f * EaseOutQuint(t * 2f)) : (0.5f * EaseInQuint(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInExpo = null;
		private static float EaseInExpo(float t) {
			return t == 0f ? 0f : (t == 1f ? 1f : Mathf.Pow(2f, 10 * (t - 1f)));
		}
		private static Func<float, float> sEaseOutExpo = null;
		private static float EaseOutExpo(float t) {
			return t == 0f ? 0f : (t == 1f ? 1f : (1f - Mathf.Pow(2, -10 * t)));
		}
		private static Func<float, float> sEaseInOutExpo = null;
		private static float EaseInOutExpo(float t) {
			return t <= 0.5f ? (0.5f * EaseInExpo(t * 2f)) : (0.5f * EaseOutExpo(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInExpo = null;
		private static float EaseOutInExpo(float t) {
			return t <= 0.5f ? (0.5f * EaseOutExpo(t * 2f)) : (0.5f * EaseInExpo(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInCirc = null;
		private static float EaseInCirc(float t) {
			return 1f - Mathf.Sqrt(1f - t * t);
		}
		private static Func<float, float> sEaseOutCirc = null;
		private static float EaseOutCirc(float t) {
			return Mathf.Sqrt(1f - (t -= 1f) * t);
		}
		private static Func<float, float> sEaseInOutCirc = null;
		private static float EaseInOutCirc(float t) {
			return t <= 0.5f ? (0.5f * EaseInCirc(t * 2f)) : (0.5f * EaseOutCirc(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInCirc = null;
		private static float EaseOutInCirc(float t) {
			return t <= 0.5f ? (0.5f * EaseOutCirc(t * 2f)) : (0.5f * EaseInCirc(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInSine = null;
		private static float EaseInSine(float t) {
			return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
		}
		private static Func<float, float> sEaseOutSine = null;
		private static float EaseOutSine(float t) {
			return Mathf.Sin(t * Mathf.PI * 0.5f);
		}
		private static Func<float, float> sEaseInOutSine = null;
		private static float EaseInOutSine(float t) {
			return 0.5f - 0.5f * Mathf.Cos(t * Mathf.PI);
		}
		private static Func<float, float> sEaseOutInSine = null;
		private static float EaseOutInSine(float t) {
			return t <= 0.5f ? (0.5f * EaseOutSine(t * 2f)) : (0.5f * EaseInSine(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInElastic = null;
		private static float EaseInElastic(float t) {
			if (t == 0f || t == 1f) return t;
			float p = 0.3f;
			float s = p / 4f;
			return Mathf.Pow(2, 10 * (t -= 1f)) * Mathf.Sin((s - t) * 2f * Mathf.PI / p);
		}
		private static Func<float, float> sEaseOutElastic = null;
		private static float EaseOutElastic(float t) {
			if (t == 0f || t == 1f) return t;
			float p = 0.3f;
			float s = p / 4f;
			return 1f + Mathf.Pow(2, -10f * t) * Mathf.Sin((t - s) * 2f * Mathf.PI / p);
		}
		private static Func<float, float> sEaseInOutElastic = null;
		private static float EaseInOutElastic(float t) {
			return t <= 0.5f ? (0.5f * EaseInElastic(t * 2f)) : (0.5f * EaseOutElastic(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInElastic = null;
		private static float EaseOutInElastic(float t) {
			return t <= 0.5f ? (0.5f * EaseOutElastic(t * 2f)) : (0.5f * EaseInElastic(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInBounce = null;
		private static float EaseInBounce(float t) {
			return 1f - EaseOutBounce(1f - t);
		}
		private static Func<float, float> sEaseOutBounce = null;
		private static float EaseOutBounce(float t) {
			float p = 1f / 2.75f;
			if (t < p) return 7.5625f * t * t;
			if (t < 2f * p) return 7.5625f * (t -= (1.5f * p)) * t + 0.75f;
			if (t < 2.5f * p) return 7.5625f * (t -= (2.25f * p)) * t + 0.9375f;
			return 7.5625f * (t -= (2.625f * p)) * t + 0.984375f;
		}
		private static Func<float, float> sEaseInOutBounce = null;
		private static float EaseInOutBounce(float t) {
			return t <= 0.5f ? (0.5f * EaseInBounce(t * 2f)) : (0.5f * EaseOutBounce(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInBounce = null;
		private static float EaseOutInBounce(float t) {
			return t <= 0.5f ? (0.5f * EaseOutBounce(t * 2f)) : (0.5f * EaseInBounce(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseInBack = null;
		private static float EaseInBack(float t) {
			return t * t * (t * 2.70158f - 1.70158f);
		}
		private static Func<float, float> sEaseOutBack = null;
		private static float EaseOutBack(float t) {
			return 1f + (t -= 1f) * t * (2.70158f * t + 1.70158f);
		}
		private static Func<float, float> sEaseInOutBack = null;
		private static float EaseInOutBack(float t) {
			return t <= 0.5f ? (0.5f * EaseInBack(t * 2f)) : (0.5f * EaseOutBack(t * 2f - 1f) + 0.5f);
		}
		private static Func<float, float> sEaseOutInBack = null;
		private static float EaseOutInBack(float t) {
			return t <= 0.5f ? (0.5f * EaseOutBack(t * 2f)) : (0.5f * EaseInBack(t * 2f - 1f) + 0.5f);
		}

	}
}