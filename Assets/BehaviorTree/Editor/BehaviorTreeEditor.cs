using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using BT;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using BT.NodesView;

public class BehaviorTreeEditor : EditorWindow
{
    // caches
    private BehaviorTreeSettings Settings;
    private BehaviorTreeView _treeView;
    private InspectorView _inspectorView;
    // TODO: uncomment
    private ToolBarMenuView _toolBar;
    public BehaviorTree BehaviorTree;
    private static BehaviorTree SelectedBehaviorTree;
    private static BehaviorTreeEditor Window;

    private MiniMap _minimap;
    private Toggle _minimapToggle;
    
    private Toggle _blackboardToggle;
    private CustomBlackboard _blackboard;

    private const string MINIMAP_TOGGLE_NAME = "minimap-toggle";
    private const string BLACKBOARD_TOGGLE_NAME = "blackboard-toggle";
    private const string BEHAVIOR_TREE_EDITOR_UXML_PATH = "Editor/BehaviorTreeEditor.uxml";
    private const string BEHAVIOR_TREE_EDITOR_USS_PATH = "Editor/BehaviorTreeEditor.uss";
    private const string BEHAVIOR_TREE_WINDOW_TITLE = "Behavior Tree Editor";


    /// <summary>
    /// Responsible for opening the BT editor if a BTSO is opened
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (Selection.activeObject is BehaviorTree)
        {
            EditorWindow currentWindow = EditorWindow.focusedWindow;

            if (currentWindow != null && currentWindow.GetType().Name == "ProjectBrowser")
            {
                OpenEditor();
                return true;
            }
        }

