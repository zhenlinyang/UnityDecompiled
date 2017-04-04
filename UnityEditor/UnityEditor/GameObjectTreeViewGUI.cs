using System;
using System.Linq;
using System.Threading;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
	internal class GameObjectTreeViewGUI : TreeViewGUI
	{
		private enum GameObjectColorType
		{
			Normal,
			Prefab,
			BrokenPrefab,
			Count
		}

		internal static class GameObjectStyles
		{
			public static GUIStyle disabledLabel;

			public static GUIStyle prefabLabel;

			public static GUIStyle disabledPrefabLabel;

			public static GUIStyle brokenPrefabLabel;

			public static GUIStyle disabledBrokenPrefabLabel;

			public static GUIContent loadSceneGUIContent;

			public static GUIContent unloadSceneGUIContent;

			public static GUIContent saveSceneGUIContent;

			public static GUIStyle optionsButtonStyle;

			public static GUIStyle sceneHeaderBg;

			public static readonly int kSceneHeaderIconsInterval;

			static GameObjectStyles()
			{
				GameObjectTreeViewGUI.GameObjectStyles.disabledLabel = new GUIStyle("PR DisabledLabel");
				GameObjectTreeViewGUI.GameObjectStyles.prefabLabel = new GUIStyle("PR PrefabLabel");
				GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel = new GUIStyle("PR DisabledPrefabLabel");
				GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel = new GUIStyle("PR BrokenPrefabLabel");
				GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel = new GUIStyle("PR DisabledBrokenPrefabLabel");
				GameObjectTreeViewGUI.GameObjectStyles.loadSceneGUIContent = new GUIContent(EditorGUIUtility.FindTexture("SceneLoadIn"), "Load scene");
				GameObjectTreeViewGUI.GameObjectStyles.unloadSceneGUIContent = new GUIContent(EditorGUIUtility.FindTexture("SceneLoadOut"), "Unload scene");
				GameObjectTreeViewGUI.GameObjectStyles.saveSceneGUIContent = new GUIContent(EditorGUIUtility.FindTexture("SceneSave"), "Save scene");
				GameObjectTreeViewGUI.GameObjectStyles.optionsButtonStyle = "PaneOptions";
				GameObjectTreeViewGUI.GameObjectStyles.sceneHeaderBg = "ProjectBrowserTopBarBg";
				GameObjectTreeViewGUI.GameObjectStyles.kSceneHeaderIconsInterval = 2;
				GameObjectTreeViewGUI.GameObjectStyles.disabledLabel.alignment = TextAnchor.MiddleLeft;
				GameObjectTreeViewGUI.GameObjectStyles.prefabLabel.alignment = TextAnchor.MiddleLeft;
				GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel.alignment = TextAnchor.MiddleLeft;
				GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel.alignment = TextAnchor.MiddleLeft;
				GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel.alignment = TextAnchor.MiddleLeft;
				GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.disabledLabel);
				GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.prefabLabel);
				GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel);
				GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel);
				GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel);
			}

			private static void ClearSelectionTexture(GUIStyle style)
			{
				Texture2D background = style.hover.background;
				style.onNormal.background = background;
				style.onActive.background = background;
				style.onFocused.background = background;
			}
		}

		private float m_PrevScollPos;

		private float m_PrevTotalHeight;

		public event Action scrollPositionChanged
		{
			add
			{
				Action action = this.scrollPositionChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.scrollPositionChanged, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.scrollPositionChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.scrollPositionChanged, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action scrollHeightChanged
		{
			add
			{
				Action action = this.scrollHeightChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.scrollHeightChanged, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.scrollHeightChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.scrollHeightChanged, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action mouseAndKeyboardInput
		{
			add
			{
				Action action = this.mouseAndKeyboardInput;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.mouseAndKeyboardInput, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.mouseAndKeyboardInput;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.mouseAndKeyboardInput, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		private bool showingStickyHeaders
		{
			get
			{
				return SceneManager.sceneCount > 1;
			}
		}

		public GameObjectTreeViewGUI(TreeViewController treeView, bool useHorizontalScroll) : base(treeView, useHorizontalScroll)
		{
			this.k_TopRowMargin = 0f;
		}

		public override void OnInitialize()
		{
			this.m_PrevScollPos = this.m_TreeView.state.scrollPos.y;
			this.m_PrevTotalHeight = this.m_TreeView.GetTotalRect().height;
		}

		private void DetectScrollChange()
		{
			float y = this.m_TreeView.state.scrollPos.y;
			if (this.scrollPositionChanged != null && !Mathf.Approximately(y, this.m_PrevScollPos))
			{
				this.scrollPositionChanged();
			}
			this.m_PrevScollPos = y;
		}

		private void DetectTotalRectChange()
		{
			float height = this.m_TreeView.GetTotalRect().height;
			if (this.scrollHeightChanged != null && !Mathf.Approximately(height, this.m_PrevTotalHeight))
			{
				this.scrollHeightChanged();
			}
			this.m_PrevTotalHeight = height;
		}

		private void DetectMouseDownInTreeViewRect()
		{
			if (this.mouseAndKeyboardInput != null)
			{
				Event current = Event.current;
				bool flag = current.type == EventType.MouseDown || current.type == EventType.MouseUp;
				bool flag2 = current.type == EventType.KeyDown || current.type == EventType.KeyUp;
				if ((flag && this.m_TreeView.GetTotalRect().Contains(current.mousePosition)) || flag2)
				{
					this.mouseAndKeyboardInput();
				}
			}
		}

		private void DoStickySceneHeaders()
		{
			int num;
			int num2;
			this.GetFirstAndLastRowVisible(out num, out num2);
			if (num >= 0 && num2 >= 0)
			{
				float y = this.m_TreeView.state.scrollPos.y;
				if (num != 0 || y > this.topRowMargin)
				{
					GameObjectTreeViewItem firstItem = (GameObjectTreeViewItem)this.m_TreeView.data.GetItem(num);
					GameObjectTreeViewItem gameObjectTreeViewItem = (GameObjectTreeViewItem)this.m_TreeView.data.GetItem(num + 1);
					bool flag = firstItem.scene != gameObjectTreeViewItem.scene;
					float width = GUIClip.visibleRect.width;
					Rect rowRect = this.GetRowRect(num, width);
					if (!firstItem.isSceneHeader || !Mathf.Approximately(y, rowRect.y))
					{
						if (!flag)
						{
							rowRect.y = y;
						}
						GameObjectTreeViewItem gameObjectTreeViewItem2 = ((GameObjectTreeViewDataSource)this.m_TreeView.data).sceneHeaderItems.FirstOrDefault((GameObjectTreeViewItem p) => p.scene == firstItem.scene);
						if (gameObjectTreeViewItem2 != null)
						{
							bool selected = this.m_TreeView.IsItemDragSelectedOrSelected(gameObjectTreeViewItem2);
							bool focused = this.m_TreeView.HasFocus();
							bool useBoldFont = gameObjectTreeViewItem2.scene == SceneManager.GetActiveScene();
							this.DoItemGUI(rowRect, num, gameObjectTreeViewItem2, selected, focused, useBoldFont);
							if (GUI.Button(new Rect(rowRect.x, rowRect.y, rowRect.height, rowRect.height), GUIContent.none, GUIStyle.none))
							{
								this.m_TreeView.Frame(gameObjectTreeViewItem2.id, true, false);
							}
							this.m_TreeView.HandleUnusedMouseEventsForItem(rowRect, gameObjectTreeViewItem2, num);
							this.HandleStickyHeaderContextClick(rowRect, gameObjectTreeViewItem2);
						}
					}
				}
			}
		}

		private void HandleStickyHeaderContextClick(Rect rect, GameObjectTreeViewItem sceneHeaderItem)
		{
			Event current = Event.current;
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				bool flag = (current.type == EventType.MouseDown && current.button == 1) || current.type == EventType.ContextClick;
				if (flag && rect.Contains(Event.current.mousePosition))
				{
					current.Use();
					this.m_TreeView.contextClickItemCallback(sceneHeaderItem.id);
				}
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				if (current.type == EventType.MouseDown && current.button == 1 && rect.Contains(Event.current.mousePosition))
				{
					current.Use();
				}
			}
		}

		public override void BeginRowGUI()
		{
			this.DetectScrollChange();
			this.DetectTotalRectChange();
			this.DetectMouseDownInTreeViewRect();
			base.BeginRowGUI();
			if (this.showingStickyHeaders && Event.current.type != EventType.Repaint)
			{
				this.DoStickySceneHeaders();
			}
		}

		public override void EndRowGUI()
		{
			base.EndRowGUI();
			if (this.showingStickyHeaders && Event.current.type == EventType.Repaint)
			{
				this.DoStickySceneHeaders();
			}
		}

		public override Rect GetRectForFraming(int row)
		{
			Rect rectForFraming = base.GetRectForFraming(row);
			if (this.showingStickyHeaders && row < this.m_TreeView.data.rowCount)
			{
				GameObjectTreeViewItem gameObjectTreeViewItem = this.m_TreeView.data.GetItem(row) as GameObjectTreeViewItem;
				if (gameObjectTreeViewItem != null && !gameObjectTreeViewItem.isSceneHeader)
				{
					rectForFraming.y -= this.k_LineHeight;
					rectForFraming.height = 2f * this.k_LineHeight;
				}
			}
			return rectForFraming;
		}

		public override bool BeginRename(TreeViewItem item, float delay)
		{
			GameObjectTreeViewItem gameObjectTreeViewItem = item as GameObjectTreeViewItem;
			bool result;
			if (gameObjectTreeViewItem == null)
			{
				result = false;
			}
			else if (gameObjectTreeViewItem.isSceneHeader)
			{
				result = false;
			}
			else
			{
				UnityEngine.Object objectPPTR = gameObjectTreeViewItem.objectPPTR;
				if ((objectPPTR.hideFlags & HideFlags.NotEditable) != HideFlags.None)
				{
					Debug.LogWarning("Unable to rename a GameObject with HideFlags.NotEditable.");
					result = false;
				}
				else
				{
					result = base.BeginRename(item, delay);
				}
			}
			return result;
		}

		protected override void RenameEnded()
		{
			string text = (!string.IsNullOrEmpty(base.GetRenameOverlay().name)) ? base.GetRenameOverlay().name : base.GetRenameOverlay().originalName;
			int userData = base.GetRenameOverlay().userData;
			bool userAcceptedRename = base.GetRenameOverlay().userAcceptedRename;
			if (userAcceptedRename)
			{
				ObjectNames.SetNameSmartWithInstanceID(userData, text);
				TreeViewItem treeViewItem = this.m_TreeView.data.FindItem(userData);
				if (treeViewItem != null)
				{
					treeViewItem.displayName = text;
				}
				EditorApplication.RepaintAnimationWindow();
			}
		}

		protected override void DoItemGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
		{
			GameObjectTreeViewItem gameObjectTreeViewItem = item as GameObjectTreeViewItem;
			if (gameObjectTreeViewItem != null)
			{
				if (gameObjectTreeViewItem.isSceneHeader)
				{
					Color color = GUI.color;
					GUI.color *= new Color(1f, 1f, 1f, 0.9f);
					GUI.Label(rect, GUIContent.none, GameObjectTreeViewGUI.GameObjectStyles.sceneHeaderBg);
					GUI.color = color;
				}
				base.DoItemGUI(rect, row, item, selected, focused, useBoldFont);
				if (gameObjectTreeViewItem.isSceneHeader)
				{
					this.DoAdditionalSceneHeaderGUI(gameObjectTreeViewItem, rect);
				}
				if (SceneHierarchyWindow.s_Debug)
				{
					GUI.Label(new Rect(rect.xMax - 70f, rect.y, 70f, rect.height), string.Concat(new object[]
					{
						"",
						row,
						" (",
						gameObjectTreeViewItem.id,
						")"
					}), EditorStyles.boldLabel);
				}
			}
		}

		protected void DoAdditionalSceneHeaderGUI(GameObjectTreeViewItem goItem, Rect rect)
		{
			Rect position = new Rect(rect.width - 16f - 4f, rect.y + (rect.height - 6f) * 0.5f, 16f, rect.height);
			if (Event.current.type == EventType.Repaint)
			{
				GameObjectTreeViewGUI.GameObjectStyles.optionsButtonStyle.Draw(position, false, false, false, false);
			}
			position.y = rect.y;
			position.height = rect.height;
			position.width = 24f;
			if (EditorGUI.DropdownButton(position, GUIContent.none, FocusType.Passive, GUIStyle.none))
			{
				this.m_TreeView.SelectionClick(goItem, true);
				this.m_TreeView.contextClickItemCallback(goItem.id);
			}
		}

		protected override void OnContentGUI(Rect rect, int row, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
		{
			if (Event.current.type == EventType.Repaint)
			{
				GameObjectTreeViewItem gameObjectTreeViewItem = item as GameObjectTreeViewItem;
				if (gameObjectTreeViewItem != null)
				{
					if (gameObjectTreeViewItem.isSceneHeader)
					{
						if (gameObjectTreeViewItem.scene.isDirty)
						{
							label += "*";
						}
						Scene.LoadingState loadingState = gameObjectTreeViewItem.scene.loadingState;
						if (loadingState != Scene.LoadingState.NotLoaded)
						{
							if (loadingState == Scene.LoadingState.Loading)
							{
								label += " (is loading)";
							}
						}
						else
						{
							label += " (not loaded)";
						}
						bool useBoldFont2 = gameObjectTreeViewItem.scene == SceneManager.GetActiveScene();
						using (new EditorGUI.DisabledScope(!gameObjectTreeViewItem.scene.isLoaded))
						{
							base.OnContentGUI(rect, row, item, label, selected, focused, useBoldFont2, isPinging);
						}
					}
					else
					{
						if (!isPinging)
						{
							rect.xMin += this.GetContentIndent(item) + base.extraSpaceBeforeIconAndLabel;
						}
						int colorCode = gameObjectTreeViewItem.colorCode;
						if (string.IsNullOrEmpty(item.displayName))
						{
							if (gameObjectTreeViewItem.objectPPTR != null)
							{
								gameObjectTreeViewItem.displayName = gameObjectTreeViewItem.objectPPTR.name;
							}
							else
							{
								gameObjectTreeViewItem.displayName = "deleted gameobject";
							}
							label = gameObjectTreeViewItem.displayName;
						}
						GUIStyle gUIStyle = TreeViewGUI.Styles.lineStyle;
						if (!gameObjectTreeViewItem.shouldDisplay)
						{
							gUIStyle = GameObjectTreeViewGUI.GameObjectStyles.disabledLabel;
						}
						else if ((colorCode & 3) == 0)
						{
							gUIStyle = ((colorCode >= 4) ? GameObjectTreeViewGUI.GameObjectStyles.disabledLabel : TreeViewGUI.Styles.lineStyle);
						}
						else if ((colorCode & 3) == 1)
						{
							gUIStyle = ((colorCode >= 4) ? GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel : GameObjectTreeViewGUI.GameObjectStyles.prefabLabel);
						}
						else if ((colorCode & 3) == 2)
						{
							gUIStyle = ((colorCode >= 4) ? GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel : GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel);
						}
						Texture iconForItem = this.GetIconForItem(item);
						gUIStyle.padding.left = 0;
						if (iconForItem != null)
						{
							Rect position = rect;
							position.width = this.k_IconWidth;
							GUI.DrawTexture(position, iconForItem, ScaleMode.ScaleToFit);
							rect.xMin += base.iconTotalPadding + this.k_IconWidth + this.k_SpaceBetweenIconAndText;
						}
						gUIStyle.Draw(rect, label, false, false, selected, focused);
					}
				}
			}
		}
	}
}
