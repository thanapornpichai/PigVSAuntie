using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Config/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string sheetName;
    public string sheetId;
    public string gridId;

    public List<PlayerConfig> playerData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleOffline.FillData<PlayerConfig>(sheetName, gridId, list =>
        {
            playerData = list;
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