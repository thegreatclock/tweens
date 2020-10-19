using GreatClock.Common.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace GreatClock.Common.Tweens {

	public static class Tweener {

		public static T GetTweener<T, O>(O target) where T : TweenerBase<O>, new() where O : class {
			T t = null;
			if (cached_tweeners != null) {
				int index = GetTweenerIndex(typeof(T));
				if (index < cached_tweeners.Count) {
					Queue<TweenerBase> queue = cached_tweeners[index];
					if (queue != null && queue.Count > 0) {
						t = queue.Dequeue() as T;
					}
				}
			}
			if (t == null) { t = new T(); }
			t.Target = target;
			t.Init();
			return t;
		}

		public static void CacheTweener(TweenerBase tweener) {
			if (tweener == null) { return; }
			int index = GetTweenerIndex(tweener.GetType());
			tweener.Clear();
			if (cached_tweeners == null) {
				int capacity = 16;
				if (index >= capacity) {
					index--;
					index |= index >> 1;
					index |= index >> 2;
					index |= index >> 4;
					index |= index >> 8;
					index |= index >> 16;
					index++;
					capacity = index;
				}
				cached_tweeners = new List<Queue<TweenerBase>>(16);
			}
			while (index >= cached_tweeners.Count) {
				cached_tweeners.Add(null);
			}
			Queue<TweenerBase> queue = cached_tweeners[index];
			if (queue == null) {
				queue = new Queue<TweenerBase>();
				cached_tweeners[index] = queue;
			}
			queue.Enqueue(tweener);
		}

		private static List<Queue<TweenerBase>> cached_tweeners;

		private static Dictionary<System.Type, int> type_indices;
		private static int GetTweenerIndex(System.Type type) {
			if (type_indices == null) { type_indices = new Dictionary<System.Type, int>(32); }
			int index;
			if (type_indices.TryGetValue(type, out index)) { return index; }
			index = type_indices.Count;
			type_indices.Add(type, index);
			return index;
		}

	}

	public abstract class TweenerBase {
		public abstract void Pause();
		public abstract void Resume();
		public abstract void Stop();
		public abstract void Reset();
		public abstract void Clear();
		public abstract bool Playing { get; }
		public System.Action OnFinishEvent;
	}
	public abstract class TweenerBase<O> : TweenerBase where O : class {
		public O Target { get; set; }
		private bool mInited = false;
		public void Init() {
			if (mInited) { return; }
			mInited = true;
			OnInit();
		}
		public override void Clear() {
			Target = null;
			mInited = false;
		}
		protected abstract void OnInit();
	}
	public abstract class TweenerBase<O, T, P> : TweenerBase<O>
		where O : class where P : PropTween<T>, new() {
		private readonly GameUpdater.IUpdater mUpdater;
		public TweenerBase() {
			mUpdater = GameUpdater.updater;
		}
		public TweenerBase(GameUpdater.IUpdater updater) {
			mUpdater = updater ?? GameUpdater.updater;
		}
		public bool AutoCache = false;
		public bool Unscaled { get; private set; }
		public eLoopType Loop { get { return mPropTween.Loop; } set { mPropTween.Loop = value; } }
		public float LoopInterval { get { return mPropTween.LoopInterval; } set { mPropTween.LoopInterval = value; } }
		public bool IntervalKeepAtEnd { get { return mPropTween.IntervalKeepAtEnd; } set { mPropTween.IntervalKeepAtEnd = value; } }
		public T Current { get { return mPropTween.Value; } set { mPropTween.Value = value; StartValue = value; } }
		public T StartValue { get; private set; }
		public override bool Playing { get { return mAnimating || mPaused; } }
		public void To(T to, float duration, Ease.eEaseType ease) {
			Unscaled = false;
			if (!mPropTween.AnimTo(to, duration, ease) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.Add(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void To(T to, float duration, AnimationCurve curve) {
			Unscaled = false;
			if (!mPropTween.AnimTo(to, duration, curve) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.Add(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void UnscaledTo(T to, float duration, Ease.eEaseType ease) {
			Unscaled = true;
			if (!mPropTween.AnimTo(to, duration, ease) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.AddUnScaled(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void UnscaledTo(T to, float duration, AnimationCurve curve) {
			Unscaled = true;
			if (!mPropTween.AnimTo(to, duration, curve) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.AddUnScaled(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void DelayTo(T to, float delay, float duration, Ease.eEaseType ease) {
			Unscaled = false;
			if (!mPropTween.DelayAnimTo(to, delay, duration, ease) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.Add(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void DelayTo(T to, float delay, float duration, AnimationCurve curve) {
			Unscaled = false;
			if (!mPropTween.DelayAnimTo(to, delay, duration, curve) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.Add(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void UnscaledDelayTo(T to, float delay, float duration, Ease.eEaseType ease) {
			Unscaled = true;
			if (!mPropTween.DelayAnimTo(to, delay, duration, ease) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.AddUnScaled(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		public void UnscaledDelayTo(T to, float delay, float duration, AnimationCurve curve) {
			Unscaled = true;
			if (!mPropTween.DelayAnimTo(to, delay, duration, curve) && !mAnimating) {
				mAnimating = true;
				if (mOnUpdate == null) { mOnUpdate = OnUpdate; }
				mUpdater.AddUnScaled(mOnUpdate, -50);
			}
			OnApply(mPropTween.Value);
		}
		private P mPropTween = new P();
		private bool mAnimating = false;
		private bool mPaused = false;
		private GameUpdater.UpdateDelegate mOnUpdate;
		private void OnUpdate(float deltaTime) {
			eUpdateReturn up = mPropTween.Update(deltaTime);
			if (up != eUpdateReturn.NoUpdate) { OnApply(mPropTween.Value); }
			if (up == eUpdateReturn.Finished) {
				mUpdater.Remove(mOnUpdate);
				mAnimating = false;
				OnFinish();
				System.Action callback = OnFinishEvent;
				if (callback != null) { try { callback(); } catch (System.Exception e) { Debug.LogException(e); } }
				if (AutoCache) { Tweener.CacheTweener(this); }
			} else if (up == eUpdateReturn.LoopFinished) {
				System.Action callback = OnFinishEvent;
				if (callback != null) { try { callback(); } catch (System.Exception e) { Debug.LogException(e); } }
			}
		}
		protected abstract void OnApply(T value);
		protected virtual void OnFinish() { }
		protected virtual void OnClear() { }
		public sealed override void Clear() {
			if (mAnimating) {
				mUpdater.Remove(mOnUpdate);
				mAnimating = false;
			}
			OnClear();
			AutoCache = false;
			mPaused = false;
			OnFinishEvent = null;
			mPropTween.Loop = eLoopType.Once;
			mPropTween.LoopInterval = 0f;
			mPropTween.IntervalKeepAtEnd = false;
			base.Clear();
		}
		public override void Pause() {
			if (mAnimating) {
				mUpdater.Remove(mOnUpdate);
				mAnimating = false;
				mPaused = true;
			}
		}
		public override void Resume() {
			if (mPaused && !mAnimating) {
				mPaused = false;
				mAnimating = true;
				if (Unscaled) {
					mUpdater.AddUnScaled(mOnUpdate, -50);
				} else {
					mUpdater.Add(mOnUpdate, -50);
				}
			}
		}
		public override void Stop() {
			if (mAnimating) {
				mUpdater.Remove(mOnUpdate);
				mAnimating = false;
				mPropTween.Value = mPropTween.Value;
			}
			mPaused = false;
			OnFinishEvent = null;
		}
		public override void Reset() {
			mPropTween.Value = StartValue;
			OnApply(StartValue);
		}
	}

}
