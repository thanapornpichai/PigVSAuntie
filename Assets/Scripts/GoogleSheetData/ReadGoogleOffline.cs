using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ReadGoogleOffline
{
    public const char Determiner = ',';

    public static void FillData<T>(string sheetId, string gridId, Action<List<T>> calBack) where T : new()
    {
        string fileName = $"{sheetId}";
        GetTable(fileName, arr =>
        {
            var type = typeof(T);
            List<T> lst = new List<T>();

            for (int i = 1; i < arr.Count; i++)
            {
                if (string.IsNullOrEmpty(arr[i][0]))
                    break;

                var t = new T();
                lst.Add(t);
            }

            var header = arr[0];
            for (int i = 0; i < header.Count; i++)
            {
                if (!string.IsNullOrEmpty(header[i]))
                {
                    var property = type.GetField(header[i].Replace("\r", string.Empty),
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property != null)
                    {
                        for (int j = 0; j < lst.Count; j++)
                        {
                            if (arr[j + 1][i] == null)
                                arr[j + 1][i] = string.Empty;

                            var value = arr[j + 1][i].Replace("\r", string.Empty);
                            var fieldType = property.FieldType;

                            if (!string.IsNullOrEmpty(value))
                            {
                                if (fieldType == typeof(int))
                                {
                                    property.SetValue(lst[j], int.Parse(value));
                                }
                                else if (fieldType == typeof(float))
                                {
                                    property.SetValue(lst[j], float.Parse(value));
                                }
                                else if (fieldType == typeof(string))
                                {
                                    property.SetValue(lst[j], value);
                                }
                                else if (fieldType == typeof(bool))
                                {
                                    property.SetValue(lst[j], bool.Parse(value));
                                }
                                else
                                {
                                    UnityEngine.Debug.LogWarning($"Unsupported field type: {fieldType.Name}");
                                }
                            }
                        }
                    }
                }
            }

            calBack(lst);
        });
    }

    private static void GetTable(string fileName, Action<List<List<string>>> callBack)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            UnityEngine.Debug.LogError($"CSV file '{fileName}' not found in Resources folder.");
            return;
        }

        var data = SplitCsvGrid(csvFile.text);
        int numRows = data.GetLength(1);
        int numCols = data.GetLength(0);

        List<List<string>> listStr = new List<List<string>>();
        for (int y = 0; y < numRows; y++)
        {
            var row = new List<string>();
            for (int x = 0; x < numCols; x++)
            {
                row.Add(data[x, y]);
            }
            listStr.Add(row);
        }

        callBack(listStr);
    }

    private static string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split(new[] { '\n' }, StringSplitOptions.None);
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        string[,] outputGrid = new string[width, lines.Length];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    private static string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
                @"(((?<x>(?=[,]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,]+)),?)",
                System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

    public static void SetDirty(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(obj);
#endif
    }

    public static void OpenUrl(string sheetId, string gridId)
    {
        Application.OpenURL($"https://docs.google.com/spreadsheets/d/{sheetId}/edit#gid={gridId}");
    }

    public static void OpenFileWithExcel(string sheetId)
    {
        string filePath = $"D:/PigVSAuntie/Assets/Resources/{sheetId}.csv";
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            UnityEngine.Debug.LogError($"File not found at path: {filePath}");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });
            UnityEngine.Debug.Log($"Opening file: {filePath}");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"Failed to open file: {filePath}. Error: {e.Message}");
        }
    }


}
