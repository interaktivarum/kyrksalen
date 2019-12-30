using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{

    public virtual void UnloadView() {
        UnloadView("");
    }

    public virtual void UnloadView(string args) {
        gameObject.GetComponentInParent<Views>().NextView();
    }

}
