using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CMCam
{
    CMIzometricView,
    CMTopView,
}

public class CameraManager : SingletonManager<CameraManager>
{
    public CMCam cMCamEnum;
    public GameObject CMIzometricView, CMTopView;
    public List<GameObject> CamList = new List<GameObject>();

    void Start()
    {
        CamList.Add(CMIzometricView);
        CamList.Add(CMTopView);

        //Check current camera by cam enum value
        InvokeRepeating("CamControl", 0f, .2f);
    }

    public void CamControl()
    {
        switch (cMCamEnum)
        {
            case CMCam.CMIzometricView:
                CamUpdate(CMIzometricView);
                break;
            case CMCam.CMTopView:
                CamUpdate(CMTopView);
                break;
        }
    }

    public void CamUpdate(GameObject activeCam)
    {
        for (int i = 0; i < CamList.Count; i++)
        {
            if (CamList[i] != activeCam)
                CamList[i].SetActive(false);

            if (CamList[i] == activeCam)
                CamList[i].SetActive(true);

        }
    }

}
