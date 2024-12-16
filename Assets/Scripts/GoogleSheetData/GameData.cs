using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Config/GameData")]
public class GameData : ScriptableObject
{
    public string sheetName;
    public string sheetId;
    public string gridId;

    public List<DataConfig> gameData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleOffline.FillData<DataConfig>(sheetName, gridId, list =>
        {
            gameData = list;
            ReadGoogleOffline.SetDirty(this);
        });
    }

    [ContextMenu("OpenSheet")]
    private void Open()
    {
        ReadGoogleOffline.OpenUrl(sheetId, gridId);
    }

}

[Serializable]
public class DataConfig
{
    public string Name;
    public float Value;
}
;