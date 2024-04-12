using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICSVLog
{
    public static string Header()
    {
        return "";
    }

    public string Content();
}

public struct ScoreLog : ICSVLog
{
    public float percentage;
    public int score;
    
    public static string Header()
    {
        return "Percentage,Score";
    }

    public string Content()
    {
        return $"{percentage},{score}";
    }
}

public class LogUtil : Singleton<LogUtil>
{
    public ScoreBar scoreBar;
    Dictionary<Type, List<ICSVLog>> CSVLogs = new();
    
    protected override void Awake()
    {
        base.Awake();
        // Application.logMessageReceived += HandleLog;
        if (scoreBar)
        {
            scoreBar.OnScoreChanged += OnScoreChanged;
            CSVLogs.Add(typeof(ScoreLog), new List<ICSVLog>());
        }
    }
    
    void OnScoreChanged(float newPercentage)
    {
        CSVLogs[typeof(ScoreLog)].Add(new ScoreLog
        {
            percentage = newPercentage * 100,
            score = Mathf.RoundToInt(scoreBar.score)
        });
    }

    public void LogCSV()
    {
        foreach (var logs in CSVLogs)
        {
            string directory = Application.dataPath + $"/Logs/{logs.Key}/";
            string fileName = $"{logs.Key}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            string header = logs.Key.GetMethod(nameof(ICSVLog.Header))?.Invoke(null, null) as string;
            string content = string.Join("\n", logs.Value.ConvertAll(log => log.Content()));
            TryWriteToFile(directory, fileName, $"{header}\n{content}");
        }
    }
    
    void TryWriteToFile(string directory, string fileName, string content, bool overwrite = true)
    {
        string filePath = directory + fileName;
        if (System.IO.File.Exists(filePath) && !overwrite) return;
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }
        
        try {
            System.IO.File.WriteAllText(filePath, content);
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }
}