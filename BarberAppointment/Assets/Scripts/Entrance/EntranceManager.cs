using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EntranceManager: MonoBehaviour
{

    [SerializeField]
    CanvasGroup fadeScreen;

    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI WarningText;

    bool isConnected = false;
    readonly float checkInterval = 2f;                                                                          // Her 2 saniyede bir kontrol icin.
    float timer = 0f;



    private void Start()
    {
        StartCoroutine(StartTheSceneCoroutine());

        StartCoroutine(CheckConnectionCoroutine());
        StartCoroutine(OpenTheAppCoroutine());

    }

    void Update()
    {
        if (!isConnected)
        {
            timer += Time.deltaTime;
            if (timer >= checkInterval)
            {
                timer = 0f;
                StartCoroutine(CheckConnectionCoroutine());
            }
        }
        
    }


    IEnumerator CheckConnectionCoroutine()
    {
        
        UnityWebRequest request = UnityWebRequest.Get("https://henka.test/APIs/signUp.php");                    // Herhangi bir dosyaya baglanmaya calismamiz baglanti kontrolu icin yeterli.

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Bağlantı başarılı.");
            isConnected = true;
        }
        else
        {
            Debug.LogWarning($"Bağlantı başarısız: {request.error}");
            isConnected = false;
        }

        StartCoroutine(OpenTheAppCoroutine());
    }

    IEnumerator OpenTheAppCoroutine()
    {
        isConnected = true;
        if (isConnected)
        {

            if (WarningText.text != string.Empty) { WarningText.text = "Sunucu bağlantısı başarılı!"; }         // string.Empty yada "" . Ayni sey.

            yield return new WaitForSeconds(1.5f);                                                              // Yukleme cubugu tam anlaminda islevsel olmadigi icin. Bilgisayar/Telefon yuklemeyi cok hizli -
                                                                                                                // yapiyor. Ufak bir yavaslatma.

            AsyncOperation operation;


            if (PlayerPrefs.GetInt("rememberMe") == 0)
                operation = SceneManager.LoadSceneAsync("LoginScene");
            else
                operation = SceneManager.LoadSceneAsync("MainScene");


            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                slider.value = progress;
                yield return null;                                                                              // Bu satir olmazsa diger isleme gecemez ve editor coker.
            }


        }
        else
        {
            yield return new WaitForSeconds(1.5f);

            slider.value = 0.3f;                                                                                // Dolmayan bir gorunum.

            yield return new WaitForSeconds(.5f);                                                               // Ufak bir gecikme daha.

            Debug.LogWarning("Uygulama başlatılamıyor. Lütfen sunucu bağlantınızı kontrol edin.");

            WarningText.text = "Sunucuya bağlanılamadı. Bağlantınızı kontrol edin!";
        }
    }

    IEnumerator StartTheSceneCoroutine()
    {

        fadeScreen.GetComponent<CanvasGroup>().alpha = 1;

        yield return new WaitForSeconds(.5f);

        fadeScreen.DOFade(0, 1f).SetEase(Ease.InFlash);

    }


}
