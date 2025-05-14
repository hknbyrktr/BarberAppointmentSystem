using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;



//------------------------------------------- Time' lar icin.
[System.Serializable]
public class S_Time
{
    public string appointmentTime;
}

[System.Serializable]
public class TimeList
{
    public S_Time[] times;
}
//------------------------------------------- 


public class TimesPanelMngr : MonoBehaviour
{


    [SerializeField] Transform timesTransform;
    [SerializeField] GameObject timeObejct;




    public IEnumerator GetTimesByDate(int barberID, string appointmentDate)
    {

        WWWForm form = new();
        form.AddField("appointmentDate", appointmentDate);
        form.AddField("barberID", barberID);

        UnityWebRequest request = UnityWebRequest.Post("https://target_Ip/api/get_times.php", form);                                           // Url'yi uzatmasin diye Post ile yaptik.
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string DBText = request.downloadHandler?.text;
            TimeList timeList = JsonUtility.FromJson<TimeList>(DBText);

            for (int i = 0; i < timeList.times.Length; i++)
            {
                Instantiate(timeObejct, timesTransform);

                string timeString = timeList.times[i].ToString().Substring(0, 5);

                timesTransform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = timeList.times[i].appointmentTime.ToString().Substring(0, 5);           // Time butonlarinin text ini yaz.

                string time = timeList.times[i].appointmentTime.ToString();

                timesTransform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!AppManager.Instance.animationActive)
                    {
                        AppManager.Instance.barberID = barberID;
                        AppManager.Instance.appointmentDate = appointmentDate;
                        AppManager.Instance.appointmentTime = time;

                        AppManager.Instance.LeftShift(1);
                    }

                });

            }

        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }

    }



}
