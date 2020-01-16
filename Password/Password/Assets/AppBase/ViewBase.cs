using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{

    public Views views;

    public virtual void SetReferences() {
        views = GetComponentInParent<Views>();
    }

    public virtual void LoadView() {
        gameObject.SetActive(true);
    }

    public virtual void UnloadView() {
        UnloadView("");
    }

    public virtual void UnloadView(string args) {
        views.NextView();
    }

}
