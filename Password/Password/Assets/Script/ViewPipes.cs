using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewPipes : ViewBase {
    public ViewTeams _viewTeams;
    public LetterPipes _pipes;
    public WordHandler _wordHandler;
    public Image background;
    public Image backgroundCorrect;
    public Image backgroundIncorrect;
    public Light globalLight;
    int _wrongs;

    // Start is called before the first frame update
    void Start() {

    }

    public override void SetReferences() {
        base.SetReferences();
        _viewTeams = FindObjectOfType<ViewTeams>();
        _pipes = GetComponentInChildren<LetterPipes>();
        _wordHandler = FindObjectOfType<WordHandler>();
    }

    public override void LoadView() {
        base.LoadView();
        CreateLetters();
        _wrongs = 0;
        Clear();
    }

    // Update is called once per frame
    void Update() {

    }

    void Clear() {
        foreach (LetterSphere sphere in GetComponentsInChildren<LetterSphere>()) {
            Destroy(sphere.gameObject);
        }
        GetComponentInChildren<LetterInput>().ClearPassword();
    }

    void CreateLetters() {
        int teamId = _viewTeams.teamIdSelected;
        Anagram anagram = _wordHandler.GetAnagram(teamId);
        _pipes.SetAnagram(anagram);
    }

    public void InputFinished(string password) {

        if (PasswordCheck(password)) {
            AnimateSceneColors(Color.green);
            AnimateBackground(backgroundCorrect, 1);
            views.BlockScreensaver();
            views._mh.SendStringToServer("CorrectPassword:1");
            //StartCoroutine(CorrectPassword());
            views.FadeTo(Color.black, 2, 4);
        }
        else {
            GetComponentInChildren<LetterInput>().OpenCap();
            AnimateSceneColors(Color.red);
            AnimateBackground(backgroundIncorrect, 1);
            Help();
            views._mh.SendStringToServer("CorrectPassword:0");
        }
    }

    public bool PasswordCheck(string password) {
        foreach (string word in _pipes.anagram.words) {
            if (word.ToLower() == password.ToLower()) {
                return true;
            }
        }
        return false;
    }

    void Help() {
        _wrongs++;
        if (_wrongs % 2 == 0) {
            Anagram anagram = _pipes.anagram;
            bool removed = _pipes.RemoveExtraPipe();
            if (!removed) {
                _pipes.RearrangeToWord();
            }
        }
    }

    YieldInstruction AnimateSceneColors(Color c) {
        Sequence sequence = DOTween.Sequence();
        //sequence.Append(globalLight.DOIntensity(0, 2));
        sequence.Append(globalLight.DOColor(c, 2));
        //sequence.Append(globalLight.DOIntensity(3, 2));
        return sequence.WaitForCompletion();

        /*Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(background.DOColor(Color.gray, 2));
        sequence2.Append(background.DOColor(c, 2));
        return sequence2.WaitForCompletion();*/
    }

    YieldInstruction AnimateBackground(Image image, float opacity) {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(new Color(1,1,1,opacity), 2));
        return sequence.WaitForCompletion();
    }

    YieldInstruction ResetSceneColors() {
        Color c = new Color(0.3f, 0.4f, 1);
        AnimateBackground(backgroundCorrect, 0);
        AnimateBackground(backgroundIncorrect, 0);
        return globalLight.DOColor(c, 2).WaitForCompletion();
    }

    public void NewTry() {
        ResetSceneColors();
        _pipes.ResetBlocks();
    }

    /*IEnumerator CorrectPassword() {
        yield return AnimateSceneColors(Color.green);
        views.BlockScreensaver();
        //InitUnloadView();
    }*/

    public override YieldInstruction DoUnloadView() {
        ResetSceneColors();
        return null;
    }

}
