using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Config/ItemData")]
public class ItemData : ScriptableObject
{
    public string sheetName;
    public string sheetId;
    public string gridId;

    public List<ItemConfig> itemData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleOffline.FillData<ItemConfig>(sheetName, gridId, list =>
        {
            itemData = list;
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
public class ItemConfig
{
    public string Name;
    public int Amount;
    public int Damage;
    public int HP;
    public int Inventory;
}
;