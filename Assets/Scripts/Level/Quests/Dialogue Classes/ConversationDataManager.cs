using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scripts.Level.Quests.Dialogue_Classes
{
#if UNITY_EDITOR
    public class ConversationDataManager
    {
        public ConversationDataManager(Conversation conversation, string assetName, string path, Questor questor)
        {
            _conversation = conversation;
            _assetName = assetName;
            _path = path;
            _questor = questor;
        }

        private Conversation _conversation;
        private string _assetName;
        private string _path;
        private Questor _questor;


        #region Save & Load data Methods
        
        public void SaveButton()
        {
            SaveQuests();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public void SaveWithReplacementButton()
        {
            DeleteQuests();
            SaveQuests();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            // EditorUtility.SetDirty(this);
        }
        
        public void LoadButton()
        {
            string path = _path + _questor.talkerName;

            if (AssetDatabase.Contains(_conversation))
            {
                path += $"/{_assetName}_copy.asset";

                var loadedObj = AssetDatabase.LoadAssetAtPath(path, _conversation.GetType());
                
                CopyFields(loadedObj as Conversation, _conversation);///
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public void DeleteButton()
        {
            DeleteQuests();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void SaveQuests(bool withReplacement = false)
        {
            string path = _path;

            if (!AssetDatabase.IsValidFolder(path + "/" + _questor.talkerName))
                AssetDatabase.CreateFolder(path, _questor.talkerName);

            path = $"{_path}{_questor.talkerName}/";

            string assetName = $"{_assetName}_copy.asset";

            if (File.Exists(path + assetName) && !withReplacement)
                throw new Exception($"Asset {assetName} already exists.");

            var newQuest = ScriptableObject.CreateInstance<Conversation>();

            SaveAsset(_conversation, newQuest, path + assetName);
        }

        private void DeleteQuests()
        {
            string path = _path + _questor.talkerName;

            if (!AssetDatabase.IsValidFolder(path))
                throw new Exception($"Folder {path} is not valid.");

            string assetName = $"{_assetName}_copy.asset";

            DeleteAsset(path + assetName);
        }

        private void DeleteAsset(string path) => AssetDatabase.DeleteAsset(path);

        private void SaveAsset(Conversation oldObj, Conversation newObj, string path)
        {
            CopyFields(oldObj, newObj);

            AssetDatabase.CreateAsset(newObj as UnityEngine.Object, path);
        }

        private void CopyFields(Conversation oldObj, Conversation newObj)
        {
            var oldFields = oldObj.GetType().GetFields();
            var newFields = newObj.GetType().GetFields();

            for (int i = 0; i < oldFields.Length; i++)
                newFields[i].SetValue(newObj, oldFields[i].GetValue(oldObj));
        }

        #endregion
    }
#endif
}