using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Config/ItemData")]
public class ItemData : ScriptableObject
{
    public string sheetId;
    public string gridId;

    public List<ItemConfig> itemData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleSheets.FillData<ItemConfig>(sheetId, gridId, list =>
        {
            itemData = list;
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
public class ItemConfig
{
    public string Name;
    public int Amount;
    public int Damage;
    public int HP;
    public int Inventory;
}
;