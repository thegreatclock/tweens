using UnityEngine;

namespace GreatClock.Common.Tweens {

	public abstract class TweenComponentBase : MonoBehaviour {

		[SerializeField, HideInInspector]
		protected eDisableMode m_DisableMode = eDisableMode.Stop;
		[SerializeField, HideInInspector]
		protected TweenData[] m_Tweens;

		public enum eDisableMode {
			None, Stop, Pause
		}

		public enum eEaseDefinition { EaseFunction, Curve, CurveWithDuration }

		private TweenerBase[] mTweeners;

		private int mEnableCount = 0;

		#region lifecircle

		void Awake() {
			InitComponents();
		}

		void OnEnable() {
			int count = m_Tweens.Length;
			bool firstEnable = mEnableCount++ <= 0;
			bool playMode = true;
			switch (m_DisableMode) {
				case eDisableMode.None:
					if (!firstEnable) { return; }
					break;
				case eDisableMode.Pause:
					playMode = firstEnable;
					break;
			}
			if (playMode) {
				for (int i = 0; i < count; i++) {
					if (!m_Tweens[i].autoPlay) { continue; }
					PlayTween(i, null);
				}
			} else if (mTweeners != null) {
				for (int i = 0, imax = mTweeners.Length; i < imax; i++) {
					TweenerBase tween = mTweeners[i];
					if (tween != null) { tween.Resume(); }
				}
			}
		}

		void OnDisable() {
			if (mTweeners != null) {
				switch (m_DisableMode) {
					case eDisableMode.Pause:
						for (int i = 0, imax = mTweeners.Length; i < imax; i++) {
							TweenerBase tween = mTweeners[i];
							if (tween != null) { tween.Pause(); }
						}
						break;
					case eDisableMode.Stop:
						for (int i = 0, imax = mTweeners.Length; i < imax; i++) {
							TweenerBase tween = mTweeners[i];
							if (tween != null) {
								tween.Stop();
								tween.Reset();
							}
						}
						break;
				}
			}
		}

		void OnDestroy() {
			if (mTweeners != null) {
				for (int i = 0, imax = mTweeners.Length; i < imax; i++) {
					Tweener.CacheTweener(mTweeners[i]);
				}
				mTweeners = null;
			}
		}

		#endregion

		public float PlayGroup(string group) {
			if (string.IsNullOrEmpty(group)) { return -1f; }
			float ret = 0f;
			for (int i = 0, imax = m_Tweens.Length; i < imax; i++) {
				TweenData tween = m_Tweens[i];
				if (string.IsNullOrEmpty(tween.group)) { continue; }
				if (group != tween.group) { continue; }
				PlayTween(i, null);
				float dur = float.MaxValue;
				if (tween.loop == eLoopType.Once || tween.loop == eLoopType.Back) {
					dur = tween.duration < 0f && tween.curve != null ? tween.curve[tween.curve.length - 1].time : tween.duration;
				}
				if (dur > ret) { ret = dur; }
			}
			return ret;
		}

		public void StopGroup(string group, bool reset) {
			for (int i = 0, imax = mTweeners.Length; i < imax; i++) {
				TweenData tween = m_Tweens[i];
				if (string.IsNullOrEmpty(tween.group)) { continue; }
				if (group != tween.group) { continue; }
				TweenerBase tweener = mTweeners[i];
				if (tweener != null) {
					tweener.Stop();
					if (reset) { tweener.Reset(); }
				}
			}
		}

		public void PlayTween(int i, System.Action onFinish) {
			if (i < 0 || i >= m_Tweens.Length) { return; }
			if (mTweeners == null) { mTweeners = new TweenerBase[TweenTypeMax + 1]; }
			TweenData tween = m_Tweens[i];
			int ti = (int)tween.tweenType;
			TweenerBase tweener = mTweeners[ti];
			if (tweener != null) { tweener.Stop(); }
			PlayTweenData(tween);

			if (onFinish != null) {
				tweener = mTweeners[ti];
				if (tweener != null && tweener.Playing) {
					tweener.OnFinishEvent += onFinish;
				} else {
					try { onFinish(); } catch (System.Exception e) { Debug.LogException(e); }
				}
			}
		}

		public bool StopTween(int i, bool reset) {
			if (mTweeners == null) { return false; }
			if (i < 0 || i >= m_Tweens.Length) { return false; }
			TweenData tween = m_Tweens[i];
			int ti = (int)tween.tweenType;
			TweenerBase tweener = mTweeners[ti];
			if (tweener == null) { return false; }
			tweener.Stop();
			if (reset) { tweener.Reset(); }
			return true;
		}

		protected void PlayTweener<S, O, T, P>(TweenData tween, O target, T value)
			where P : PropTween<T>, new() where O : class where S : TweenerBase<O, T, P>, new() {
			int index = (int)tween.tweenType;
			S tweener = mTweeners[index] as S;
			if (tweener == null) {
				tweener = Tweener.GetTweener<S, O>(target);
				Tweener.CacheTweener(mTweeners[index]);
				mTweeners[index] = tweener;
			}
			tweener.Loop = tween.loop;
			tweener.LoopInterval = tween.loopInterval;
			tweener.IntervalKeepAtEnd = tween.intervalKeepAtEnd;
			AnimationCurve curve = tween.curve;
			float duration = tween.duration;
			Ease.eEaseType ease = tween.ease;
			switch (tween.easeDefinition) {
				case eEaseDefinition.Curve:
					if (curve != null && curve.length < 2) {
						curve = null;
					}
					ease = Ease.eEaseType.Linear;
					break;
				case eEaseDefinition.CurveWithDuration:
					if (curve != null && curve.length >= 2) {
						duration = -1f;
					} else {
						curve = null;
					}
					ease = Ease.eEaseType.Linear;
					break;
				default:
					curve = null;
					break;
			}
			if (curve != null) {
				if (tween.unscaled) {
					tweener.UnscaledDelayTo(value, tween.delay, duration, curve);
				} else {
					tweener.DelayTo(value, tween.delay, duration, curve);
				}
			} else {
				if (tween.unscaled) {
					tweener.UnscaledDelayTo(value, tween.delay, duration, ease);
				} else {
					tweener.DelayTo(value, tween.delay, duration, ease);
				}
			}
		}

		protected abstract void InitComponents();

		protected abstract void PlayTweenData(TweenData tween);

		protected abstract int TweenTypeMax { get; }

		[System.Serializable]
		public class TweenData {
			public bool autoPlay;
			public string group;
			public bool unscaled;
			public int tweenType;
			public float delay = 0f;
			public float duration = 1f;
			public eEaseDefinition easeDefinition = eEaseDefinition.EaseFunction;
			public Ease.eEaseType ease = Ease.eEaseType.Linear;
			public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
			public eLoopType loop = eLoopType.Once;
			public float loopInterval;
			public bool intervalKeepAtEnd;
			public float floatValue;
			public Vector2 vector2Value;
			public Vector3 vector3Value;
			public Color colorValue;
		}

		protected class ComponentData<T> where T : Component {
			private GameObject mGameObject;
			private bool mCreateIfMissing;
			private T mComponent = null;
			private bool mInited = false;
			public ComponentData(GameObject go, bool createIfMissing) { mGameObject = go; mCreateIfMissing = createIfMissing; }
			public T component {
				get {
					if (!mInited) {
						mInited = true;
						mComponent = mGameObject.GetComponent<T>();
						if (mComponent == null && mCreateIfMissing) {
							mComponent = mGameObject.AddComponent<T>();
						}
					}
					return mComponent;
				}
			}
		}

	}

}
