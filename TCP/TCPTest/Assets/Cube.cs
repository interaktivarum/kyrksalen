using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MsgColor {
    public float r;
    public float g;
    public float b;
}

public class Cube : MonoBehaviour
{

    public TCPMessageHandler ipmh;

    // Start is called before the first frame update
    void Start()
    {
        ipmh.AddCallback("SetColors", SetColors);
        ipmh.AddCallback("RandomizeColors", RandomizeColors);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.2f, 0.1f, 0.3f));
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            JsonMessage msg = new JsonMessage("MouseButtonLeft");
            ipmh.SendMessageToServer(msg, RandomizeColors);
        }
        else if (Input.GetMouseButtonDown(1)) {
            JsonMessage msg = new JsonMessage("MouseButtonRight");
            ipmh.SendMessageToServer(msg);
        }
    }

    public void SetColors(string args) {
        MsgColor argsColor = JsonUtility.FromJson<MsgColor>(args);
        GetComponent<MeshRenderer>().material.color = new Color(argsColor.r, argsColor.g, argsColor.b);
    }

    public void ResetColors(string args) {
        //Debug.Log("Reset colors");
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void RandomizeColors(string args) {
        //Debug.Log("Reset colors");
        GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }

    public void HandleCallback(string args) {
        
    }

}
