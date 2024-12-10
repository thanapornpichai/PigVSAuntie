using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Config/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string sheetId;
    public string gridId;

    public List<PlayerConfig> playerData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleSheets.FillData<PlayerConfig>(sheetId, gridId, list =>
        {
            playerData = list;
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
public class PlayerConfig
{
    public string Name;
    public int HP;
    public int MinPower;
    public int MaxPower;
    public int NormalAttack;
    public int SmallAttack;
}
;