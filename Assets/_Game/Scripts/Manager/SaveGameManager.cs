using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class SaveGameManager : Singleton<SaveGameManager>
{
        public DataItemContainer dataItemContainer;
        private const string KEY = "dv0x4vsAQxffsxOrmBywQZELCS8k8InXeoju8xRK1NA=";
        private const string IV = "OmAyItBPXgbCpZLgB0FmoA==";
        private const string PATH = "/savegame.json";

        #region DataGame

        private int sampleData;
        public int SampleData
        {
            get => sampleData;
            set
            {
                sampleData = value;
                SaveData();
            }
        }
        

        #endregion



        public void SaveData()
        {
            //Custom data before saving
            GameData saveData = new GameData();
            saveData.sampleData = this.sampleData;
            
            string path = Application.persistentDataPath + PATH;
            if (SaveData(saveData, path))
            {
                Debug.Log("Data saved successfully");
            }
        }
        
        public void LoadData()
        {
            string path = Application.persistentDataPath + PATH;
            
            //Custom default data
            GameData defaultData = new GameData();
            defaultData.sampleData = 0;
            
            // Load data
            GameData data = LoadData(path, defaultData);
            
            
            //Custom properties
            this.sampleData = data.sampleData;
        }

        #region SaveAndLoadDataMoreDetail

        

        public bool SaveData(GameData data, string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using FileStream stream = File.Create(path);
                using Aes aes = Aes.Create();
                aes.Key = Convert.FromBase64String(KEY);
                aes.IV = Convert.FromBase64String(IV);
                using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                using CryptoStream cryptoStream = new CryptoStream(
                    stream,
                    cryptoTransform, 
                    CryptoStreamMode.Write);
                cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save game data: {e.Message}");
                return false;
            }
        }

        public GameData LoadData(string path, GameData defaultData = null)
        {
            if (!File.Exists(path))
            {
                Debug.Log("Cann't load data, file not found");
                return defaultData;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(path);
                using Aes aes = Aes.Create();
                
                aes.Key = Convert.FromBase64String(KEY);
                aes.IV = Convert.FromBase64String(IV);
                
                using ICryptoTransform cryptoTransform = aes.CreateDecryptor(
                    aes.Key,
                    aes.IV
                    );
                using MemoryStream memoryStream = new MemoryStream(fileBytes);
                using CryptoStream cryptoStream = new CryptoStream(
                    memoryStream,
                    cryptoTransform,
                    CryptoStreamMode.Read
                    );
                using StreamReader streamReader = new StreamReader(cryptoStream);
                string result =  streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<GameData>(result);
            }
            catch (Exception e)
            {
                Debug.LogError($"Cann't load data due to: {e.Message}");
                throw e;
            }
        }
        #endregion
}
public class GameData
{
    public int sampleData;
} 