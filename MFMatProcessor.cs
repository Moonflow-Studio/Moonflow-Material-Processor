using System.Collections.Generic;
#if MFRefLink
using Moonflow.MFAssetTools.MFRefLink;
#endif
using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatProcessor
    {
        public List<MFMatFilterCon> matConList;

        public List<MFMatDataSetter> matDataSetterList;
    #if MFRefLink
        private List<MFRefMaterialData> _matList;
        private HashSet<MFRefLinkData> _matCache;
        public delegate void MaterialFilter(ref List<MFRefMaterialData> matList, MFMatFilterCon condition);
    #else
        private List<Material> _matList;
        public delegate void MaterialFilter(ref List<Material> matList, MFMatFilterCon condition);
    #endif
        
        private List<Material> _matResults;
        

        public MaterialFilter GetMaterialByFilter;

        public enum FilterType
        {
            Shader,
            Vector,
            // Color,
            Float,
            Int,
            Texture,
            Keyword,
            RenderQueue,
            Reference,
            Name,
            Path
        }

        public enum DataSetterType
        {
            SetShader,
            SetVector,
            SetColor,
            SetFloat,
            SetInt,
            SetTexture,
            SetKeyword,
            SetRenderQueue,
            CleanCache
        }

        public MFMatProcessor()
        {
#if MFRefLink
            _matList = new List<MFRefMaterialData>();
#else
            _matList = new List<Material>();
#endif
            matConList = new List<MFMatFilterCon>();
            matDataSetterList = new List<MFMatDataSetter>();
        }
#if MFRefLink
        public bool GetCache()
        {
            _matCache = MFRefLinkCore.GetFilterCache("Material");
            if (_matCache == null)
                return false;
            return true;
        }
#endif
        
        private void GetMatAssetList()
        {
#if MFRefLink
            if(_matList == null)
                _matList = new List<MFRefMaterialData>();
            _matList.Clear();
            if (GetCache())
            {
                foreach (var mat in _matCache)
                {
                    _matList.Add(mat as MFRefMaterialData);
                }
            }
#else
            if(_matList == null)
                _matList = new List<Material>();
            _matList.Clear();
            var assets = AssetDatabase.FindAssets("t:Material", new[] {"Assets", "Packages"});
            foreach (var asset in assets)
            {
                var mat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(asset));
                _matList.Add(mat);
            }
#endif
        }

        public List<Material> GetMatList(bool passFilter)
        {
            if (passFilter)
            {
                if(_matList == null || _matList.Count == 0)
                    GetMatAssetList();
                return GetMatList(_matList);
            }
            else
            {
                return _matResults;
            }
        }
#if MFRefLink
        public List<Material> GetMatList(List<MFRefMaterialData> matList)
#else
        public List<Material> GetMatList(List<Material> matList)
