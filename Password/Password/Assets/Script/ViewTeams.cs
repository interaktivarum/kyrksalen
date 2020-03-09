using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewTeams : ViewBase
{

    public int teamIdSelected;

    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(GetComponentInChildren<Image>().DOColor(new Color(0.6f, 0.8f, 1), 10));
        seq.Append(GetComponentInChildren<Image>().DOColor(Color.white, 10));
        seq.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTeamClick(Team team) {
        int id = 0;
        foreach (Team t in GetComponentsInChildren<Team>()) {
            if (t == team) {
                t.Select();
                teamIdSelected = id;
                views._mh.SendStringToServer("TeamSelected:" + id);
            }
            else {
                t.NotSelected();
            }
            id++;
        }
        InitUnloadView();
    }

    public override YieldInstruction DoUnloadView() {
        return new WaitForSeconds(3);
    }
}
