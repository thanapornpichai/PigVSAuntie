using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Config/GameData")]
public class GameData : ScriptableObject
{
    public string sheetId;
    public string gridId;

    public List<DataConfig> gameData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleSheets.FillData<DataConfig>(sheetId, gridId, list =>
        {
            gameData = list;
            ReadGoogleSheets.SetDirty(this);
        });
    }

    [ContextMenu("OpenSheet")]
    private void Open()
    {
        ReadGoogleSheets.OpenUrl(sheetId, gridId);
    }

}

[Serializable]
public class DataConfig
{
    public string Name;
    public float Value;
}
;