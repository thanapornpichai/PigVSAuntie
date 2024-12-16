using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Config/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string sheetName;
    public string sheetId;
    public string gridId;

    public List<EnemyConfig> enemyData;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleOffline.FillData<EnemyConfig>(sheetName, gridId, list =>
        {
            enemyData = list;
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
public class EnemyConfig
{
    public string Name;
    public int HP;
    public int MissedChance;
}
;