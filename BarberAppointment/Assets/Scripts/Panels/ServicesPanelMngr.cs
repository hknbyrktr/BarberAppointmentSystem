using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;



public class ServicesPanelMngr : MonoBehaviour
{

    WarningPanelMngr warningPanelMngr;

    List<int> services = new();

    [SerializeField] GameObject warningText;


    private void Start()
    {
        warningPanelMngr = AppManager.Instance.Panels[6].GetComponent<WarningPanelMngr>();

        ToggleSwitch();
    }


    public void ConfirmAppointment_Btn()
    {
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            if (this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Toggle>().isOn)
                services.Add(i + 1);                                                                                                  // 0. cocugun ID' si 1...
        }

        if (services.Count > 0)
        {
            warningPanelMngr.closeButton.SetActive(false);
            warningPanelMngr.OpenWarninPanel("~Onaylandı~", "Ana menüye dönmek istermisiniz ?", 2);

            services.Clear();                                                                                                          // Eski haline geri getiriyoruz.
            for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
            {
                this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Toggle>().isOn = false;
            }
            StartCoroutine(ConfirmAppointment());
        }
        else
            StartCoroutine(ShakePanel());

    }


    IEnumerator ConfirmAppointment()
    {
        string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string serviceList = string.Join(",", services);

        WWWForm form = new();
        form.AddField("barberID", AppManager.Instance.barberID);
        form.AddField("userName", PlayerPrefs.GetString("userName"));
        form.AddField("appointmentDate", AppManager.Instance.appointmentDate);
        form.AddField("appointmentTime", AppManager.Instance.appointmentTime);
        form.AddField("registrationDate", nowTime);
        form.AddField("services", serviceList);


        UnityWebRequest request = UnityWebRequest.Post("https://target_Ip/api/update_appointment.php", form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {

            string response = request.downloadHandler?.text;

            if (response == "Success")
            {

                warningPanelMngr.closeButton.SetActive(false);
                warningPanelMngr.OpenWarninPanel("~Onaylandı~", "Ana menüye dönmek istermisiniz ?", 2);

                services.Clear();                                                                                                          // Eski haline geri getiriyoruz.
                for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
                {
                    this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Toggle>().isOn = false;
                }

            }
            else
            {
                Debug.Log("Hata: " + response);

                warningPanelMngr.closeButton.SetActive(false);
                warningPanelMngr.OpenWarninPanel("~Hata~", response + "\nAna menuye dönmek istermisiniz ?", 2);
            }
        }
        else
        {
            Debug.LogError("Hata: " + request.error);

            warningPanelMngr.closeButton.SetActive(false);
            warningPanelMngr.OpenWarninPanel("~Hata~", request.error + "\nAna menuye dönmek istermisiniz ?", 2);
        }

    }


    IEnumerator ShakePanel()
    {

        warningText.SetActive(true);

        RectTransform mainPanel = FindObjectOfType<AppManager>().GetComponent<RectTransform>();

        for (int i = 0; i < 3; i++)
        {
            mainPanel.DOAnchorPosX(50, .5f);
            yield return new WaitForSeconds(.1f);
            mainPanel.DOAnchorPosX(-50, .3f);
            yield return new WaitForSeconds(.1f);
            mainPanel.DOAnchorPosX(0, .3f);
        }

    }


    void ToggleSwitch()
    {
        Transform services = this.transform.GetChild(0);

        for (int i = 0; i < services.childCount; i++)
        {
            int index = i;

            services.GetChild(index).GetComponent<Button>().onClick.AddListener(() =>
            services.GetChild(index).GetChild(0).GetComponent<Toggle>().isOn = !services.GetChild(index).GetChild(0).GetComponent<Toggle>().isOn
            );

        }
    }


}
