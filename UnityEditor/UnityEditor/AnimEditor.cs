using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class AnimEditor : ScriptableObject
	{
		private struct ChangedCurvesPerClip
		{
			public List<EditorCurveBinding> bindings;

			public List<AnimationCurve> curves;
		}

		private static List<AnimEditor> s_AnimationWindows = new List<AnimEditor>();

		[SerializeField]
		private SplitterState m_HorizontalSplitter;

		[SerializeField]
		private AnimationWindowState m_State;

		[SerializeField]
		private DopeSheetEditor m_DopeSheet;

		[SerializeField]
		private AnimationWindowHierarchy m_Hierarchy;

		[SerializeField]
		private AnimationWindowClipPopup m_ClipPopup;

		[SerializeField]
		private AnimationEventTimeLine m_Events;

		[SerializeField]
		private CurveEditor m_CurveEditor;

		[SerializeField]
		private AnimEditorOverlay m_Overlay;

		[SerializeField]
		private EditorWindow m_OwnerWindow;

		[NonSerialized]
		private Rect m_Position;

		[NonSerialized]
		private bool m_TriggerFraming;

		[NonSerialized]
		private bool m_StylesInitialized;

		[NonSerialized]
		private bool m_Initialized;

		internal static PrefColor kEulerXColor = new PrefColor("Testing/EulerX", 1f, 0f, 1f, 1f);

		internal static PrefColor kEulerYColor = new PrefColor("Testing/EulerY", 1f, 1f, 0f, 1f);

		internal static PrefColor kEulerZColor = new PrefColor("Testing/EulerZ", 0f, 1f, 1f, 1f);

		internal static PrefKey kAnimationPlayToggle = new PrefKey("Animation/Play Animation", " ");

		internal static PrefKey kAnimationPrevFrame = new PrefKey("Animation/Previous Frame", ",");

		internal static PrefKey kAnimationNextFrame = new PrefKey("Animation/Next Frame", ".");

		internal static PrefKey kAnimationPrevKeyframe = new PrefKey("Animation/Previous Keyframe", "&,");

		internal static PrefKey kAnimationNextKeyframe = new PrefKey("Animation/Next Keyframe", "&.");

		internal static PrefKey kAnimationFirstKey = new PrefKey("Animation/First Keyframe", "#,");

		internal static PrefKey kAnimationLastKey = new PrefKey("Animation/Last Keyframe", "#.");

		internal static PrefKey kAnimationRecordKeyframe = new PrefKey("Animation/Record Keyframe", "k");

		internal static PrefKey kAnimationShowCurvesToggle = new PrefKey("Animation/Show Curves", "c");

		internal const int kSliderThickness = 15;

		internal const int kLayoutRowHeight = 18;

		internal const int kIntFieldWidth = 35;

		internal const int kHierarchyMinWidth = 300;

		internal const int kToggleButtonWidth = 80;

		internal const float kDisabledRulerAlpha = 0.12f;

		public bool stateDisabled
		{
			get
			{
				return this.m_State.disabled;
			}
		}

		private float hierarchyWidth
		{
			get
			{
				return (float)this.m_HorizontalSplitter.realSizes[0];
			}
		}

		private float contentWidth
		{
			get
			{
				return (float)this.m_HorizontalSplitter.realSizes[1];
			}
		}

		public AnimationWindowState state
		{
			get
			{
				return this.m_State;
			}
		}

		public AnimationWindowSelection selection
		{
			get
			{
				return this.m_State.selection;
			}
		}

		public AnimationWindowSelectionItem selectedItem
		{
			get
			{
				return this.m_State.selectedItem;
			}
			set
			{
				this.m_State.selectedItem = value;
			}
		}

		public IAnimationWindowControl controlInterface
		{
			get
			{
				return this.state.controlInterface;
			}
		}

		public IAnimationWindowControl overrideControlInterface
		{
			get
			{
				return this.state.overrideControlInterface;
			}
			set
			{
				this.state.overrideControlInterface = value;
			}
		}

		private bool triggerFraming
		{
			get
			{
				return this.m_TriggerFraming;
			}
			set
			{
				this.m_TriggerFraming = value;
			}
		}

		internal CurveEditor curveEditor
		{
			get
			{
				return this.m_CurveEditor;
			}
		}

		internal DopeSheetEditor dopeSheetEditor
		{
			get
			{
				return this.m_DopeSheet;
			}
		}

		public static List<AnimEditor> GetAllAnimationWindows()
		{
			return AnimEditor.s_AnimationWindows;
		}

		public void OnAnimEditorGUI(EditorWindow parent, Rect position)
		{
			this.m_DopeSheet.m_Owner = parent;
			this.m_OwnerWindow = parent;
			this.m_Position = position;
			if (!this.m_Initialized)
			{
				this.Initialize();
			}
			this.m_State.OnGUI();
			if (this.m_State.disabled && this.controlInterface.recording)
			{
				this.m_State.StopRecording();
			}
			this.SynchronizeLayout();
			using (new EditorGUI.DisabledScope(this.m_State.disabled || this.m_State.animatorIsOptimized))
			{
				this.OverlayEventOnGUI();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				SplitterGUILayout.BeginHorizontalSplit(this.m_HorizontalSplitter, new GUILayoutOption[0]);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
				this.PlayControlsOnGUI();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
				this.LinkOptionsOnGUI();
				this.ClipSelectionDropDownOnGUI();
				GUILayout.FlexibleSpace();
				this.FrameRateInputFieldOnGUI();
				this.AddKeyframeButtonOnGUI();
				this.AddEventButtonOnGUI();
				GUILayout.EndHorizontal();
				this.HierarchyOnGUI();
				GUILayout.BeginHorizontal(AnimationWindowStyles.miniToolbar, new GUILayoutOption[0]);
				this.TabSelectionOnGUI();
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				Rect rect = GUILayoutUtility.GetRect(this.contentWidth, 18f);
				Rect rect2 = GUILayoutUtility.GetRect(this.contentWidth, 18f);
				Rect rect3 = GUILayoutUtility.GetRect(this.contentWidth, this.contentWidth, 0f, 3.40282347E+38f, new GUILayoutOption[]
				{
					GUILayout.ExpandHeight(true)
				});
				this.MainContentOnGUI(rect3);
				this.TimeRulerOnGUI(rect);
				this.EventLineOnGUI(rect2);
				GUILayout.EndVertical();
				SplitterGUILayout.EndHorizontalSplit();
				GUILayout.EndHorizontal();
				this.OverlayOnGUI(rect3);
				this.RenderEventTooltip();
				this.HandleHotKeys();
			}
		}

		private void MainContentOnGUI(Rect contentLayoutRect)
		{
			if (this.m_State.animatorIsOptimized)
			{
				Vector2 vector = GUI.skin.label.CalcSize(AnimationWindowStyles.animatorOptimizedText);
				Rect position = new Rect(contentLayoutRect.x + contentLayoutRect.width * 0.5f - vector.x * 0.5f, contentLayoutRect.y + contentLayoutRect.height * 0.5f - vector.y * 0.5f, vector.x, vector.y);
				GUI.Label(position, AnimationWindowStyles.animatorOptimizedText);
			}
			else
			{
				if (this.m_State.disabled)
				{
					this.SetupWizardOnGUI(contentLayoutRect);
				}
				else
				{
					if (this.triggerFraming && Event.current.type == EventType.Repaint)
					{
						this.m_DopeSheet.FrameClip();
						this.m_CurveEditor.FrameClip(true, true);
						this.triggerFraming = false;
					}
					if (this.m_State.showCurveEditor)
					{
						this.CurveEditorOnGUI(contentLayoutRect);
					}
					else
					{
						this.DopeSheetOnGUI(contentLayoutRect);
					}
				}
				this.HandleCopyPaste();
			}
		}

		private void OverlayEventOnGUI()
		{
			if (!this.m_State.animatorIsOptimized)
			{
				if (!this.m_State.disabled)
				{
					Rect position = new Rect(this.hierarchyWidth - 1f, 0f, this.contentWidth - 15f, this.m_Position.height - 15f);
					GUI.BeginGroup(position);
					this.m_Overlay.HandleEvents();
					GUI.EndGroup();
				}
			}
		}

		private void OverlayOnGUI(Rect contentRect)
		{
			if (!this.m_State.animatorIsOptimized)
			{
				if (!this.m_State.disabled)
				{
					if (Event.current.type == EventType.Repaint)
					{
						Rect rect = new Rect(contentRect.xMin, contentRect.yMin, contentRect.width - 15f, contentRect.height - 15f);
						Rect position = new Rect(this.hierarchyWidth - 1f, 0f, this.contentWidth - 15f, this.m_Position.height - 15f);
						GUI.BeginGroup(position);
						Rect rect2 = new Rect(0f, 0f, position.width, position.height);
						Rect contentRect2 = rect;
						contentRect2.position -= position.min;
						this.m_Overlay.OnGUI(rect2, contentRect2);
						GUI.EndGroup();
					}
				}
			}
		}

		public void Update()
		{
			if (!(this.m_State == null))
			{
				this.PlaybackUpdate();
			}
		}

		public void OnEnable()
		{
			base.hideFlags = HideFlags.HideAndDontSave;
			AnimEditor.s_AnimationWindows.Add(this);
			if (this.m_State == null)
			{
				this.m_State = (ScriptableObject.CreateInstance(typeof(AnimationWindowState)) as AnimationWindowState);
				this.m_State.hideFlags = HideFlags.HideAndDontSave;
				this.m_State.animEditor = this;
				this.InitializeHorizontalSplitter();
				this.InitializeClipSelection();
				this.InitializeDopeSheet();
				this.InitializeEvents();
				this.InitializeCurveEditor();
				this.InitializeOverlay();
			}
			this.InitializeNonserializedValues();
			this.m_State.timeArea = ((!this.m_State.showCurveEditor) ? this.m_DopeSheet : this.m_CurveEditor);
			this.m_DopeSheet.state = this.m_State;
			this.m_ClipPopup.state = this.m_State;
			this.m_Overlay.state = this.m_State;
			CurveEditor expr_E9 = this.m_CurveEditor;
			expr_E9.curvesUpdated = (CurveEditor.CallbackFunction)Delegate.Combine(expr_E9.curvesUpdated, new CurveEditor.CallbackFunction(this.SaveChangedCurvesFromCurveEditor));
			this.m_CurveEditor.OnEnable();
		}

		public void OnDisable()
		{
			AnimEditor.s_AnimationWindows.Remove(this);
			if (this.m_CurveEditor != null)
			{
				CurveEditor expr_1F = this.m_CurveEditor;
				expr_1F.curvesUpdated = (CurveEditor.CallbackFunction)Delegate.Remove(expr_1F.curvesUpdated, new CurveEditor.CallbackFunction(this.SaveChangedCurvesFromCurveEditor));
				this.m_CurveEditor.OnDisable();
			}
			if (this.m_DopeSheet != null)
			{
				this.m_DopeSheet.OnDisable();
			}
			this.m_State.OnDisable();
		}

		public void OnDestroy()
		{
			if (this.m_CurveEditor != null)
			{
				this.m_CurveEditor.OnDestroy();
			}
			UnityEngine.Object.DestroyImmediate(this.m_State);
		}

		public void OnSelectionChanged()
		{
			this.m_State.OnSelectionChanged();
			this.triggerFraming = true;
			this.Repaint();
		}

		public void OnStartLiveEdit()
		{
			this.SaveCurveEditorKeySelection();
		}

		public void OnEndLiveEdit()
		{
			this.UpdateSelectedKeysToCurveEditor();
			this.controlInterface.ResampleAnimation();
		}

		public void OnLostFocus()
		{
			if (this.m_Hierarchy != null)
			{
				this.m_Hierarchy.EndNameEditing(true);
			}
			EditorGUI.EndEditingActiveTextField();
		}

		private void PlaybackUpdate()
		{
			if (this.m_State.disabled && this.controlInterface.playing)
			{
				this.controlInterface.StopPlayback();
			}
			if (this.controlInterface.PlaybackUpdate())
			{
				this.Repaint();
			}
		}

		private void SetupWizardOnGUI(Rect position)
		{
			GUI.Label(position, GUIContent.none, AnimationWindowStyles.dopeSheetBackground);
			Rect position2 = new Rect(position.x, position.y, position.width - 15f, position.height - 15f);
			GUI.BeginClip(position2);
			GUI.enabled = true;
			this.m_State.showCurveEditor = false;
			this.m_State.timeArea = this.m_DopeSheet;
			this.m_State.timeArea.SetShownHRangeInsideMargins(0f, 1f);
			bool flag = this.m_State.activeGameObject && !EditorUtility.IsPersistent(this.m_State.activeGameObject);
			if (flag)
			{
				string arg = (this.m_State.activeRootGameObject || this.m_State.activeAnimationClip) ? AnimationWindowStyles.animationClip.text : AnimationWindowStyles.animatorAndAnimationClip.text;
				string t = string.Format(AnimationWindowStyles.formatIsMissing.text, this.m_State.activeGameObject.name, arg);
				GUIContent content = GUIContent.Temp(t);
				Vector2 vector = GUI.skin.label.CalcSize(content);
				Rect position3 = new Rect(position2.width * 0.5f - vector.x * 0.5f, position2.height * 0.5f - vector.y * 0.5f, vector.x, vector.y);
				GUI.Label(position3, content);
				Rect position4 = new Rect(position2.width * 0.5f - 35f, position3.yMax + 3f, 70f, 20f);
				if (GUI.Button(position4, AnimationWindowStyles.create))
				{
					if (AnimationWindowUtility.InitializeGameobjectForAnimation(this.m_State.activeGameObject))
					{
						Component closestAnimationPlayerComponentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(this.m_State.activeGameObject.transform);
						this.m_State.selection.UpdateClip(this.m_State.selectedItem, AnimationUtility.GetAnimationClips(closestAnimationPlayerComponentInParents.gameObject)[0]);
						GUIUtility.ExitGUI();
					}
				}
			}
			else
			{
				Color color = GUI.color;
				GUI.color = Color.gray;
				Vector2 vector2 = GUI.skin.label.CalcSize(AnimationWindowStyles.noAnimatableObjectSelectedText);
				Rect position5 = new Rect(position2.width * 0.5f - vector2.x * 0.5f, position2.height * 0.5f - vector2.y * 0.5f, vector2.x, vector2.y);
				GUI.Label(position5, AnimationWindowStyles.noAnimatableObjectSelectedText);
				GUI.color = color;
			}
			GUI.EndClip();
			GUI.enabled = false;
		}

		private void EventLineOnGUI(Rect eventsRect)
		{
			eventsRect.width -= 15f;
			GUI.Label(eventsRect, GUIContent.none, AnimationWindowStyles.eventBackground);
			using (new EditorGUI.DisabledScope(this.m_State.selectedItem == null || !this.m_State.selectedItem.animationIsEditable))
			{
				this.m_Events.EventLineGUI(eventsRect, this.m_State);
			}
		}

		private void RenderEventTooltip()
		{
			this.m_Events.DrawInstantTooltip(this.m_Position);
		}

		private void TabSelectionOnGUI()
		{
			GUILayout.FlexibleSpace();
			EditorGUI.BeginChangeCheck();
			GUILayout.Toggle(!this.m_State.showCurveEditor, AnimationWindowStyles.dopesheet, AnimationWindowStyles.miniToolbarButton, new GUILayoutOption[]
			{
				GUILayout.Width(80f)
			});
			GUILayout.Toggle(this.m_State.showCurveEditor, AnimationWindowStyles.curves, AnimationWindowStyles.miniToolbarButton, new GUILayoutOption[]
			{
				GUILayout.Width(80f)
			});
			if (EditorGUI.EndChangeCheck())
			{
				this.SwitchBetweenCurvesAndDopesheet();
			}
			else if (AnimEditor.kAnimationShowCurvesToggle.activated)
			{
				this.SwitchBetweenCurvesAndDopesheet();
				Event.current.Use();
			}
		}

		private void HierarchyOnGUI()
		{
			Rect rect = GUILayoutUtility.GetRect(this.hierarchyWidth, this.hierarchyWidth, 0f, 3.40282347E+38f, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true)
			});
			if (!this.m_State.disabled)
			{
				this.m_Hierarchy.OnGUI(rect);
			}
		}

		private void FrameRateInputFieldOnGUI()
		{
			AnimationWindowSelectionItem selectedItem = this.m_State.selectedItem;
			using (new EditorGUI.DisabledScope(selectedItem == null || !selectedItem.animationIsEditable))
			{
				GUILayout.Label(AnimationWindowStyles.samples, AnimationWindowStyles.toolbarLabel, new GUILayoutOption[0]);
				EditorGUI.BeginChangeCheck();
				int num = EditorGUILayout.DelayedIntField((int)this.m_State.clipFrameRate, EditorStyles.toolbarTextField, new GUILayoutOption[]
				{
					GUILayout.Width(35f)
				});
				if (EditorGUI.EndChangeCheck())
				{
					this.m_State.clipFrameRate = (float)num;
					this.UpdateSelectedKeysToCurveEditor();
				}
			}
		}

		private void ClipSelectionDropDownOnGUI()
		{
			this.m_ClipPopup.OnGUI();
		}

		private void DopeSheetOnGUI(Rect position)
		{
			Rect rect = new Rect(position.xMin, position.yMin, position.width - 15f, position.height);
			if (Event.current.type == EventType.Repaint)
			{
				this.m_DopeSheet.rect = rect;
				this.m_DopeSheet.SetTickMarkerRanges();
				this.m_DopeSheet.RecalculateBounds();
			}
			if (!this.m_State.showCurveEditor)
			{
				Rect position2 = new Rect(position.xMin, position.yMin, position.width - 15f, position.height - 15f);
				Rect position3 = new Rect(position2.xMin, position2.yMin, position2.width, 16f);
				this.m_DopeSheet.BeginViewGUI();
				GUI.Label(position, GUIContent.none, AnimationWindowStyles.dopeSheetBackground);
				if (!this.m_State.disabled)
				{
					this.m_DopeSheet.TimeRuler(position2, this.m_State.frameRate, false, true, 0.12f, this.m_State.timeFormat);
					this.m_DopeSheet.DrawMasterDopelineBackground(position3);
				}
				this.m_DopeSheet.OnGUI(position2, this.m_State.hierarchyState.scrollPos * -1f);
				this.m_DopeSheet.EndViewGUI();
				Rect position4 = new Rect(rect.xMax, rect.yMin, 15f, position2.height);
				float height = this.m_Hierarchy.GetTotalRect().height;
				float bottomValue = Mathf.Max(height, this.m_Hierarchy.GetContentSize().y);
				this.m_State.hierarchyState.scrollPos.y = GUI.VerticalScrollbar(position4, this.m_State.hierarchyState.scrollPos.y, height, 0f, bottomValue);
				if (this.m_DopeSheet.spritePreviewLoading)
				{
					this.Repaint();
				}
			}
		}

		private void CurveEditorOnGUI(Rect position)
		{
			if (Event.current.type == EventType.Repaint)
			{
				this.m_CurveEditor.rect = position;
				this.m_CurveEditor.SetTickMarkerRanges();
			}
			Rect position2 = new Rect(position.xMin, position.yMin, position.width - 15f, position.height - 15f);
			this.m_CurveEditor.vSlider = this.m_State.showCurveEditor;
			this.m_CurveEditor.hSlider = this.m_State.showCurveEditor;
			if (Event.current.type == EventType.Layout)
			{
				this.UpdateCurveEditorData();
			}
			this.m_CurveEditor.BeginViewGUI();
			if (!this.m_State.disabled)
			{
				GUI.Box(position2, GUIContent.none, AnimationWindowStyles.curveEditorBackground);
				this.m_CurveEditor.GridGUI();
			}
			EditorGUI.BeginChangeCheck();
			this.m_CurveEditor.CurveGUI();
			if (EditorGUI.EndChangeCheck())
			{
				this.SaveChangedCurvesFromCurveEditor();
			}
			this.m_CurveEditor.EndViewGUI();
		}

		private void TimeRulerOnGUI(Rect timeRulerRect)
		{
			Rect position = new Rect(timeRulerRect.xMin, timeRulerRect.yMin, timeRulerRect.width - 15f, timeRulerRect.height);
			GUI.Box(timeRulerRect, GUIContent.none, EditorStyles.toolbarButton);
			this.m_State.timeArea.TimeRuler(position, this.m_State.frameRate, true, false, 1f, this.m_State.timeFormat);
			if (!this.m_State.disabled)
			{
				GUI.BeginGroup(position);
				Rect rect = new Rect(0f, 0f, position.width, position.height);
				this.RenderClipOverlay(rect);
				this.RenderSelectionOverlay(rect);
				GUI.EndGroup();
			}
		}

		private void AddEventButtonOnGUI()
		{
			AnimationWindowSelectionItem selectedItem = this.m_State.selectedItem;
			if (selectedItem != null)
			{
				using (new EditorGUI.DisabledScope(!selectedItem.animationIsEditable))
				{
					if (GUILayout.Button(AnimationWindowStyles.addEventContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
					{
						this.m_Events.AddEvent(this.m_State.currentTime - selectedItem.timeOffset, selectedItem.rootGameObject, selectedItem.animationClip);
					}
				}
			}
		}

		private void AddKeyframeButtonOnGUI()
		{
			bool flag = this.m_State.selection.Find((AnimationWindowSelectionItem selectedItem) => selectedItem.animationIsEditable);
			using (new EditorGUI.DisabledScope(!flag))
			{
				if (GUILayout.Button(AnimationWindowStyles.addKeyframeContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
				{
					this.SaveCurveEditorKeySelection();
					AnimationKeyTime time = AnimationKeyTime.Time(this.m_State.currentTime, this.m_State.frameRate);
					AnimationWindowUtility.AddSelectedKeyframes(this.m_State, time);
					this.UpdateSelectedKeysToCurveEditor();
					GUIUtility.ExitGUI();
				}
			}
		}

		private void PlayControlsOnGUI()
		{
			using (new EditorGUI.DisabledScope(!this.controlInterface.canRecord))
			{
				this.RecordButtonOnGUI();
			}
			if (GUILayout.Button(AnimationWindowStyles.firstKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.controlInterface.GoToFirstKeyframe();
				EditorGUI.EndEditingActiveTextField();
			}
			if (GUILayout.Button(AnimationWindowStyles.prevKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.controlInterface.GoToPreviousKeyframe();
				EditorGUI.EndEditingActiveTextField();
			}
			using (new EditorGUI.DisabledScope(!this.controlInterface.canPlay))
			{
				this.PlayButtonOnGUI();
			}
			if (GUILayout.Button(AnimationWindowStyles.nextKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.controlInterface.GoToNextKeyframe();
				EditorGUI.EndEditingActiveTextField();
			}
			if (GUILayout.Button(AnimationWindowStyles.lastKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.controlInterface.GoToLastKeyframe();
				EditorGUI.EndEditingActiveTextField();
			}
			GUILayout.FlexibleSpace();
			EditorGUI.BeginChangeCheck();
			int frame = EditorGUILayout.DelayedIntField(this.m_State.currentFrame, EditorStyles.toolbarTextField, new GUILayoutOption[]
			{
				GUILayout.Width(35f)
			});
			if (EditorGUI.EndChangeCheck())
			{
				this.controlInterface.GoToFrame(frame);
			}
		}

		private void LinkOptionsOnGUI()
		{
			if (this.m_State.linkedWithSequencer)
			{
				if (!GUILayout.Toggle(true, AnimationWindowStyles.sequencerLinkContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
				{
					this.m_State.linkedWithSequencer = false;
					this.m_State.selection.Clear();
					GUIUtility.ExitGUI();
				}
			}
		}

		private void HandleHotKeys()
		{
			if (GUI.enabled && !this.m_State.disabled)
			{
				bool flag = false;
				if (AnimEditor.kAnimationPrevKeyframe.activated)
				{
					this.controlInterface.GoToPreviousKeyframe();
					flag = true;
				}
				if (AnimEditor.kAnimationNextKeyframe.activated)
				{
					this.controlInterface.GoToNextKeyframe();
					flag = true;
				}
				if (AnimEditor.kAnimationNextFrame.activated)
				{
					this.controlInterface.GoToNextFrame();
					flag = true;
				}
				if (AnimEditor.kAnimationPrevFrame.activated)
				{
					this.controlInterface.GoToPreviousFrame();
					flag = true;
				}
				if (AnimEditor.kAnimationFirstKey.activated)
				{
					this.controlInterface.GoToFirstKeyframe();
					flag = true;
				}
				if (AnimEditor.kAnimationLastKey.activated)
				{
					this.controlInterface.GoToLastKeyframe();
					flag = true;
				}
				if (flag)
				{
					Event.current.Use();
					this.Repaint();
				}
				if (AnimEditor.kAnimationPlayToggle.activated)
				{
					if (this.controlInterface.playing)
					{
						this.controlInterface.StopPlayback();
					}
					else
					{
						this.controlInterface.StartPlayback();
					}
					Event.current.Use();
				}
				if (AnimEditor.kAnimationRecordKeyframe.activated)
				{
					this.SaveCurveEditorKeySelection();
					AnimationKeyTime time = AnimationKeyTime.Time(this.m_State.currentTime, this.m_State.frameRate);
					AnimationWindowUtility.AddSelectedKeyframes(this.m_State, time);
					this.UpdateSelectedKeysToCurveEditor();
					Event.current.Use();
				}
			}
		}

		private void PlayButtonOnGUI()
		{
			EditorGUI.BeginChangeCheck();
			bool flag = GUILayout.Toggle(this.controlInterface.playing, AnimationWindowStyles.playContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (flag)
				{
					this.controlInterface.StartPlayback();
				}
				else
				{
					this.controlInterface.StopPlayback();
				}
				EditorGUI.EndEditingActiveTextField();
			}
		}

		private void RecordButtonOnGUI()
		{
			EditorGUI.BeginChangeCheck();
			bool flag = GUILayout.Toggle(this.controlInterface.recording, AnimationWindowStyles.recordContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (flag)
				{
					this.m_State.StartRecording();
				}
				else
				{
					this.m_State.StopRecording();
				}
			}
		}

		private void SwitchBetweenCurvesAndDopesheet()
		{
			if (!this.m_State.showCurveEditor)
			{
				this.SwitchToCurveEditor();
			}
			else
			{
				this.SwitchToDopeSheetEditor();
			}
		}

		internal void SwitchToCurveEditor()
		{
			this.m_State.showCurveEditor = true;
			this.UpdateSelectedKeysToCurveEditor();
			AnimationWindowUtility.SyncTimeArea(this.m_DopeSheet, this.m_CurveEditor);
			this.m_State.timeArea = this.m_CurveEditor;
		}

		internal void SwitchToDopeSheetEditor()
		{
			this.m_State.showCurveEditor = false;
			this.UpdateSelectedKeysFromCurveEditor();
			AnimationWindowUtility.SyncTimeArea(this.m_CurveEditor, this.m_DopeSheet);
			this.m_State.timeArea = this.m_DopeSheet;
		}

		private void RenderSelectionOverlay(Rect rect)
		{
			if (!this.m_State.showCurveEditor || this.m_CurveEditor.hasSelection)
			{
				if (this.m_State.showCurveEditor || this.m_State.selectedKeys.Count != 0)
				{
					Bounds bounds = (!this.m_State.showCurveEditor) ? this.m_State.selectionBounds : this.m_CurveEditor.selectionBounds;
					float num = this.m_State.TimeToPixel(bounds.min.x) + rect.xMin;
					float num2 = this.m_State.TimeToPixel(bounds.max.x) + rect.xMin;
					if (num2 - num < 14f)
					{
						float num3 = (num + num2) * 0.5f;
						num = num3 - 7f;
						num2 = num3 + 7f;
					}
					AnimationWindowUtility.DrawRangeOfSelection(rect, num, num2);
				}
			}
		}

		private void RenderClipOverlay(Rect rect)
		{
			Vector2 timeRange = this.m_State.timeRange;
			AnimationWindowUtility.DrawRangeOfClip(rect, this.m_State.TimeToPixel(timeRange.x) + rect.xMin, this.m_State.TimeToPixel(timeRange.y) + rect.xMin);
		}

		private void SynchronizeLayout()
		{
			this.m_HorizontalSplitter.realSizes[1] = (int)Mathf.Min(this.m_Position.width - (float)this.m_HorizontalSplitter.realSizes[0], (float)this.m_HorizontalSplitter.realSizes[1]);
			if (this.selectedItem != null && this.selectedItem.animationClip != null)
			{
				this.m_State.frameRate = this.selectedItem.animationClip.frameRate;
			}
			else
			{
				this.m_State.frameRate = 60f;
			}
		}

		private void SaveChangedCurvesFromCurveEditor()
		{
			this.m_State.SaveKeySelection("Edit Curve");
			Dictionary<AnimationClip, AnimEditor.ChangedCurvesPerClip> dictionary = new Dictionary<AnimationClip, AnimEditor.ChangedCurvesPerClip>();
			AnimEditor.ChangedCurvesPerClip value = default(AnimEditor.ChangedCurvesPerClip);
			for (int i = 0; i < this.m_CurveEditor.animationCurves.Length; i++)
			{
				CurveWrapper curveWrapper = this.m_CurveEditor.animationCurves[i];
				if (curveWrapper.changed)
				{
					if (!curveWrapper.animationIsEditable)
					{
						Debug.LogError("Curve is not editable and shouldn't be saved.");
					}
					if (curveWrapper.animationClip != null)
					{
						if (dictionary.TryGetValue(curveWrapper.animationClip, out value))
						{
							value.bindings.Add(curveWrapper.binding);
							value.curves.Add(curveWrapper.curve);
						}
						else
						{
							value.bindings = new List<EditorCurveBinding>();
							value.curves = new List<AnimationCurve>();
							value.bindings.Add(curveWrapper.binding);
							value.curves.Add(curveWrapper.curve);
							dictionary.Add(curveWrapper.animationClip, value);
						}
					}
					curveWrapper.changed = false;
				}
			}
			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<AnimationClip, AnimEditor.ChangedCurvesPerClip> current in dictionary)
				{
					Undo.RegisterCompleteObjectUndo(current.Key, "Edit Curve");
					AnimationUtility.SetEditorCurves(current.Key, current.Value.bindings.ToArray(), current.Value.curves.ToArray());
				}
				this.m_State.StartRecording();
			}
		}

		private void UpdateSelectedKeysFromCurveEditor()
		{
			this.m_State.ClearKeySelections();
			foreach (CurveSelection current in this.m_CurveEditor.selectedCurves)
			{
				AnimationWindowKeyframe animationWindowKeyframe = AnimationWindowUtility.CurveSelectionToAnimationWindowKeyframe(current, this.m_State.allCurves);
				if (animationWindowKeyframe != null)
				{
					this.m_State.SelectKey(animationWindowKeyframe);
				}
			}
		}

		private void UpdateSelectedKeysToCurveEditor()
		{
			this.UpdateCurveEditorData();
			this.m_CurveEditor.ClearSelection();
			this.m_CurveEditor.BeginRangeSelection();
			foreach (AnimationWindowKeyframe current in this.m_State.selectedKeys)
			{
				CurveSelection curveSelection = AnimationWindowUtility.AnimationWindowKeyframeToCurveSelection(current, this.m_CurveEditor);
				if (curveSelection != null)
				{
					this.m_CurveEditor.AddSelection(curveSelection);
				}
			}
			this.m_CurveEditor.EndRangeSelection();
		}

		private void SaveCurveEditorKeySelection()
		{
			this.UpdateSelectedKeysToCurveEditor();
			this.m_CurveEditor.SaveKeySelection("Edit Curve");
		}

		private void HandleCopyPaste()
		{
			if (Event.current.type == EventType.ValidateCommand || Event.current.type == EventType.ExecuteCommand)
			{
				string commandName = Event.current.commandName;
				if (commandName != null)
				{
					if (!(commandName == "Copy"))
					{
						if (commandName == "Paste")
						{
							if (Event.current.type == EventType.ExecuteCommand)
							{
								this.SaveCurveEditorKeySelection();
								this.m_State.PasteKeys();
								this.UpdateSelectedKeysToCurveEditor();
								GUIUtility.ExitGUI();
							}
							Event.current.Use();
						}
					}
					else
					{
						if (Event.current.type == EventType.ExecuteCommand)
						{
							if (this.m_State.showCurveEditor)
							{
								this.UpdateSelectedKeysFromCurveEditor();
							}
							this.m_State.CopyKeys();
						}
						Event.current.Use();
					}
				}
			}
		}

		internal void UpdateCurveEditorData()
		{
			this.m_CurveEditor.animationCurves = this.m_State.activeCurveWrappers;
		}

		public void Repaint()
		{
			if (this.m_OwnerWindow != null)
			{
				this.m_OwnerWindow.Repaint();
			}
		}

		private void Initialize()
		{
			AnimationWindowStyles.Initialize();
			this.InitializeHierarchy();
			this.m_CurveEditor.state = this.m_State;
			this.m_HorizontalSplitter.realSizes[0] = 300;
			this.m_HorizontalSplitter.realSizes[1] = (int)Mathf.Max(this.m_Position.width - 300f, 300f);
			this.m_DopeSheet.rect = new Rect(0f, 0f, this.contentWidth, 100f);
			this.m_Initialized = true;
		}

		private void InitializeClipSelection()
		{
			this.m_ClipPopup = new AnimationWindowClipPopup();
		}

		private void InitializeHierarchy()
		{
			this.m_Hierarchy = new AnimationWindowHierarchy(this.m_State, this.m_OwnerWindow, new Rect(0f, 0f, this.hierarchyWidth, 100f));
		}

		private void InitializeDopeSheet()
		{
			this.m_DopeSheet = new DopeSheetEditor(this.m_OwnerWindow);
			this.m_DopeSheet.SetTickMarkerRanges();
			this.m_DopeSheet.hSlider = true;
			this.m_DopeSheet.shownArea = new Rect(1f, 1f, 1f, 1f);
			this.m_DopeSheet.rect = new Rect(0f, 0f, this.contentWidth, 100f);
			this.m_DopeSheet.hTicks.SetTickModulosForFrameRate(this.m_State.frameRate);
		}

		private void InitializeEvents()
		{
			this.m_Events = new AnimationEventTimeLine(this.m_OwnerWindow);
		}

		private void InitializeCurveEditor()
		{
			this.m_CurveEditor = new CurveEditor(new Rect(0f, 0f, this.contentWidth, 100f), new CurveWrapper[0], false);
			CurveEditorSettings curveEditorSettings = new CurveEditorSettings();
			curveEditorSettings.hTickStyle.distMin = 30;
			curveEditorSettings.hTickStyle.distFull = 80;
			curveEditorSettings.hTickStyle.distLabel = 0;
			if (EditorGUIUtility.isProSkin)
			{
				curveEditorSettings.vTickStyle.tickColor.color = new Color(1f, 1f, 1f, curveEditorSettings.vTickStyle.tickColor.color.a);
				curveEditorSettings.vTickStyle.labelColor.color = new Color(1f, 1f, 1f, curveEditorSettings.vTickStyle.labelColor.color.a);
			}
			curveEditorSettings.vTickStyle.distMin = 15;
			curveEditorSettings.vTickStyle.distFull = 40;
			curveEditorSettings.vTickStyle.distLabel = 30;
			curveEditorSettings.vTickStyle.stubs = true;
			curveEditorSettings.hRangeMin = 0f;
			curveEditorSettings.hRangeLocked = false;
			curveEditorSettings.vRangeLocked = false;
			curveEditorSettings.hSlider = true;
			curveEditorSettings.vSlider = true;
			curveEditorSettings.allowDeleteLastKeyInCurve = true;
			curveEditorSettings.rectangleToolFlags = CurveEditorSettings.RectangleToolFlags.FullRectangleTool;
			curveEditorSettings.undoRedoSelection = true;
			this.m_CurveEditor.shownArea = new Rect(1f, 1f, 1f, 1f);
			this.m_CurveEditor.settings = curveEditorSettings;
			this.m_CurveEditor.state = this.m_State;
		}

		private void InitializeHorizontalSplitter()
		{
			this.m_HorizontalSplitter = new SplitterState(new float[]
			{
				300f,
				900f
			}, new int[]
			{
				300,
				300
			}, null);
			this.m_HorizontalSplitter.realSizes[0] = 300;
			this.m_HorizontalSplitter.realSizes[1] = 300;
		}

		private void InitializeOverlay()
		{
			this.m_Overlay = new AnimEditorOverlay();
		}

		private void InitializeNonserializedValues()
		{
			AnimationWindowState expr_07 = this.m_State;
			expr_07.onFrameRateChange = (Action<float>)Delegate.Combine(expr_07.onFrameRateChange, new Action<float>(delegate(float newFrameRate)
			{
				this.m_CurveEditor.invSnap = newFrameRate;
				this.m_CurveEditor.hTicks.SetTickModulosForFrameRate(newFrameRate);
			}));
			AnimationWindowState expr_2E = this.m_State;
			expr_2E.onStartLiveEdit = (Action)Delegate.Combine(expr_2E.onStartLiveEdit, new Action(this.OnStartLiveEdit));
			AnimationWindowState expr_55 = this.m_State;
			expr_55.onEndLiveEdit = (Action)Delegate.Combine(expr_55.onEndLiveEdit, new Action(this.OnEndLiveEdit));
			AnimationWindowSelection expr_81 = this.m_State.selection;
			expr_81.onSelectionChanged = (Action)Delegate.Combine(expr_81.onSelectionChanged, new Action(this.OnSelectionChanged));
		}
	}
}
