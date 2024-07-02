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
    #region Data
    // Data item container
    public DataItemContainer dataItemContainer;
    // Get data item by name
    public static DataItem GetDataItem(string name)
    {
        if (Instance.dataItemContainer.dataItems.ContainsKey(name))
        {
            return Instance.dataItemContainer.dataItems[name];
        }
        Debug.LogError($"Item {name} not found");
        return null;
    }
    #endregion
        // Key and IV for encrypt and decrypt
        private const string KEY = "dv0x4vsAQxffsxOrmBywQZELCS8k8InXeoju8xRK1NA=";
        private const string IV = "OmAyItBPXgbCpZLgB0FmoA==";
        // Path to save game data
        private const string PATH = "/savegame.json";
        private void Awake()
        {
            this.LoadData();
            this.Setup();
            
        }
        // Setup data
        public void Setup()
        {
            GameManager.Instance.Player.LoadData();
            GameManager.Instance.RenUIPlayer.LoadData();
        }
        #region DataGame
        // Data game
        private List<ItemData> inventoryItems;
        private List<ItemData> itemPlayerHold;
 
        
        public List<ItemData> InventoryItems
        {
            get => inventoryItems;
            set
            {
                inventoryItems = value;
                SaveData();
            }
        }
        public List<ItemData> ItemPlayerHold
        {
            get => itemPlayerHold;
            set
            {
                itemPlayerHold = value;
                SaveData();
            }
        }
        #endregion
        // Save data
        public void SaveData()
        {
            //Custom data before saving
            GameData saveData = new GameData();
            saveData.inventoryDatas = this.inventoryItems;
            saveData.itemPlayerHold = this.itemPlayerHold;
            string path = Application.persistentDataPath + PATH;
            if (SaveData(saveData, path))
            {
                
            }
        }
        
        public void LoadData()
        {
            string path = Application.persistentDataPath + PATH;
            //Custom default data
            GameData defaultData = new GameData();
            defaultData.inventoryDatas = new List<ItemData>();
            defaultData.itemPlayerHold = new List<ItemData>();
            
            // Load data
            GameData data = LoadData(path, defaultData);
            
            
            //Custom properties
            this.inventoryItems = data.inventoryDatas;
            this.itemPlayerHold = data.itemPlayerHold;
        }

        #region SaveAndLoadDataMoreDetail

        
        // Save data to file
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
        //Load data
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
// Game data for saving
[Serializable]
public class GameData
{
    public List<ItemData> inventoryDatas;
    public List<ItemData> itemPlayerHold;
}
// Data item for saving
public struct ItemData
{
    public ItemData(string name, int quantity)
    {
        this.name = name;
        this.quantity = quantity;
    }
    public string name;
    public int quantity;
}