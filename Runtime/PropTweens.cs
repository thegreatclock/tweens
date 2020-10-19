using UnityEngine;

namespace GreatClock.Common.Tweens {

	public enum ePropTweenState {
		None, SpeedControl, FixedDuration
	}

	public enum eLoopType {
		Once, Back, Loop, PingPong, Yoyo, BackLoop, BackPingPong, BackYoyo
	}

	public enum eUpdateReturn {
		Update, NoUpdate, Finished, LoopFinished
	}

	public abstract class PropTween {
		public abstract eUpdateReturn Update(float deltaTime);
	}

	public abstract class PropTween<T> : PropTween {
		private T mValue;
		private T mTo;
		private float mTimer;
		private float mDuration;
		private float mSpeedFactor;
		private float mMinSpeed;
		private float mMaxSpeed;
		private System.Func<float, float> mEaseFunc;
		private AnimationCurve mCurve;
		private float mCurveLength;
		private float mIntervalTimeout;
		public eLoopType Loop { get; set; }
		public float LoopInterval { get; set; }
		public bool IntervalKeepAtEnd { get; set; }
		public int LoopCount { get; private set; }
		public ePropTweenState State { get; private set; }
		public T From { get; private set; }
		public T To {
			get {
				return mTo;
			}
			set {
				if (State != ePropTweenState.SpeedControl) {
					throw new System.InvalidOperationException("'To' can only be set during SpeedControl !");
				}
				mTo = value;
			}
		}
		public T Value {
			get {
				return mValue;
			}
			set {
				State = ePropTweenState.None;
				mValue = value;
			}
		}
		public override eUpdateReturn Update(float deltaTime) {
			eUpdateReturn ret = eUpdateReturn.Finished;
			switch (State) {
				case ePropTweenState.SpeedControl:
					float distance = GetDistance(mValue, mTo);
					float speed = Mathf.Clamp(mSpeedFactor * distance, mMinSpeed, mMaxSpeed);
					T newValue;
					ret = SpeedEnvaluate(mValue, mTo, speed * deltaTime, out newValue) ?
						eUpdateReturn.Finished : eUpdateReturn.Update;
					mValue = newValue;
					break;
				case ePropTweenState.FixedDuration:
					if (mIntervalTimeout > 0f) {
						mIntervalTimeout -= deltaTime;
						deltaTime = 0f;
						if (mIntervalTimeout < 0f) {
							deltaTime = -mIntervalTimeout;
							mIntervalTimeout = 0f;
						}
					}
					mTimer += deltaTime;
					float t = mTimer * mDuration;
					if (t < 0f) {
						ret = eUpdateReturn.NoUpdate;
						break;
					}
					float ratio = t;
					bool isPlayOnlyOnce = Loop == eLoopType.Once || Loop == eLoopType.Back;
					int loopCount = Mathf.FloorToInt(t);
					ret = isPlayOnlyOnce && t >= 1f ? eUpdateReturn.Finished :
						(LoopCount < loopCount ? eUpdateReturn.LoopFinished : eUpdateReturn.Update);
					if (ret == eUpdateReturn.LoopFinished) {
						mIntervalTimeout = (loopCount - LoopCount) * LoopInterval;
						float overflow = (t - loopCount) / mDuration;
						mIntervalTimeout -= overflow;
						if (mIntervalTimeout < 0f) {
							overflow = -mIntervalTimeout;
							mIntervalTimeout = 0f;
						}
						mTimer -= overflow;
						LoopCount = loopCount;
					}
					T from = From;
					T to = To;
					switch (Loop) {
						case eLoopType.Once:
							ratio = Mathf.Clamp01(ratio);
							break;
						case eLoopType.Back:
							ratio = Mathf.Clamp01(ratio);
							from = To;
							to = From;
							break;
						case eLoopType.Loop:
							if (mIntervalTimeout > 0f) {
								ratio = IntervalKeepAtEnd ? 1f : 0f;
							} else {
								ratio = ratio % 1f;
							}
							break;
						case eLoopType.PingPong:
							bool forward1 = (((int)ratio) & 1) == 0;
							ratio = ratio % 1f;
							if (!forward1) {
								ratio = 1f - ratio;
							}
							if (mIntervalTimeout > 0f) {
								ratio = Mathf.Round(ratio);
							}
							break;
						case eLoopType.Yoyo:
							if ((((int)ratio) & 1) != 0) {
								from = To;
								to = From;
							}
							ratio = ratio % 1f;
							if (mIntervalTimeout > 0f) {
								ratio = Mathf.Round(ratio);
							}
							break;
						case eLoopType.BackLoop:
							if (mIntervalTimeout > 0f) {
								ratio = IntervalKeepAtEnd ? 0f : 1f;
							} else {
								ratio = ratio % 1f;
							}
							from = To;
							to = From;
							break;
						case eLoopType.BackPingPong:
							bool forward2 = (((int)ratio) & 1) == 0;
							ratio = ratio % 1f;
							if (!forward2) {
								ratio = 1f - ratio;
							}
							from = To;
							to = From;
							if (mIntervalTimeout > 0f) {
								ratio = Mathf.Round(ratio);
							}
							break;
						case eLoopType.BackYoyo:
							if ((((int)ratio) & 1) == 0) {
								from = To;
								to = From;
							}
							ratio = ratio % 1f;
							if (mIntervalTimeout > 0f) {
								ratio = Mathf.Round(ratio);
							}
							break;
					}
					if (mCurve == null) {
						if (mEaseFunc != null) { ratio = mEaseFunc(ratio); }
					} else {
						ratio = mCurve.Evaluate(ratio * mCurveLength);
					}
					mValue = Lerp(from, to, ratio);
					break;
			}
			if (ret == eUpdateReturn.Finished && deltaTime != 0f) { State = ePropTweenState.None; }
			return ret;
		}

