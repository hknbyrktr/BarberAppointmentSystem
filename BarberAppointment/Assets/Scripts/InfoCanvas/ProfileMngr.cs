using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using System;
using TMPro;



[System.Serializable]
public class Information
{
    public string name;
    public string lastName;
    public string userName;
    public string password;
    public string phoneNum;
}
[System.Serializable]
public class InfoList
{
    public Information[] info;
}



public class ProfileMngr : MonoBehaviour
{

    WarningPanelMngr warningPanelMngr;
    InfoPanelMngr infoPanelMngr;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lastNameText;
    [SerializeField] TextMeshProUGUI userNameText;
    [SerializeField] TextMeshProUGUI passwordText;
    [SerializeField] TextMeshProUGUI phoneNumText;



    void Start()
    {
        warningPanelMngr = AppManager.Instance.Panels[6].GetComponent<WarningPanelMngr>();
        infoPanelMngr = AppManager.Instance.InfoPanel[0].GetComponent<InfoPanelMngr>();

    }



    public void MyAppointmentBtn()
    {
        StartCoroutine(infoPanelMngr.LeftShift());


    }


    public IEnumerator SetProfilePanel()
    {

        string userName = PlayerPrefs.GetString("userName");
        UnityWebRequest request = UnityWebRequest.Get("https://target_Ip/api/get_customer_info.php?userName=" + userName);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string DBText = request.downloadHandler?.text;   

            try
            {
                InfoList infoList = JsonUtility.FromJson<InfoList>(DBText);

                if (infoList.info.Length == 1)
                {
                    nameText.text = infoList.info[0].name;
                    lastNameText.text = infoList.info[0].lastName;
                    userNameText.text = infoList.info[0].userName;
                    passwordText.text = infoList.info[0].password;
                    phoneNumText.text = infoList.info[0].phoneNum;
                }
                else
                    Debug.LogError("Birden fazla kullanici var. Veritabaninda birseyler ters gitmis olmali !");

            }
            catch (Exception e)
            {
                Debug.Log(e);

                warningPanelMngr.closeButton.SetActive(false);
                warningPanelMngr.OpenWarninPanel("~Hata~", e.ToString(), 2);
            }
        }
        else
        {
            Debug.LogError("Hata: " + request.error);

            warningPanelMngr.closeButton.SetActive(false);
            warningPanelMngr.OpenWarninPanel("~Hata~", request.error + "\nAna menuye dönmek istermisiniz ?", 2);
        }

    }



    public void LogOut()
    {
        string title = "~Oturumu Kapat~";
        string warningText = "Oturumu kapatmak istediğinize emin misiniz?";
        warningPanelMngr.OpenWarninPanel(title, warningText, 1, 38);
    }


}
