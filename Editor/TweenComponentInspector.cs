using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GreatClock.Common.Tweens {

	[CustomEditor(typeof(TweenComponentBase), true)]
	public class TweenComponentInspector : Editor {

		private static Dictionary<int, List<TweenerTypeData>> type_tweeners = new Dictionary<int, List<TweenerTypeData>>();

		[InitializeOnLoadMethod]
		static void Initialize() {
			string scriptPath = "Assets/TweenComponent.cs";
			foreach (string guid in AssetDatabase.FindAssets("TweenComponent")) {
				string p = AssetDatabase.GUIDToAssetPath(guid);
				if (!p.EndsWith("/TweenComponent.cs")) { continue; }
				scriptPath = p;
			}
			Type tFloat = typeof(float);
			Type tVector2 = typeof(Vector2);
			Type tVector3 = typeof(Vector3);
			Type tQuaternion = typeof(Quaternion);
			Type tColor = typeof(Color);
			Type ta = typeof(TweenerDataAttribute);
			Type ttb = typeof(TweenerBase);
			Type ttc = typeof(TweenComponentBase);
			string tns = ttc.Namespace;
			string eTweenTypeFullName = tns + ".TweenComponent+eTweenType";
			Type prevTweenTypeEnum = null;
			List<string> nss = new List<string>();
			SortedList<int, TweenerTypeData> types = new SortedList<int, TweenerTypeData>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (Type type in assembly.GetTypes()) {
					if (type.IsEnum) {
						if (type.FullName == eTweenTypeFullName) {
							prevTweenTypeEnum = type;
						}
						continue;
					}
					if (!type.IsSubclassOf(ttb)) { continue; }
					if (type.IsGenericType) { continue; }
					TweenerDataAttribute attr = Attribute.GetCustomAttribute(type, ta) as TweenerDataAttribute;
					if (attr == null) { continue; }
					Type bt = type.BaseType;
					if (!bt.IsGenericType) { continue; }
					Type[] gts = bt.GetGenericArguments();
					if (gts.Length != 3) { continue; }
					ePropertyType pt = ePropertyType.Float;
					if (tFloat.Equals(gts[1])) {
						pt = ePropertyType.Float;
					} else if (tVector2.Equals(gts[1])) {
						pt = ePropertyType.Vector2;
					} else if (tVector3.Equals(gts[1]) || tQuaternion.Equals(gts[1])) {
						pt = ePropertyType.Vector3;
					} else if (tColor.Equals(gts[1])) {
						pt = ePropertyType.Color;
					} else {
						continue;
					}
					types.Add(attr.Priority, new TweenerTypeData(attr, pt, type, bt, gts));
					string ns = GetTypeNameSpace(type);
					if (!string.IsNullOrEmpty(ns) && !nss.Contains(ns)) { nss.Add(ns); }
					foreach (Type t in gts) {
						ns = GetTypeNameSpace(t);
						if (!string.IsNullOrEmpty(ns) && !nss.Contains(ns)) { nss.Add(ns); }
					}
				}
			}
			nss.Sort();
			Type tt = typeof(Transform);
			List<string> tweenTypes = new List<string>();
			List<List<TweenerTypeData>> tweenDatas = new List<List<TweenerTypeData>>();
			List<Type> components = new List<Type>();
			foreach (KeyValuePair<int, TweenerTypeData> kv in types) {
				int index = tweenTypes.IndexOf(kv.Value.attr.TweenType);
				if (index < 0) {
					index = tweenTypes.Count;
					tweenTypes.Add(kv.Value.attr.TweenType);
					tweenDatas.Add(new List<TweenerTypeData>());
				}
				tweenDatas[index].Add(kv.Value);
				Type componentType = kv.Value.generics[0];
				if (!tt.Equals(componentType) && !components.Contains(componentType)) {
					components.Add(componentType);
				}
			}
			int sizeComponents = components.Count;
			Dictionary<string, int> tweenTypeEnum = new Dictionary<string, int>();
			StringBuilder code = new StringBuilder();
			foreach (string ns in nss) {
				if (ns == tns) { continue; }
				code.AppendLine(string.Format("using {0};", ns));
			}
			code.AppendLine();
			code.AppendLine(string.Format("namespace {0} {{", ttc.Namespace));
			code.AppendLine();
			code.AppendLine("\tpublic class TweenComponent : TweenComponentBase {");
			code.AppendLine();
			int enumMax = WriteEnum(code, "\t\t", prevTweenTypeEnum, "public", "eTweenType", tweenTypes, tweenTypeEnum);
			code.AppendLine();
			if (sizeComponents > 0) {
				foreach (Type type in components) {
					code.AppendLine(string.Format("\t\tprivate ComponentData<{0}> m{0};", type.Name));
				}
				code.AppendLine();
			}
			code.AppendLine(string.Format("\t\tprotected override int TweenTypeMax {{ get {{ return {0}; }} }}", enumMax));
			code.AppendLine();
			Type tcim = typeof(TweenerComponentCreateIfMissingAttribute);
			if (sizeComponents > 0) {
				code.AppendLine("\t\tprotected override void InitComponents() {");
				code.AppendLine("\t\t\tGameObject go = gameObject;");
				foreach (Type type in components) {
					code.AppendLine(string.Format("\t\t\tm{0} = new ComponentData<{0}>(go, {1});",
						type.Name, Attribute.GetCustomAttribute(type, tcim) != null ? "true" : "false"));
				}
				code.AppendLine("\t\t}");
			} else {
				code.AppendLine("\t\tprotected override void InitComponents() { }");
			}
			code.AppendLine();
			code.AppendLine("\t\tprotected override void PlayTweenData(TweenData tween) {");
			code.AppendLine("\t\t\tswitch ((eTweenType)tween.tweenType) {");
			Type tq = typeof(Quaternion);
			int typesCount = tweenTypes.Count;
			tween_type_contents = new GUIContent[typesCount];
			tween_type_values = new int[typesCount];
			for (int i = 0; i < typesCount; i++) {
				string type = tweenTypes[i];
				List<TweenerTypeData> datas = tweenDatas[i];
				TweenerTypeData firstData = datas[0];
				int typeIndex;
				if (tweenTypeEnum.TryGetValue(type, out typeIndex)) {
					type_tweeners.Add(typeIndex, datas);
					while (typeIndex >= tween_field_and_label.Count) {
						tween_field_and_label.Add(null);
					}
					if (tween_field_and_label[typeIndex] == null) {
						tween_field_and_label[typeIndex] = new FieldAndLabel(firstData.propertyType, new GUIContent(firstData.attr.PropertyName));
					}
					tween_type_contents[i] = new GUIContent(string.IsNullOrEmpty(firstData.attr.TweenCategory) ?
						firstData.attr.TweenType : string.Concat(firstData.attr.TweenCategory, "/", firstData.attr.TweenType));
					tween_type_values[i] = typeIndex;
				}
				code.AppendLine(string.Format("\t\t\t\tcase eTweenType.{0}:", type));
				for (int j = 0; j < 2; j++) {
					int count = 0;
					foreach (TweenerTypeData data in datas) {
						if (j == 0 ^ tt.Equals(data.generics[0])) { continue; }
						string valueProp = null;
						switch (data.propertyType) {
							case ePropertyType.Float:
								valueProp = "tween.floatValue";
								break;
							case ePropertyType.Vector2:
								valueProp = "tween.vector2Value";
								break;
							case ePropertyType.Vector3:
								valueProp = tq.Equals(data.generics[1]) ? "Quaternion.Euler(tween.vector3Value)" : "tween.vector3Value";
								break;
							case ePropertyType.Color:
								valueProp = "tween.colorValue";
								break;
						}
						string component = "transform";
						if (j > 0) {
							component = string.Format("m{0}.component", data.generics[0].Name);
							code.AppendLine(string.Format("\t\t\t\t\t{0}if ({1} != null) {{",
								count == 0 ? "" : "} else ", component));
						}
						code.AppendLine(string.Format("\t\t\t\t\t{0}PlayTweener<{1}, {2}, {3}, {4}>(tween, {5}, {6});",
							j == 0 ? "" : "\t", data.type.Name, data.generics[0].Name, GetTypeName(data.generics[1]),
							data.generics[2].Name, component, valueProp));
						count++;
					}
					if (j > 0 && count > 0) {
						code.AppendLine("\t\t\t\t\t}");
					}
				}
				code.AppendLine("\t\t\t\t\tbreak;");
			}
			code.AppendLine("\t\t\t}");
			code.AppendLine("\t\t}");
			code.AppendLine();
			code.AppendLine("\t}");
			code.AppendLine();
			code.AppendLine("}");
			string codeContent = code.ToString();
			if (!File.Exists(scriptPath) || File.ReadAllText(scriptPath, Encoding.UTF8) != codeContent) {
				File.WriteAllText(scriptPath, code.ToString());
				AssetDatabase.Refresh();
			}
		}

		private struct TweenerTypeData {
			public readonly TweenerDataAttribute attr;
			public readonly ePropertyType propertyType;
			public readonly Type type;
			public readonly Type baseType;
			public readonly Type[] generics;
			public TweenerTypeData(TweenerDataAttribute attr, ePropertyType propertyType, Type type, Type baseType, Type[] generics) {
				this.attr = attr;
				this.propertyType = propertyType;
				this.type = type;
				this.baseType = baseType;
				this.generics = generics;
			}
		}

		private static int WriteEnum(StringBuilder code, string indent, Type et, string modifier, string name, List<string> enums, Dictionary<string, int> values) {
			List<string> ns = new List<string>();
			List<int> vs = new List<int>();
			ns.Add("None");
			vs.Add(0);
			int max = 0;
			if (et != null) {
				string[] ens = Enum.GetNames(et);
				Array evs = Enum.GetValues(et);
				for (int i = 0, imax = ens.Length; i < imax; i++) {
					string n = ens[i];
					if (ns.Contains(n)) { continue; }
					int v = (int)evs.GetValue(i);
					ns.Add(n);
					vs.Add(v);
					if (v > max) { max = v; }
				}
			}
			foreach (string n in enums) {
				if (ns.Contains(n)) { continue; }
				max++;
				ns.Add(n);
				vs.Add(max);
			}
			code.AppendLine(string.Format("{0}{1} enum {2} {{", indent, modifier, name));
			code.Append(indent);
			code.Append("\t");
			for (int i = 0, imax = ns.Count - 1; i <= imax; i++) {
				code.AppendFormat("{0} = {1}", ns[i], vs[i]);
				if (i < imax) {
					if (((i + 1) % 5) == 0) {
						code.AppendLine(",");
						code.Append(indent);
						code.Append("\t");
					} else {
						code.Append(", ");
					}
				}
				values.Add(ns[i], vs[i]);
			}
			code.AppendLine();
			code.AppendLine(string.Format("{0}}}", indent));
			return max;
		}

		private static string GetTypeName(Type type) {
			InitTypes();
			if (s_type_float.Equals(type)) { return "float"; }
			return type.Name;
		}

		private static string GetTypeNameSpace(Type type) {
			InitTypes();
			if (s_type_float.Equals(type)) { return null; }
			return type.Namespace;
		}

		private static bool s_types_inited = false;
		private static Type s_type_float;
		private static void InitTypes() {
			if (s_types_inited) { return; }
			s_type_float = typeof(float);
		}

		private class FieldAndLabel {
			public readonly ePropertyType field;
			public readonly GUIContent label;
			public FieldAndLabel(ePropertyType field, GUIContent label) {
				this.field = field;
				this.label = label;
			}
		}

		private static List<FieldAndLabel> tween_field_and_label = new List<FieldAndLabel>();
		private static GUIContent invalid_tween_type;
		private void GetTweenValueFieldAndLabel(int type, out ePropertyType field, out GUIContent label) {
			FieldAndLabel fl = type >= 0 && type < tween_field_and_label.Count ? tween_field_and_label[type] : null;
			if (fl == null) {
				//Debug.LogErrorFormat("Invalid Tween Type '{0}' ！", type);
				field = ePropertyType.Float;
				if (invalid_tween_type == null) {
					invalid_tween_type = new GUIContent("Invalid");
				}
				label = invalid_tween_type;
				return;
			}
			field = fl.field;
			label = fl.label;
		}

		private static GUIContent[] tween_type_contents;
		private static int[] tween_type_values;

		public override void OnInspectorGUI() {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DisableMode"));
			EditorGUILayout.LabelField("Tweens");
			SerializedProperty tweens = serializedObject.FindProperty("m_Tweens");
			int toRemoveAt = -1;
			for (int i = 0, imax = tweens.arraySize; i < imax; i++) {
				SerializedProperty tween = tweens.GetArrayElementAtIndex(i);
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(12f);
				Color cachedBgColor = GUI.backgroundColor;
				if ((i & 1) == 0) {
					GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1f);
				}
				EditorGUILayout.BeginVertical("CN Box", GUILayout.MinHeight(10f));
				GUI.backgroundColor = cachedBgColor;
				SerializedProperty pTweenType = tween.FindPropertyRelative("tweenType");
				EditorGUILayout.BeginHorizontal();
				int ntt = EditorGUILayout.IntPopup(pTweenType.intValue, tween_type_contents, tween_type_values);
				if (ntt != pTweenType.intValue) { pTweenType.intValue = ntt; }
				if (GUILayout.Button("x", GUILayout.Width(24f))) { toRemoveAt = i; }
				EditorGUILayout.EndHorizontal();
				SerializedProperty pEaseDefine = tween.FindPropertyRelative("easeDefinition");
				TweenComponentBase.eEaseDefinition easeDefine = (TweenComponentBase.eEaseDefinition)pEaseDefine.enumValueIndex;
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("group"));
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("autoPlay"));
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("unscaled"));
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("delay"));
				bool durationInCurve = easeDefine == TweenComponentBase.eEaseDefinition.CurveWithDuration;
				EditorGUI.BeginDisabledGroup(durationInCurve);
				if (durationInCurve) {
					float duration = 0f;
					AnimationCurve curve = tween.FindPropertyRelative("curve").animationCurveValue;
					if (curve != null && curve.length >= 2) {
						duration = curve[curve.length - 1].time;
					}
					Color cachedGUIColor = GUI.color;
					if (duration <= 0f) {
						GUI.color = Color.red;
					}
					EditorGUILayout.FloatField("Duration", duration);
					GUI.color = cachedGUIColor;
				} else {
					EditorGUILayout.PropertyField(tween.FindPropertyRelative("duration"));
				}
				EditorGUI.EndDisabledGroup();
				EditorGUILayout.PropertyField(pEaseDefine);
				if (easeDefine == TweenComponentBase.eEaseDefinition.EaseFunction) {
					EditorGUILayout.PropertyField(tween.FindPropertyRelative("ease"));
				} else {
					EditorGUILayout.PropertyField(tween.FindPropertyRelative("curve"));
				}
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("loop"));
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("loopInterval"));
				EditorGUILayout.PropertyField(tween.FindPropertyRelative("intervalKeepAtEnd"));
				ePropertyType valueField;
				GUIContent valueLabel;
				GetTweenValueFieldAndLabel(pTweenType.intValue, out valueField, out valueLabel);
				SerializedProperty property = null;
				switch (valueField) {
					case ePropertyType.Float:
						property = tween.FindPropertyRelative("floatValue");
						break;
					case ePropertyType.Vector2:
						property = tween.FindPropertyRelative("vector2Value");
						break;
					case ePropertyType.Vector3:
						property = tween.FindPropertyRelative("vector3Value");
						break;
					case ePropertyType.Color:
						property = tween.FindPropertyRelative("colorValue");
						break;
				}
				if (property != null) {
					EditorGUILayout.PropertyField(property, valueLabel);
				}
				EditorGUILayout.BeginHorizontal();
				//EditorGUILayout.LabelField("");
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Copy Current") && property != null) {
					List<TweenerTypeData> datas;
					if (type_tweeners.TryGetValue(pTweenType.intValue, out datas)) {
						TweenComponentBase self = target as TweenComponentBase;
						foreach (TweenerTypeData data in datas) {
							bool tempComponent = false;
							Component component = self.GetComponent(data.generics[0]);
							if (component == null) {
								if (Attribute.GetCustomAttribute(data.generics[0], typeof(TweenerComponentCreateIfMissingAttribute)) != null) {
									component = self.gameObject.AddComponent(data.generics[0]);
									tempComponent = true;
								}
							}
							if (component == null) { continue; }
							Type tt = typeof(Tweener);
							MethodInfo methodGet = tt.GetMethod("GetTweener");
							methodGet = methodGet.MakeGenericMethod(data.type, data.generics[0]);
							object tweener = methodGet.Invoke(null, new object[] { component });
							PropertyInfo p = tweener.GetType().GetProperty("Current");
							object value = p.GetValue(tweener, null);
							switch (valueField) {
								case ePropertyType.Float:
									property.floatValue = (float)value;
									break;
								case ePropertyType.Vector2:
									property.vector2Value = (Vector2)value;
									break;
								case ePropertyType.Vector3:
									property.vector3Value = value.GetType().Equals(typeof(Quaternion)) ?
										((Quaternion)value).eulerAngles :  (Vector3)value;
									break;
								case ePropertyType.Color:
									property.colorValue = (Color)value;
									break;
							}
							if (tempComponent) { DestroyImmediate(component); }
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(12f);
			if (GUILayout.Button("Add Tween")) {
				tweens.InsertArrayElementAtIndex(tweens.arraySize);
			}
			EditorGUILayout.EndHorizontal();
			if (toRemoveAt >= 0) {
				tweens.DeleteArrayElementAtIndex(toRemoveAt);
			}
			serializedObject.ApplyModifiedProperties();
		}

	}

}
