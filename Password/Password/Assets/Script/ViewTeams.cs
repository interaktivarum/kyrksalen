using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTeams : ViewBase
{

    public int teamIdSelected;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTeamClick(Team team) {
        int id = 0;
        foreach (Team t in GetComponentsInChildren<Team>()) {
            if (t == team) {
                teamIdSelected = id;
                views._mh.SendStringToServer("TeamSelected:" + id);
                break;
            }
            id++;
        }
        UnloadView();
    }
}