		public bool AnimTo(T to, float duration) {
			return AnimToInternal(to, 0f, duration, Ease.eEaseType.Linear, null);
		}
		public bool AnimTo(T to, float duration, Ease.eEaseType ease) {
			return AnimToInternal(to, 0f, duration, ease, null);
		}
		public bool AnimTo(T to, float duration, AnimationCurve curve) {
			return AnimToInternal(to, 0f, duration, Ease.eEaseType.Linear, curve);
		}

		public bool DelayAnimTo(T to, float delay, float duration) {
			return AnimToInternal(to, delay, duration, Ease.eEaseType.Linear, null);
		}
		public bool DelayAnimTo(T to, float delay, float duration, Ease.eEaseType ease) {
			return AnimToInternal(to, delay, duration, ease, null);
		}
		public bool DelayAnimTo(T to, float delay, float duration, AnimationCurve curve) {
			return AnimToInternal(to, delay, duration, Ease.eEaseType.Linear, curve);
		}

		public void GoTo(T to, float speedFactor, float minSpeed, float maxSpeed) {
			State = ePropTweenState.SpeedControl;
			From = mValue;
			mTo = to;
			mSpeedFactor = speedFactor;
			mMinSpeed = minSpeed;
			mMaxSpeed = maxSpeed;
		}

		protected abstract T Lerp(T from, T to, float t);
		protected abstract float GetDistance(T from, T to);
		protected abstract bool SpeedEnvaluate(T current, T to, float delta, out T val);
		private bool AnimToInternal(T to, float delay, float duration, Ease.eEaseType ease, AnimationCurve curve) {
			LoopCount = 0;
			mIntervalTimeout = 0f;
			mCurveLength = curve == null || curve.length < 2 ? 0f : curve[curve.length - 1].time;
			if (duration < 0f && mCurveLength > 0f) {
				duration = mCurveLength;
			}
			if (duration <= 0f && delay <= 0f) {
				State = ePropTweenState.None;
				if (Loop != eLoopType.Back) { mValue = to; }
				return true;
			}
			From = mValue;
			mTo = to;
			State = ePropTweenState.FixedDuration;
			mDuration = 1f / duration;
			mEaseFunc = Ease.GetEaseFunc(ease);
			mCurve = curve;
			mTimer = Mathf.Min(0f, -delay);
			if (Update(0f) == eUpdateReturn.NoUpdate) {
				if (Loop == eLoopType.Back || Loop == eLoopType.BackLoop ||
					Loop == eLoopType.BackPingPong || Loop == eLoopType.BackYoyo) {
					mValue = to;
				}
			}
			return false;
		}

	}

	public sealed class FloatTween : PropTween<float> {
		protected override float GetDistance(float from, float to) {
			return Mathf.Abs(from - to);
		}
		protected override float Lerp(float from, float to, float t) {
			return (to - from) * t + from;
		}
		protected override bool SpeedEnvaluate(float current, float to, float delta, out float val) {
			val = current + Mathf.Sign(to - current) * delta;
			if ((to - val) * (to - current) <= 0f) {
				val = to;
				return true;
			}
			return false;
		}
	}

	public sealed class Vector2Tween : PropTween<Vector2> {
		protected override float GetDistance(Vector2 from, Vector2 to) {
			return Vector3.Distance(from, to);
		}
		protected override Vector2 Lerp(Vector2 from, Vector2 to, float t) {
			return (to - from) * t + from;
		}
		protected override bool SpeedEnvaluate(Vector2 current, Vector2 to, float delta, out Vector2 val) {
			Vector2 dir = (to - current).normalized;
			val = current + dir * delta;
			if (Vector3.Dot(to - val, dir) <= 0f) {
				val = to;
				return true;
			}
			return false;
		}
	}

	public sealed class Vector3Tween : PropTween<Vector3> {
		protected override float GetDistance(Vector3 from, Vector3 to) {
			return Vector3.Distance(from, to);
		}
		protected override Vector3 Lerp(Vector3 from, Vector3 to, float t) {
			return (to - from) * t + from;
		}
		protected override bool SpeedEnvaluate(Vector3 current, Vector3 to, float delta, out Vector3 val) {
			Vector3 dir = (to - current).normalized;
			val = current + dir * delta;
			if (Vector3.Dot(to - val, dir) <= 0f) {
				val = to;
				return true;
			}
			return false;
		}
	}

	public sealed class QuaternionTween : PropTween<Quaternion> {
		protected override float GetDistance(Quaternion from, Quaternion to) {
			return Quaternion.Angle(from, to);
		}
		protected override Quaternion Lerp(Quaternion from, Quaternion to, float t) {
			return Quaternion.LerpUnclamped(from, to, t);
		}
		protected override bool SpeedEnvaluate(Quaternion current, Quaternion to, float delta, out Quaternion val) {
			float d0 = Quaternion.Angle(current, to);
			val = Quaternion.RotateTowards(current, to, delta);
			if (d0 <= Quaternion.Angle(val, to)) {
				val = to;
				return true;
			}
			return false;
		}
	}

	public sealed class ColorTween : PropTween<Color> {
		protected override float GetDistance(Color from, Color to) {
			throw new System.NotImplementedException();
		}
		protected override Color Lerp(Color from, Color to, float t) {
			return Color.LerpUnclamped(from, to, t);
		}
		protected override bool SpeedEnvaluate(Color current, Color to, float delta, out Color val) {
			throw new System.NotImplementedException();
		}
	}

}
