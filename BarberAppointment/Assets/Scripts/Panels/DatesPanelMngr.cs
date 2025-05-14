using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


//------------------------------------------- Date' ler icin. 
[System.Serializable]
public class Date
{
    public string appointmentDate;
}

[System.Serializable]
public class DateList
{
    public Date[] dates;
}
//-------------------------------------------  



public class DatesPanelMngr : MonoBehaviour
{

    [SerializeField] TimesPanelMngr timesPanelMngr;

    [SerializeField] Transform datesTransform;
    [SerializeField] Transform noValuePanel;

    [SerializeField] GameObject dateObejct;

    CommentsPanelMngr commentsPanelMngr;


    public void CommentsForBarberButton()
    {

        if (!AppManager.Instance.animationActive)
        {
            commentsPanelMngr = AppManager.Instance.Panels[4].GetComponent<CommentsPanelMngr>();

            StartCoroutine(commentsPanelMngr.CommentsForBarberCreate());

            AppManager.Instance.LeftShift(3);                                                                   // 1. indeksten 3. indekse atlamak icin.
        }

    }



    public IEnumerator GetDatesByBarberID(int barberID)
    {
        AppManager.Instance.barberID = barberID;

        UnityWebRequest request = UnityWebRequest.Get("https://target_Ip/api/get_dates.php?barberID=" + barberID);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string DBText = request.downloadHandler?.text;

            DateList dateList = JsonUtility.FromJson<DateList>(DBText);

            for (int i = 0; i < dateList.dates.Length; i++)
            {
                Instantiate(dateObejct, datesTransform);
                                                                                                                                                           // Burada bu formatta : yyyy-mm-dd.
                DateTime date = DateTime.Parse(dateList.dates[i].appointmentDate);
                datesTransform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = date.ToString("dd / MM / yyyy");                             // Date butonlarinin text ini yaz.


                string appointmentDate = dateList.dates[i].appointmentDate.ToString();                                                                     // Zaten bir tane var. "appointmentDate" diye belirtmesekde olur yani. 

                datesTransform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!AppManager.Instance.animationActive)
                    {
                        StartCoroutine(timesPanelMngr.GetTimesByDate(barberID, appointmentDate));

                        AppManager.Instance.LeftShift(1);
                    }

                });
            }

            if (dateList.dates.Length == 0)
            {
                Transform panel = Instantiate(noValuePanel, datesTransform);
                panel.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Şuanda bu berber için boş randevu bulunmamakta. Geri dönüp farklı bir berber seçebilirsiniz.";
            }


        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }
    }


}
