using GreatClock.Common.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreatClock.Common.Tweens {

	[TweenerComponentCreateIfMissing, ExecuteInEditMode]
	public class AxesTransformSupport : MonoBehaviour {

		private Transform mTrans;
		private bool mPosChanged;
		private Vector3 mPos;
		private bool mRotChanged;
		private Vector3 mRot;
		private bool mScaleChanged;
		private Vector3 mScale;

		private int mUpdateSinceFrame;
		private System.DateTime mLastModifyTime;
		private GameUpdater.UpdateDelegate mOnUpdate;

		void Awake() {
			mTrans = transform;
			mPosChanged = false;
			mPos = mTrans.localPosition;
			mRotChanged = false;
			mRot = mTrans.localEulerAngles;
			mScaleChanged = false;
			mScale = mTrans.localScale;
			mUpdateSinceFrame = -1;
			mOnUpdate = OnUpdate;
		}

		public float px { get { return mPos.x; } set { if (mPos.x != value) { PosChanged(); mPos.x = value; } } }
		public float py { get { return mPos.y; } set { if (mPos.y != value) { PosChanged(); mPos.y = value; } } }
		public float pz { get { return mPos.z; } set { if (mPos.z != value) { PosChanged(); mPos.z = value; } } }

		public float rx { get { return mRot.x; } set { if (mRot.x != value) { RotChanged(); mRot.x = value; } } }
		public float ry { get { return mRot.y; } set { if (mRot.y != value) { RotChanged(); mRot.y = value; } } }
		public float rz { get { return mRot.z; } set { if (mRot.z != value) { RotChanged(); mRot.z = value; } } }

		public float sx { get { return mScale.x; } set { if (mScale.x != value) { ScaleChanged(); mScale.x = value; } } }
		public float sy { get { return mScale.y; } set { if (mScale.y != value) { ScaleChanged(); mScale.y = value; } } }
		public float sz { get { return mScale.z; } set { if (mScale.z != value) { ScaleChanged(); mScale.z = value; } } }

		private void PosChanged() {
			mPosChanged = true;
			CheckUpdate();
		}

		private void RotChanged() {
			mRotChanged = true;
			CheckUpdate();
		}

		private void ScaleChanged() {
			mScaleChanged = true;
			CheckUpdate();
		}

		private void CheckUpdate() {
			mLastModifyTime = System.DateTime.UtcNow;
			if (mUpdateSinceFrame < 0) {
				mUpdateSinceFrame = Time.frameCount;
				GameUpdater.updater.Add(mOnUpdate, 1000);
			}
		}

		private void OnUpdate(float deltaTime) {
			bool updated = false;
			if (mPosChanged) {
				mPosChanged = false;
				updated = true;
				mTrans.localPosition = mPos;
			}
			if (mRotChanged) {
				mRotChanged = false;
				updated = true;
				mTrans.localEulerAngles = mRot;
			}
			if (mScaleChanged) {
				mScaleChanged = false;
				updated = true;
				mTrans.localScale = mScale;
			}
			if (!updated && (System.DateTime.UtcNow - mLastModifyTime).TotalSeconds > 1f) {
				mUpdateSinceFrame = -1;
				GameUpdater.updater.Remove(mOnUpdate);
			}
		}

	}

}