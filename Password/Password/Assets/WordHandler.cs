using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AnagramList {
    public Anagram[] anagrams;
}

[System.Serializable]
public class Anagram {
    public string[] words;
}

public class WordHandler : MonoBehaviour
{

    public AnagramList anagrams;
    public char[] letters;

    // Start is called before the first frame update
    void Start()
    {
        WordsFromJSON();
        DistinctLetters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WordsFromJSON() {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Application.streamingAssetsPath + "/words.json";

        if (File.Exists(filePath)) {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            anagrams = JsonUtility.FromJson<AnagramList>(dataAsJson);
        }
        else {
            Debug.LogError("Cannot load json data!");
        }
    }

    void DistinctLetters() {
        string appended = "";
        foreach (Anagram anagram in anagrams.anagrams) {
            foreach(string word in anagram.words) {
                appended += word;
            }
        }
        letters = appended.Distinct().ToArray(); 
    }
}
