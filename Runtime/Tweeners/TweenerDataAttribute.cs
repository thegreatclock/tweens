using System;

namespace GreatClock.Common.Tweens {

	[AttributeUsage(AttributeTargets.Class)]
	public class TweenerDataAttribute : Attribute {
		public readonly int Priority;
		public readonly string TweenCategory;
		public readonly string TweenType;
		public readonly string PropertyName;
		public TweenerDataAttribute(int priority, string tweenCategory, string tweenType, string propertyName) {
			Priority = priority;
			TweenCategory = tweenCategory;
			TweenType = tweenType;
			PropertyName = propertyName;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class TweenerComponentCreateIfMissingAttribute : Attribute { }

	public enum ePropertyType { Float, Vector2, Vector3, Color }

}
