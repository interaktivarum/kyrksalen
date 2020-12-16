using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlesHandler : MonoBehaviour
{

    public int language = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLanguage(int l) {
        language = l;
    }

    public void SwitchLanguage() {
        language = language == 0 ? 1 : 0;
        Debug.Log("Switch language to " + language);
    }

    public int languageOpposite() {
        return language == 0 ? 1 : 0;
    }

}