#endif
        {
            if (_matResults == null)
            {
                _matResults = new List<Material>();
            }
            else
            {
                _matResults.Clear();
            }
            
            foreach (var matAsset in matList)
            {
                //load material by asset path from matAsset
#if MFRefLink
                _matResults.Add(AssetDatabase.LoadAssetAtPath<Material>(matAsset.path));
#else
                _matResults.Add(matAsset);
#endif
            }
            return _matResults;
        }

        public void FilteredMatList()
        {
#if MFRefLink
            MFRefLinkCore.ShaderMaterialLink();
#endif
            //split MatConList to or and and
            Prework(out MFMatFilterCon[] orCon, out MFMatFilterCon[] andCon);
            //get material asset loading from _matList by guid, add to _matResult
            GetMatList(true);
            if(andCon.Length> 0)
                AndFilter(andCon);
            if (orCon.Length > 0)
                OrFilter(orCon);
            
        }
        
        public bool SetMatData(out string tips)
        {
            if (!CheckAllLegal())
            {
                tips = "Some function is illegal";
                return false;
            }

            if (_matResults == null || _matResults.Count == 0)
            {
                tips = "Material List is empty";
                return false;
            }
            
            if (matDataSetterList == null || matDataSetterList.Count == 0)
            {
                tips = "Function List is empty";
                return false;
            }
            
            foreach (var mat in _matResults)
            {
                foreach (var setter in matDataSetterList)
                {
                    setter.SetData(mat);
                }
            }
            tips = "Successful";
            return true;
        }
        
        public bool CheckAllLegal()
        {
            foreach (var setter in matDataSetterList)
            {
                if (!setter.isLegal())
                {
                    return false;
                }
            }
            return true;
        }

        private void AndFilter(MFMatFilterCon[] andCon)
        {
            //for each material in _matList, use andCon to filter
            for (int i = 0; i < _matResults.Count; i++)
            {
                if (andCon != null)
                {
                    for (int j = 0; j < andCon.Length; j++)
                    {
                        if (!andCon[j].Check(_matResults[i]))
                        {
                            _matResults.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            }
        }

        private void OrFilter(MFMatFilterCon[] orCon)
        {
            //transfer _matResult to Hashset<Material>
            HashSet<Material> matSet = new HashSet<Material>();
            //for each MFMatFilterCon in orCon, use it to filter _matResults, add to matSet if returns true
            for (int i = 0; i < orCon.Length; i++)
            {
                for (int j = 0; j < _matResults.Count; j++)
                {
                    if (orCon[i].Check(_matResults[j]))
                    {
                        matSet.Add(_matResults[j]);
                    }
                }
            }
            //transfer matSet to _matResult
            _matResults = new List<Material>(matSet);
        }

        public void CreateFilter(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Shader:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatShaderCon>());
                    break;
                }
                case FilterType.Float:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatFloatCon>());
                    break;
                }
                case FilterType.Vector:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatVectorCon>());
                    break;
                }
                case FilterType.Texture:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatTextureCon>());
                    break;
                }
                case FilterType.Int:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatIntCon>());
                    break;
                }
                case FilterType.Keyword:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatKeywordCon>());
                    break;
                }
                case FilterType.RenderQueue:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatQueueCon>());
                    break;
                }
                case FilterType.Reference:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatRefCon>());
                    break;
                }
                case FilterType.Name:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFMatNameCon>());
                    break;
                }
                case FilterType.Path:
                {
                    matConList.Add(ScriptableObject.CreateInstance<MFPathCon>());
                    break;
                }
            }
        }
        
        public void CreateDataSetter(DataSetterType setterType)
        {
            switch (setterType)
            {
                case DataSetterType.SetFloat:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatFloatSetter>());
                    break;
                }
                case DataSetterType.SetVector:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatVectorSetter>());
                    break;
                }
                case DataSetterType.SetColor:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatVectorSetter>());
                    break;
                }
                case DataSetterType.SetTexture:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatTextureSetter>());
                    break;
                }
                case DataSetterType.SetInt:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatIntSetter>());
                    break;
                }
                case DataSetterType.SetKeyword:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatKeywordSetter>());
                    break;
                }
                case DataSetterType.SetRenderQueue:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatIntSetter>());
                    break;
                }
                case DataSetterType.SetShader:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatShaderSetter>());
                    break;
                }
                case DataSetterType.CleanCache:
                {
                    matDataSetterList.Add(ScriptableObject.CreateInstance<MFMatCacheCleanSetter>());
                    break;
                }
            }
        }

        private void Prework(out MFMatFilterCon[] orCon, out MFMatFilterCon[] andCon)
        {
            List<MFMatFilterCon> orList = new List<MFMatFilterCon>();
            List<MFMatFilterCon> andList = new List<MFMatFilterCon>();
            for (int i = 0; i < matConList.Count; i++)
            {
                if (matConList[i].or)
                {
                    orList.Add(matConList[i]);
                }
                else
                {
                    andList.Add(matConList[i]);
                }
            }

            orCon = orList.ToArray();
            andCon = andList.ToArray();
        }
#if MFRefLink
        public MFRefMaterialData GetRefMaterialData(Material mat)
        {
            //get guid of mat
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(mat));
            foreach (var cache in _matCache)
            {
                if (cache.guid == guid) return cache as MFRefMaterialData;
            }
            return null;
        }
#endif
        
        public string CopyMatNameList()
        {
            string nameList = "";
            if(_matResults == null || _matResults.Count == 0)
                return nameList;
            foreach (var mat in _matResults)
            {
                nameList += mat.name + "\n";
            }
            return nameList;
        }

        public void ForceMatListSetter(List<Material> materials)
        {
            _matResults = materials;
        }
    }
}