using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Config/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string sheetId;
    public string gridId;

    public List<EnemyConfig> enemyData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleSheets.FillData<EnemyConfig>(sheetId, gridId, list =>
        {
            enemyData = list;
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
public class EnemyConfig
{
    public string Name;
    public int HP;
    public int MissedChance;
}
;