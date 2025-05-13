using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


//------------------------------------------- Berberler icin.
[System.Serializable]
public class Barber
{
    public int barberID;
    public string name;
    public string lastName;
}

[System.Serializable]
public class BarberList
{
    public Barber[] barbers;
}
//-------------------------------------------   


public class BarbersPanelMngr : MonoBehaviour
{

    [SerializeField] Transform barbersTransform;
    [SerializeField] GameObject barberObject;

    DatesPanelMngr datesPanelMngr;

    private void Start()
    {
        StartCoroutine(GetBarbers());

        datesPanelMngr = AppManager.Instance.Panels[1].GetComponent<DatesPanelMngr>();
    }


    IEnumerator GetBarbers()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://henka.test/APIs/get_berbers.php");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string DBText = request.downloadHandler?.text;

            BarberList barberList = JsonUtility.FromJson<BarberList>(DBText);

            for (int i = 0; i < barberList.barbers.Length; i++)
            {
                Instantiate(barberObject, barbersTransform);

                barbersTransform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = barberList.barbers[i].name + " " + barberList.barbers[i].lastName;

                int barberID = barberList.barbers[i].barberID;

                barbersTransform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                {
                    print(AppManager.Instance.animationActive);
                    if (!AppManager.Instance.animationActive)                                                                             // LeftShift fonskiyonu gecis devam ediyosa calismaz. Eger burda da LesftShift gibi kosul -
                    {                                                                                                                     // koymazsak bunlar calisir ama sen goremezsin (cunku sahne hala kaymamis olcak). 
                        StartCoroutine(datesPanelMngr.GetDatesByBarberID(barberID));

                        AppManager.Instance.LeftShift(1);
                    }

                });

            }


        }
        else
        {
            Debug.Log($"Bağlantı hatası! : {request.error}");
        }
    }


}
