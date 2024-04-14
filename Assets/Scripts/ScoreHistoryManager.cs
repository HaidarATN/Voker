using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class ScoreHistoryManager : MonoBehaviour
{
    public string filename, fileExt;
    public TextAsset jsonFormat;
    public List<ScoreHistoryData> historyData;

    // Start is called before the first frame update
    void Start()
    {
        ReadHistoryData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadHistoryData()
    {
        //File.ReadAllText()
        print(FilePathGenerator());
        if (File.Exists(FilePathGenerator()))
        {
            print("Exist");
            ReadJSONData();
        }

        else
        {
            //File.Create(FilePathGenerator(),1024,FileOptions.WriteThrough);
            GenerateNewHistoryFile();
            print("Not Exist");
        }
    }

    string FilePathGenerator()
    {
        string path = Path.Combine(Application.persistentDataPath, "LeaderboardData", filename + fileExt);

        return path;
    }

    void ReadJSONData()
    {
        var jsonData = JSON.Parse(File.ReadAllText(FilePathGenerator()));

        for(int i = 0; i < jsonData["leaderboard"].Count; i++)
        {
            print(jsonData["leaderboard"][i]["name"]);
            ScoreHistoryData temp = new ScoreHistoryData();
            temp.name = jsonData["leaderboard"][i]["name"];
            temp.score = jsonData["leaderboard"][i]["score"].AsInt;
            historyData.Add(temp);
        }
    }

    void GenerateNewHistoryFile()
    {
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "LeaderboardData"));
        File.Create(FilePathGenerator()).Dispose();

        string format = jsonFormat.text;

        File.WriteAllText(FilePathGenerator(), format);
        //jsonData["leaderboard"][-1]["score"]
    }

    public void AssignNewHistory(int score)
    {
        //Assign to JSON file
        var jsonData = JSON.Parse(File.ReadAllText(FilePathGenerator()));

        jsonData["leaderboard"][-1]["name"] = PlayerPrefs.GetString("PlayerName");
        jsonData["leaderboard"][jsonData["leaderboard"].Count-1]["score"] = score;

        File.WriteAllText(FilePathGenerator(), jsonData.ToString());

        //Assign to history manager list
        ScoreHistoryData temp = new ScoreHistoryData();
        temp.name = jsonData["leaderboard"][jsonData["leaderboard"].Count - 1]["name"];
        temp.score = jsonData["leaderboard"][jsonData["leaderboard"].Count - 1]["score"].AsInt;
        historyData.Add(temp);
    }
}

[System.Serializable]
public class ScoreHistoryData
{
    public string name;
    public int score;
}
