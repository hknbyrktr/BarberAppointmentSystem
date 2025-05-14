using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using TMPro;
using System;




[System.Serializable]
public class Appointment
{
    public string name;
    public string lastName;
    public string appointmentDate;
    public string appointmentTime;
    public string registrationDate;
}
[System.Serializable]
public class appointmentList
{
    public Appointment[] appointments;
}




public class MyAppointmentMngr : MonoBehaviour
{

    WarningPanelMngr warningPanelMngr;


    [SerializeField] TextMeshProUGUI InfoText;

    [SerializeField] GameObject hRod;

    int xAmount;



    void Start()
    {
        warningPanelMngr = AppManager.Instance.Panels[6].GetComponent<WarningPanelMngr>();

    }


    public IEnumerator GetMyAppointment()
    {

        ClearMyAppointment();

        string userName = PlayerPrefs.GetString("userName");

        UnityWebRequest request = UnityWebRequest.Get("https://target_Ip/api/get_appointment.php?userName=" + userName);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string DBText = request.downloadHandler?.text;

            try
            {
                appointmentList appointmentList = JsonUtility.FromJson<appointmentList>(DBText);

                Vector2 pos = new(-300 + xAmount, 220);

                Transform appointmentInfos = this.transform.GetChild(1);

                for (int i = 0; i < appointmentList.appointments.Length; i++)
                {

                    for (int j = 0; j < 4; j++)
                    {

                        TextMeshProUGUI info = Instantiate(InfoText, appointmentInfos);
                        info.GetComponent<RectTransform>().anchoredPosition = pos;
                        
                        switch (j)
                        {
                            case 0:
                                {
                                    info.text = appointmentList.appointments[i].name + " " + appointmentList.appointments[i].lastName;    // Ad, Soyad yaz.
                                    pos = new(pos.x + 200, pos.y);                                                                        // Yan sutuna gec.
                                    break;
                                }
                            case 1:
                                {
                                    info.text = appointmentList.appointments[i].appointmentDate;
                                    pos = new(pos.x + 200, pos.y);
                                    break;
                                }
                            case 2:
                                {
                                    info.text = appointmentList.appointments[i].appointmentTime;
                                    pos = new(pos.x + 200, pos.y);
                                    break;
                                }
                            case 3:
                                {
                                    info.text = appointmentList.appointments[i].registrationDate;
                                    break;
                                }

                        }

                    }

                    RectTransform rod = Instantiate(hRod, appointmentInfos).GetComponent<RectTransform>();
                    rod.anchoredPosition = new Vector2(xAmount, pos.y - 50);

                    pos = new(-300 + xAmount, pos.y - 100);                                                                       // Bi alt satira gec.
                }

                if (appointmentList.appointments.Length == 0)                                                                     // Eger randevu yoksa.
                {
                    for (int i = 0; i < 4; i++)
                    {
                        TextMeshProUGUI empty = Instantiate(InfoText, this.transform.GetChild(1));
                        empty.text = "Randevu Yok";
                        empty.GetComponent<RectTransform>().anchoredPosition = pos;

                        pos = new(pos.x + 200, pos.y);
                    }


                }

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



    /// <summary>
    /// Ilk basta profil panel gozküyor. AppointmentPanel' in gozukmemesi icin onu saga kaydiriyoruz.s
    /// </summary>
    public void SetAppointmentPanel(int xAmount)
    {

        this.gameObject.SetActive(true);

        this.xAmount = xAmount;
        for (int i = 0; i < this.transform.childCount; i++)
        {

            for (int j = 0; j < this.transform.GetChild(i).childCount; j++)
            {
                Vector2 pos = this.transform.GetChild(i).GetChild(j).GetComponent<RectTransform>().anchoredPosition;

                this.transform.GetChild(i).GetChild(j).GetComponent<RectTransform>().anchoredPosition = new(pos.x + xAmount, pos.y);
            }
        }

    }




    public void ClearMyAppointment()
    {
        Transform appointmentInfos = this.transform.GetChild(1);

        for (int i = 0; i < appointmentInfos.childCount; i++)
        {
            Destroy(appointmentInfos.GetChild(i).gameObject);
        }
    }



}
