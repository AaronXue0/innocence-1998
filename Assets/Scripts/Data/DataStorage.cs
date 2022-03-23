namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public static class DataStorage
    {
        public static GameData gameData = new GameData();

        #region Data
        public static void SaveData()
        {
            SaveProgressStates();
            SaveObjectStates();
        }

        public static void LoadData()
        {
            LoadProgressStates();
            LoadObjectStates();
        }

        public static void ResetData()
        {
            gameData = new GameData();
            SaveData();
        }
        #endregion

        #region GameChapter
        public static void SaveGameChapter()
        {
            PlayerPrefs.SetInt("gameChapter", gameData.chapter);
        }

        public static void LoadGameChapter()
        {
            gameData.chapter = PlayerPrefs.GetInt("gameChapter", gameData.chapter);
        }

        public static void SetGameChapter(int chapter)
        {
            gameData.chapter = chapter;
            SaveGameChapter();
        }

        public static int GetGameChapter()
        {
            return gameData.chapter;
        }
        #endregion

        #region GameProgress
        public static void SaveProgressStates()
        {
            string data = string.Join(",", gameData.progressStates);
            PlayerPrefs.SetString("progressStates", data);
        }

        public static void LoadProgressStates()
        {
            string data = PlayerPrefs.GetString("progressStates", "");

            if (data.Length != 0)
            {
                gameData.progressStates.Clear();
                foreach (string progressState in data.Split(','))
                    gameData.progressStates.Add(int.Parse(progressState));
            }
        }

        public static void SetProgressState(int progressID, int state)
        {
            if (gameData.progressStates.Count <= progressID)
            {
                int addStateAmount = progressID - gameData.progressStates.Count + 1;
                for (int i = 0; i < addStateAmount; i++)
                    gameData.progressStates.Add(0);
            }
            gameData.progressStates[progressID] = state;
            SaveProgressStates();
        }

        public static int GetProgressState(int progressID)
        {
            if (gameData.progressStates.Count <= progressID)
            {
                int addStateAmount = progressID - gameData.progressStates.Count + 1;
                for (int i = 0; i < addStateAmount; i++)
                    gameData.progressStates.Add(0);
                SaveProgressStates();
            }
            return gameData.progressStates[progressID];
        }
        #endregion

        #region ObjectState
        public static void SaveObjectStates()
        {
            string data = string.Join(",", gameData.objectStates);
            PlayerPrefs.SetString("objectStates", data);
        }

        public static void LoadObjectStates()
        {
            string data = PlayerPrefs.GetString("objectStates", "");

            if (data.Length != 0)
            {
                gameData.objectStates.Clear();
                foreach (string objectState in data.Split(','))
                    gameData.objectStates.Add(int.Parse(objectState));
            }
        }

        public static void SetObjectState(int objectID, int state)
        {
            if (gameData.objectStates.Count <= objectID)
            {
                int addStateAmount = objectID - gameData.objectStates.Count + 1;
                for (int i = 0; i < addStateAmount; i++)
                    gameData.objectStates.Add(0);
            }
            gameData.objectStates[objectID] = state;
            SaveObjectStates();
        }

        public static int GetObjectState(int objectID)
        {
            if (gameData.objectStates.Count <= objectID)
            {
                int addStateAmount = objectID - gameData.objectStates.Count + 1;
                for (int i = 0; i < addStateAmount; i++)
                    gameData.objectStates.Add(0);
                SaveObjectStates();
            }
            return gameData.objectStates[objectID];
        }
        #endregion
    }
}
