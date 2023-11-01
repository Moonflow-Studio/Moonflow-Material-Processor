using System;
using System.Collections.Generic;
using Moonflow.MFAssetTools.MFMatProcessor.EnternalConfigs;
#if MFRefLink
using Tools.Editor.MFAssetTools.MFRefLink.Editor;
#endif
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor.Editor
{
    public class MFMatProcessEditor : EditorWindow
    {
        private MFMatProcessor _core;
        private string _status = "";
        private MessageType _statusType = MessageType.None;
        
        private MFMatProcessor.FilterType _newFilterType;
        private MFMatProcessor.DataSetterType _newSetterType;
        private Vector2 _funcScroll;
        private int _flipListIndex = 0;
        private string[] moduleTabs = new[] { "Filter", "Replacer" };
        private int _tabIndex = 0;
        
        private static float _leftWidth = 450;
        
        private MFMatFilterConConfig _conConfig;
        private MFMatSetterConfig _setConfig;
        //create window
        [MenuItem("Moonflow/Tools/Material Transfer #%W")]
        public static void ShowWindow()
        {
#if MFRefLink
            var _ins = GetWindow<MFMatProcessEditor>("Material Transfer");
#else
            var _ins = GetWindow<MFMatProcessEditor>("Material Transfer (No RefLink Version)");
#endif
            _ins.minSize = new Vector2(900, 700);
            _ins.maxSize = new Vector2(900, 700);
        }

        private void OnEnable()
        {
            _core = new MFMatProcessor();
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                #region Left View
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(_leftWidth), GUILayout.Height(1000)))
                {
                    using (new EditorGUILayout.HorizontalScope("box"))
                    {
#if MFRefLink
                        if(GUILayout.Button("Get Cache"))
                        {
                            if (_core.GetCache())
                            {
                                _status = "Get Cache Success";
                                _statusType = MessageType.Info;
                            }
                            else
                            {
                                _status = "Get Cache Failed";
                                _statusType = MessageType.Error;
                            }
                        }
#endif
                        if (GUILayout.Button("Get Materials"))
                        {
                            _core.FilteredMatList();
                        }
                        
                        if(GUILayout.Button("Set Materials"))
                        {
                            bool success = _core.SetMatData(out _status);
                            _statusType = success ? MessageType.Info : MessageType.Error;
                        }
                    }
                    
                    MFEditorUI.DivideLine(Color.white);

                    using (new EditorGUILayout.HorizontalScope("box"))
                    {
                        //draw tab view to switch between filter and replace
                        _tabIndex = GUILayout.Toolbar(_tabIndex, moduleTabs, GUILayout.Width(_leftWidth));
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        switch (_tabIndex)
                        {
                            case 0:
                                DrawConditionView();
                                break;
                            case 1:
                                DrawReplaceView();
                                break;
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(145)))
                    {
                        EditorGUILayout.HelpBox(_status, _statusType,true);
                    }
                }
                #endregion
                
                #region Right View

                using (new EditorGUILayout.VerticalScope(GUILayout.Width(400), GUILayout.Height(700)))
                {
                    //Draw Materials List
                    var shownList = _core.GetMatList(false);
                    EditorGUILayout.LabelField("Materials List", EditorStyles.boldLabel, GUILayout.Height(27));
                    MFEditorUI.DivideLine(Color.white);
                    if (shownList != null)
                    {
                        MFEditorUI.DrawFlipList(DrawMatItem,_core.GetMatList(false), ref _flipListIndex,  20);
                    }
                    if(GUILayout.Button("Copy List"))
                    {
                        string matNameList = _core.CopyMatNameList();
                        EditorGUIUtility.systemCopyBuffer = matNameList;
                    }
                }
                #endregion
            }
        }

        private  void DrawMatItem(Material item, int index)
        {
            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(24), GUILayout.Width(400)))
            {
                float normalWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.ObjectField(index.ToString(), item, typeof(Material), false, GUILayout.Width(350));
                EditorGUIUtility.labelWidth = normalWidth;
#if MFRefLink
                if (GUILayout.Button("Ref", GUILayout.Width(50)))
                {
                    var assetData = _core.GetRefMaterialData(item);
                    MFSingleRefGraph.Open(assetData);
                }
#else
                GUI.color = new Color(1,1,1,0.1f);
                GUILayout.Label("Ref", GUILayout.Width(50));
                GUI.color = Color.white;
#endif
            }
            
        }

        private void DrawReplaceView()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(_leftWidth+5)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    _setConfig = EditorGUILayout.ObjectField(_setConfig, typeof(MFMatSetterConfig)) as MFMatSetterConfig;
                    if (GUILayout.Button("Read Config"))
                    {
                        if (_setConfig != null)
                        {
                            _core.matDataSetterList = new List<MFMatDataSetter>();
                            for (int i = 0; i < _setConfig.setters.Length; i++)
                            {
                                _core.matDataSetterList.Add(_setConfig.setters[i]);
                            }
                        }
                    }

                    if (GUILayout.Button("Save All Config"))
                    {
                        if (_setConfig == null)
                        {
                            MFMatSetterConfig newConfig = ScriptableObject.CreateInstance<MFMatSetterConfig>();
                            newConfig.setters = new MFMatDataSetter[_core.matDataSetterList.Count];
                            newConfig.setters = _core.matDataSetterList.ToArray();
                            string path = EditorUtility.SaveFilePanelInProject("Save Setter Config", "NewMFMatSetterConfig", "asset", "Save the rule config due to replace material properties as a config asset");
                            if (path.Length != 0)
                            {
                                AssetDatabase.CreateAsset(newConfig, path);
                                AssetDatabase.Refresh();
                            }
                        }
                        else
                        {
                            _setConfig.setters = new MFMatDataSetter[_core.matDataSetterList.Count];
                            _core.matDataSetterList.CopyTo(_setConfig.setters);
                            EditorUtility.SetDirty(_setConfig);
                            AssetDatabase.SaveAssetIfDirty(_setConfig);
                        }
                    }
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    //Draw a popup to add new MFMatTransCon
                    EditorGUILayout.LabelField("Add Setter", GUILayout.Width(100));
                    _newSetterType = (MFMatProcessor.DataSetterType)EditorGUILayout.EnumPopup(_newSetterType);
                    if (GUILayout.Button("Add"))
                    {
                        _core.CreateDataSetter(_newSetterType);
                    }
                }

                MFEditorUI.DivideLine(Color.gray);

                using (var s = new EditorGUILayout.ScrollViewScope(_funcScroll, GUILayout.Width(_leftWidth), GUILayout.Height(500)))
                {
                    _funcScroll = s.scrollPosition;
                    //show all MFMatTransCon
                    for (int i = 0; i < _core.matDataSetterList.Count; i++)
                    {
                        using (new EditorGUILayout.HorizontalScope("box"))
                        {
                            _core.matDataSetterList[i].Draw();
                            GUI.color = Color.red;
                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                _core.matDataSetterList.RemoveAt(i);
                            }

                            GUI.color = Color.white;
                        }
                    }
                }
            }
        }

        private void DrawConditionView()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(_leftWidth)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    _conConfig = EditorGUILayout.ObjectField(_conConfig, typeof(MFMatFilterConConfig)) as MFMatFilterConConfig;
                    if (GUILayout.Button("Read Config"))
                    {
                        if (_conConfig != null)
                        {
                            _core.matConList = new List<MFMatFilterCon>();
                            for (int i = 0; i < _conConfig.filters.Length; i++)
                            {
                                _core.matConList.Add(_conConfig.filters[i]);
                            }
                        }
                    }

                    if (GUILayout.Button("Save All Config"))
                    {
                        if (_conConfig == null)
                        {
                            MFMatFilterConConfig newConfig = ScriptableObject.CreateInstance<MFMatFilterConConfig>();
                            newConfig.filters = new MFMatFilterCon[_core.matConList.Count];
                            newConfig.filters = _core.matConList.ToArray();
                            string path = EditorUtility.SaveFilePanelInProject("Save Filter Config", "NewMatFilterCon", "asset", "Save the rule config due to filter material as a config asset");
                            if (path.Length != 0)
                            {
                                AssetDatabase.CreateAsset(newConfig, path);
                                AssetDatabase.Refresh();
                            }
                        }
                        else
                        {
                            _conConfig.filters = new MFMatFilterCon[_core.matConList.Count];
                            _core.matConList.CopyTo(_conConfig.filters);
                            EditorUtility.SetDirty(_conConfig);
                            AssetDatabase.SaveAssetIfDirty(_conConfig);
                        }
                    }
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    //Draw a popup to add new MFMatTransCon
                    // EditorGUILayout.LabelField("新增条件", GUILayout.Width(100));
                    _newFilterType = (MFMatProcessor.FilterType)EditorGUILayout.EnumPopup(_newFilterType);
                    if (GUILayout.Button("Add Filter"))
                    {
                        _core.CreateFilter(_newFilterType);
                    }
                }

                using (var rect = new EditorGUILayout.HorizontalScope())
                {
                    Handles.color = Color.grey;
                    Handles.DrawLine(new Vector2(rect.rect.x, rect.rect.y),
                        new Vector2(rect.rect.x + rect.rect.width, rect.rect.y));
                }

                using (var s = new EditorGUILayout.ScrollViewScope(_funcScroll, GUILayout.Width(_leftWidth), GUILayout.Height(500)))
                {
                    _funcScroll = s.scrollPosition;
                    if (_core.matConList != null)
                    {
                        //show all MFMatTransCon
                        for (int i = 0; i < _core.matConList.Count; i++)
                        {
                            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Width(_leftWidth - 5)))
                            {
                                //delete MFMatTransCon
                                _core.matConList[i].Draw();
                                //show MFMatTransCon
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20)))
                                {
                                    _core.matConList.RemoveAt(i);
                                }

                                GUI.color = Color.white;
                            }
                        }
                    }
                }
            }
        }
    }
}