        return false;
    }

    [MenuItem("Tanklike/Behavior Tree Editor")]
    public static void OpenEditor()
    {
        if (Window == null)
        {
            //Window = this;
            Window = GetWindow<BehaviorTreeEditor>();
        }

        if (HasOpenInstances<BehaviorTreeEditor>())
        {
            Debug.Log("Exists");
        }
        else
        {
            Debug.Log("Exists not");
            //Window = GetWindow<BehaviorTreeEditor>();//
            Window.titleContent = new GUIContent(BEHAVIOR_TREE_WINDOW_TITLE);
            Window.Show();
        }

        Window.rootVisualElement.Clear();
        Window.GenerateAllVisual();
    }

    public static void OpenBehaviorTree(BehaviorTree behaviorTree)
    {
        if(behaviorTree == null)
        {
            return;
        }

        Selection.activeObject = behaviorTree;
        EditorUtility.FocusProjectWindow();
        OpenEditor();
    }

    public BehaviorTreeSettings GetSettings()
    {
        if(Settings != null)
        {
            return Settings;
        }

        string path = BehaviorTreeSettings.PATH;
        Settings = AssetDatabase.LoadAssetAtPath<BehaviorTreeSettings>(path);

        if(Settings == null)
        {
            Debug.LogError($"Couldn't find the settings in {path}");
            return null;
        }

        return Settings;
    }

    #region Graph and elements creation
    private void GenerateAllVisual()
    {
        SetBehaviorTree();

        if(BehaviorTree == null)
        {
            return;
        }

        CreateBehaviorTreeFromUXML();
        AddStyleSheets();
        CacheInspectorView();
        CacheToolbarMenu();

        _treeView.OnNodeSelected = OnNodeSelectionChanged;

        OnSelectionChange();

        GenerateMinimap();
        GenerateBlackBoard();
        LoadViewData();

        _treeView.PopulateView(BehaviorTree);
    }


    private void SetBehaviorTree()
    {
        if (Selection.activeObject is BehaviorTree && !Application.isPlaying)
        {
            SelectedBehaviorTree = Selection.activeObject as BehaviorTree;
        }

        BehaviorTree behaviorTreeToSet;

        if (SelectedBehaviorTree == null)
        {
            return;
        }
        else if (!Application.isPlaying)
        {
            GetSettings().LastSelectedBehaviorTree = SelectedBehaviorTree;
            behaviorTreeToSet = GetSettings().LastSelectedBehaviorTree;
        }
        else
        {
            // if the game is running, we just cache the selected bt (which will be the object in run time) and avoid setting it as the last selected one in the settings
            behaviorTreeToSet = SelectedBehaviorTree;
        }

        if (GetSettings().LastSelectedBehaviorTree == null)
        {
            Debug.LogError("No behavior Tree passed");
            return;
        }

        if (BehaviorTree == null || BehaviorTree != behaviorTreeToSet)
        {
            BehaviorTree = behaviorTreeToSet;
            // TODO: temporary
            BehaviorTree.Refresh();
            BehaviorTree.CreateBlackboardContainer();
        }
    }

    private void CreateBehaviorTreeFromUXML()
    {
        string path = BehaviorTreeSettings.CORE_DIRECTORY + BEHAVIOR_TREE_EDITOR_UXML_PATH;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        visualTree.CloneTree(rootVisualElement);
        _treeView = rootVisualElement.Q<BehaviorTreeView>();

        _treeView.Initialize(this);
    }

    private void AddStyleSheets()
    {
        // the stylesheet can be added to a VisualElement.
        // this style will be applied to the VisualElement and all of its children.
        string path = BehaviorTreeSettings.CORE_DIRECTORY + BEHAVIOR_TREE_EDITOR_USS_PATH;
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
        rootVisualElement.styleSheets.Add(styleSheet);
    }

    private void CacheInspectorView()
    {
        _inspectorView = rootVisualElement.Q<InspectorView>();
    }

    private void CacheToolbarMenu()
    {
        //_toolBar = rootVisualElement.Q<ToolBarMenuView>();
        //_toolBar.Initialize(this);
    }

    #region Blackboard
    private void GenerateBlackBoard()
    {
        if (Application.isPlaying)
        {
            return;
        }

        if (_blackboard != null)
        {
            _blackboard.RemoveFromHierarchy();
            _blackboard = null;
        }

        _blackboard = new CustomBlackboard(_treeView, this);
        _blackboard.visible = IsBlackboardDisplayed();

        _treeView.AddBlackboard(_blackboard);

        _blackboardToggle = rootVisualElement.Q<Toggle>(BLACKBOARD_TOGGLE_NAME);

        _blackboardToggle.RegisterValueChangedCallback(evt =>
        {
            _blackboard.visible = evt.newValue;
            BehaviorTree.IsBlackboardDisplayed = evt.newValue;
        });

        LoadBlackboardVisibility();
    }

    private bool IsBlackboardDisplayed()
    {
        if(BehaviorTree != null)
        {
            return BehaviorTree.IsBlackboardDisplayed;
        }

        return false;
    }
    #endregion

    #region Minimap
    private void GenerateMinimap()
    {
        _minimap = new MiniMap() { anchored = false };
        _minimap.visible = IsMinimapDisplayed();

        _minimap.style.position = Position.Absolute;
        _minimap.style.top = 0;
        _minimap.style.right = 0;
        _minimap.style.width = 200;
        _minimap.style.height = 150;

        _treeView.Add(_minimap);

        // set up the minimap toggle
        _minimapToggle = rootVisualElement.Q<Toggle>(MINIMAP_TOGGLE_NAME);
        _minimapToggle.RegisterValueChangedCallback(evt =>
        {
            _minimap.visible = evt.newValue;
            BehaviorTree.IsMinimapDisplayed = evt.newValue;
        });

        LoadMinimapVisibility();
    }

    private bool IsMinimapDisplayed()
    {
        if (BehaviorTree != null)
        {
            //return BehaviorTree.SaveData.IsMinimapDisplayed;
            return BehaviorTree.IsMinimapDisplayed;
        }

        return false;
    }
    #endregion
    #endregion

    #region Editor and play mode controls
    /// <summary>
    /// Allows us to control what happens when the editor and the play mode run and stop in the editor.
    /// </summary>
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayerModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;

        if (Window == null)
        {
            Window = this;
        }

        // reinitialize the window after recompilation
        if (GetSettings().LastSelectedBehaviorTree != null)
        {
            var existingWindows = Resources.FindObjectsOfTypeAll<BehaviorTreeEditor>();
            OpenEditor();
        }
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayerModeStateChanged;
    }

    private void OnPlayerModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                CacheLastBehaviorTree();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                OnPlayModeExit();
                break;
        }
    }

    private static BehaviorTree _lastBehaviorTree;

    private void CacheLastBehaviorTree()
    {
        if(BehaviorTree != null)
        {
            _lastBehaviorTree = BehaviorTree;
        }
    }

    private void OnPlayModeExit()
    {
        Window.rootVisualElement.Clear();

        if (_lastBehaviorTree != null)
        {
            SelectedBehaviorTree = _lastBehaviorTree;
            OpenEditor();
        }
    }
    #endregion

    /// <summary>
    /// Whenever an asset in the project files is selected
    /// </summary>
    private void OnSelectionChange()
    {
        // if there is a selected gameObject, and that selection has a behavior tree runner, then do the checks
        if (Application.isPlaying && Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.TryGetComponent(out BehaviorTreeRunner runner))
            {
                if(BehaviorTree == SelectedBehaviorTree && _treeView != null)
                {
                    return;
                }

                if(runner.Tree == null)
                {
                    return;
                }

                SelectedBehaviorTree = runner.Tree;

                OpenEditor();
            }
        }
    }

    private void OnNodeSelectionChanged(NodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }

    /// <summary>
    /// Gets called around 10 times a second.
    /// </summary>
    private void OnInspectorUpdate()
    {
        _treeView?.UpdateNodeStates();
        SaveViewData();
    }

    #region Save Data
    public void SaveData()
    {
        SavePopUpWindowsView();
        SaveViewData();
    }

    private void SavePopUpWindowsView()
    {
        BehaviorTree.IsBlackboardDisplayed = _blackboard.visible;
        BehaviorTree.IsMinimapDisplayed = _minimap.visible;
    }

    private void SaveViewData()
    {
        if(BehaviorTree == null || _treeView == null)
        {
            return;
        }

        BehaviorTree.ViewPosition = _treeView.viewTransform.position;
        BehaviorTree.ViewZoom = _treeView.viewTransform.scale;
    }
    #endregion

    #region Load Data
    public void LoadData()
    {
        LoadViewData();
    }

    private void LoadMinimapVisibility()
    {
        if (_minimap == null || BehaviorTree == null || _minimapToggle == null)
        {
            return;
        }

        _minimap.visible = BehaviorTree.IsMinimapDisplayed;
        _minimapToggle.value = _minimap.visible;
    }

    private void LoadBlackboardVisibility()
    {
        if (_blackboard == null || BehaviorTree == null || _blackboardToggle == null)
        {
            return;
        }

        _blackboard.visible = BehaviorTree.IsBlackboardDisplayed;
        _blackboardToggle.value = _blackboard.visible;
    }

    private void LoadViewData()
    {
        if (BehaviorTree == null || _treeView == null)
        {
            return;
        }

        _treeView.viewTransform.position = BehaviorTree.ViewPosition;
        _treeView.viewTransform.scale = BehaviorTree.ViewZoom;
    }
    #endregion
}