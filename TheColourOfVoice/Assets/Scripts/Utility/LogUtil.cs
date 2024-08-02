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

public struct PPercentageLog : ICSVLog
{
    public List<float> percentages;
    
    public static string Header()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < 180; i++)
        {
            indices.Add(i);
        }
        return string.Join(",", indices);
    }
    
    public string Content()
    {
        return string.Join(",", percentages);
    }
}

public struct PScoreLog : ICSVLog
{
    public List<float> scores;
    
    public static string Header()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < 180; i++)
        {
            indices.Add(i);
        }
        return string.Join(",", indices);
    }
    
    public string Content()
    {
        return string.Join(",", scores);
    }
}

public class LogUtil : Singleton<LogUtil>
{
    public ScoreBar scoreBar;
    Dictionary<Type, List<ICSVLog>> CSVLogs = new();
    Dictionary<Type, ICSVLog> PLogs = new();

    protected override void Awake()
    {
        base.Awake();
        // Application.logMessageReceived += HandleLog;
        if (scoreBar)
        {
            scoreBar.OnScoreChanged += OnScoreChanged;
            CSVLogs.Add(typeof(ScoreLog), new List<ICSVLog>());
            
            var pPercentageLog = new PPercentageLog();
            pPercentageLog.percentages = new List<float>();
            PLogs.Add(typeof(PPercentageLog), pPercentageLog);
            
            var pScoreLog = new PScoreLog();
            pScoreLog.scores = new List<float>();
            PLogs.Add(typeof(PScoreLog), pScoreLog);
        }
    }
    
    void OnScoreChanged(float newPercentage)
    {
        CSVLogs[typeof(ScoreLog)].Add(new ScoreLog
        {
            percentage = newPercentage * 100,
            score = Mathf.RoundToInt(scoreBar.score)
        });
        
        PPercentageLog pPercentageLog = (PPercentageLog) PLogs[typeof(PPercentageLog)];
        pPercentageLog.percentages.Add(newPercentage);
        PLogs[typeof(PPercentageLog)] = pPercentageLog;
        
        PScoreLog pScoreLog = (PScoreLog) PLogs[typeof(PScoreLog)];
        pScoreLog.scores.Add(scoreBar.score);
        PLogs[typeof(PScoreLog)] = pScoreLog;
    }

    public void LogCSV()
    {
        /*foreach (var logs in CSVLogs)
        {
            string directory = Application.dataPath + $"/Logs/{logs.Key}/";
            string fileName = $"{logs.Key}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            string header = logs.Key.GetMethod(nameof(ICSVLog.Header))?.Invoke(null, null) as string;
            string content = string.Join("\n", logs.Value.ConvertAll(log => log.Content()));
            TryWriteToFile(directory, fileName, $"{header}\n{content}");
        }*/
        foreach (var logs in PLogs)
        {
            string directory = Application.dataPath + $"/Logs/";
            string fileName = $"{logs.Key}.csv";
            string header = logs.Key.GetMethod(nameof(ICSVLog.Header))?.Invoke(null, null) as string;
            string level = PlayerPrefs.GetInt("levelIndex", 1).ToString();
            string content = $"Level {level}," + logs.Value.Content();
            TryAddToFile(directory, fileName, header, content);
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

    void TryAddToFile(string directory, string fileName, string header, string content)
    {
        string filePath = directory + fileName;
        if (!System.IO.File.Exists(filePath))
        {
            System.IO.File.WriteAllText(filePath, $"{header}\n");
        }
        System.IO.File.AppendAllText(filePath, $"{content}\n");
    }
}