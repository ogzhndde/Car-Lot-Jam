using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : SingletonManager<TutorialManager>
{
    public int TutorialIndex = 0;
    [SerializeField] GameObject OBJ_TutorialPlayer;
    [SerializeField] GameObject OBJ_TutorialCar;
    [SerializeField] List<GameObject> OtherElements;

    [SerializeField] List<String> TutorialTexts;
    [SerializeField] TextMeshProUGUI TMP_TutorialText;

    void Start()
    {

        if (PlayerPrefs.GetInt("TutorialDone") == 0)
            SetTutorial();
    }

    public void SetTutorial()
    {
        TMP_TutorialText.gameObject.SetActive(true);

        switch (TutorialIndex)
        {
            case 0:
                ColliderCheck(OBJ_TutorialPlayer, true);
                ColliderCheck(OBJ_TutorialCar, false);
                TMP_TutorialText.text = TutorialTexts[TutorialIndex];
                OtherElementsCheck(false);
                break;

            case 1:
                ColliderCheck(OBJ_TutorialCar, true);
                ColliderCheck(OBJ_TutorialPlayer, false);
                TMP_TutorialText.text = TutorialTexts[TutorialIndex];
                OtherElementsCheck(false);
                break;

            default:
                TMP_TutorialText.gameObject.SetActive(false);
                OtherElementsCheck(true);
                PlayerPrefs.SetInt("TutorialDone", 1);
                break;
        }
    }

    void ColliderCheck(GameObject target, bool state)
    {
        target.GetComponent<Collider>().enabled = state;
    }

    void OtherElementsCheck(bool state)
    {
        for (int i = 0; i < OtherElements.Count; i++)
        {
            ColliderCheck(OtherElements[i], state);
        }
    }

    public void TutorialNext()
    {
        TutorialIndex++;
        SetTutorial();
    }
}
