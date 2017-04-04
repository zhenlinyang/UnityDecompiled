using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
	internal static class SpriteEditorUtility
	{
		public static Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset)
		{
			Vector2 result;
			switch (alignment)
			{
			case SpriteAlignment.Center:
				result = new Vector2(0.5f, 0.5f);
				break;
			case SpriteAlignment.TopLeft:
				result = new Vector2(0f, 1f);
				break;
			case SpriteAlignment.TopCenter:
				result = new Vector2(0.5f, 1f);
				break;
			case SpriteAlignment.TopRight:
				result = new Vector2(1f, 1f);
				break;
			case SpriteAlignment.LeftCenter:
				result = new Vector2(0f, 0.5f);
				break;
			case SpriteAlignment.RightCenter:
				result = new Vector2(1f, 0.5f);
				break;
			case SpriteAlignment.BottomLeft:
				result = new Vector2(0f, 0f);
				break;
			case SpriteAlignment.BottomCenter:
				result = new Vector2(0.5f, 0f);
				break;
			case SpriteAlignment.BottomRight:
				result = new Vector2(1f, 0f);
				break;
			case SpriteAlignment.Custom:
				result = customOffset;
				break;
			default:
				result = Vector2.zero;
				break;
			}
			return result;
		}

		public static Rect RoundedRect(Rect rect)
		{
			return new Rect((float)Mathf.RoundToInt(rect.xMin), (float)Mathf.RoundToInt(rect.yMin), (float)Mathf.RoundToInt(rect.width), (float)Mathf.RoundToInt(rect.height));
		}

		public static Rect RoundToInt(Rect r)
		{
			r.xMin = (float)Mathf.RoundToInt(r.xMin);
			r.yMin = (float)Mathf.RoundToInt(r.yMin);
			r.xMax = (float)Mathf.RoundToInt(r.xMax);
			r.yMax = (float)Mathf.RoundToInt(r.yMax);
			return r;
		}

		public static Rect ClampedRect(Rect rect, Rect clamp, bool maintainSize)
		{
			Rect result = new Rect(rect);
			if (maintainSize)
			{
				Vector2 center = rect.center;
				if (center.x + Mathf.Abs(rect.width) * 0.5f > clamp.xMax)
				{
					center.x = clamp.xMax - rect.width * 0.5f;
				}
				if (center.x - Mathf.Abs(rect.width) * 0.5f < clamp.xMin)
				{
					center.x = clamp.xMin + rect.width * 0.5f;
				}
				if (center.y + Mathf.Abs(rect.height) * 0.5f > clamp.yMax)
				{
					center.y = clamp.yMax - rect.height * 0.5f;
				}
				if (center.y - Mathf.Abs(rect.height) * 0.5f < clamp.yMin)
				{
					center.y = clamp.yMin + rect.height * 0.5f;
				}
				result.center = center;
			}
			else
			{
				if (result.width > 0f)
				{
					result.xMin = Mathf.Max(rect.xMin, clamp.xMin);
					result.xMax = Mathf.Min(rect.xMax, clamp.xMax);
				}
				else
				{
					result.xMin = Mathf.Min(rect.xMin, clamp.xMax);
					result.xMax = Mathf.Max(rect.xMax, clamp.xMin);
				}
				if (result.height > 0f)
				{
					result.yMin = Mathf.Max(rect.yMin, clamp.yMin);
					result.yMax = Mathf.Min(rect.yMax, clamp.yMax);
				}
				else
				{
					result.yMin = Mathf.Min(rect.yMin, clamp.yMax);
					result.yMax = Mathf.Max(rect.yMax, clamp.yMin);
				}
			}
			result.width = Mathf.Abs(result.width);
			result.height = Mathf.Abs(result.height);
			return result;
		}

		public static void DrawBox(Rect position)
		{
			Vector3[] array = new Vector3[5];
			int num = 0;
			array[num++] = new Vector3(position.xMin, position.yMin, 0f);
			array[num++] = new Vector3(position.xMax, position.yMin, 0f);
			array[num++] = new Vector3(position.xMax, position.yMax, 0f);
			array[num++] = new Vector3(position.xMin, position.yMax, 0f);
			SpriteEditorUtility.DrawLine(array[0], array[1]);
			SpriteEditorUtility.DrawLine(array[1], array[2]);
			SpriteEditorUtility.DrawLine(array[2], array[3]);
			SpriteEditorUtility.DrawLine(array[3], array[0]);
		}

		public static void DrawLine(Vector3 p1, Vector3 p2)
		{
			GL.Vertex(p1);
			GL.Vertex(p2);
		}

		public static void BeginLines(Color color)
		{
			HandleUtility.ApplyWireMaterial();
			GL.PushMatrix();
			GL.MultMatrix(Handles.matrix);
			GL.Begin(1);
			GL.Color(color);
		}

		public static void EndLines()
		{
			GL.End();
			GL.PopMatrix();
		}

		public static void FourIntFields(Vector2 rectSize, GUIContent label, GUIContent labelX, GUIContent labelY, GUIContent labelZ, GUIContent labelW, ref int x, ref int y, ref int z, ref int w)
		{
			Rect rect = GUILayoutUtility.GetRect(rectSize.x, rectSize.y);
			Rect position = rect;
			position.width = EditorGUIUtility.labelWidth;
			position.height = 16f;
			GUI.Label(position, label);
			Rect position2 = rect;
			position2.width -= EditorGUIUtility.labelWidth;
			position2.height = 16f;
			position2.x += EditorGUIUtility.labelWidth;
			position2.width /= 2f;
			position2.width -= 2f;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 13f;
			GUI.SetNextControlName("FourIntFields_x");
			x = EditorGUI.IntField(position2, labelX, x);
			position2.x += position2.width + 5f;
			GUI.SetNextControlName("FourIntFields_y");
			y = EditorGUI.IntField(position2, labelY, y);
			position2.y += 16f;
			position2.x -= position2.width + 5f;
			GUI.SetNextControlName("FourIntFields_z");
			z = EditorGUI.IntField(position2, labelZ, z);
			position2.x += position2.width + 5f;
			GUI.SetNextControlName("FourIntFields_w");
			w = EditorGUI.IntField(position2, labelW, w);
			EditorGUIUtility.labelWidth = labelWidth;
		}
	}
}
