using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{

    [Tooltip("0- nameText / 1- lastNameText / 2- userNameText / 3- passwordText / 4- phoneNumText")]
    [SerializeField] TMP_InputField[] inputTexts;

    [SerializeField] Toggle rememberMeToggle;

    [SerializeField] GameObject exitPanel;

    [SerializeField] Animator LoginOptionAnim;

    [SerializeField] Animator signUpOptionAnim;

    [SerializeField] GameObject[] boxesToBeClosed;

    [SerializeField] GameObject loginBtn;

    [SerializeField] GameObject signUpBtn;

    RectTransform loginPanel;

    public Button targetButton;



    void Start()
    {
        loginPanel = this.GetComponent<RectTransform>();

        LoginOptionAnim.SetBool("active", true);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitPanel.activeSelf)
                exitPanel.SetActive(false);
            else
                exitPanel.SetActive(true);
        }
    }

    public void LoginOption()
    {
        SetPanel(true);

        LoginOptionAnim.SetBool("active", true);
        signUpOptionAnim.SetBool("active", false);

        LoginOptionAnim.GetComponent<Canvas>().sortingOrder = 2;
        signUpOptionAnim.GetComponent<Canvas>().sortingOrder = 1;

        for (int i = 0; i < inputTexts.Length; i++)
        {
            inputTexts[i].transform.GetChild(2).gameObject.SetActive(false);
        }

    }

    public void SignUpOption()
    {
        SetPanel(false);

        LoginOptionAnim.SetBool("active", false);
        signUpOptionAnim.SetBool("active", true);

        LoginOptionAnim.GetComponent<Canvas>().sortingOrder = 1;
        signUpOptionAnim.GetComponent<Canvas>().sortingOrder = 2;

        for (int i = 0; i < inputTexts.Length; i++)
        {
            inputTexts[i].transform.GetChild(2).gameObject.SetActive(false);
        }

    }



    public void SignUp_Btn()
    {
        bool AllOk = true;

        for (int i = 0; i < inputTexts.Length; i++)
        {
            if (inputTexts[i].text == "")
            {

                inputTexts[i].transform.GetChild(2).gameObject.SetActive(true);
                inputTexts[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bu alan boş bırakılamaz !";


                if (AllOk)                                                                                                // Birden fazla hata varsa hepsi icin sallanmasina gerek yok. (zaten bir tane de olsa sallancak)
                    StartCoroutine(WrongTrial());

                AllOk = false;

            }
            else
                inputTexts[i].transform.GetChild(2).gameObject.SetActive(false);

        }

        if (inputTexts[4].text.Length != 10)                                                                              // Telefon numarasi icin yeterli karakter girilmediyse hata ver.
        {
            AllOk = false;

            inputTexts[4].transform.GetChild(2).gameObject.SetActive(true);
            inputTexts[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Eksik tuşladınız !";

            StartCoroutine(WrongTrial());
        }


        if (AllOk)
        {
            string name = inputTexts[0].text;
            string lastName = inputTexts[1].text;
            string userName = inputTexts[2].text;
            string password = inputTexts[3].text;
            string phoneNum = inputTexts[4].text;

            Debug.Log("Kullanıcı başarıyla eklendi.");

            PlayerPrefs.SetString("name", name);
            PlayerPrefs.SetString("lastName", lastName);
            PlayerPrefs.SetString("userName", userName);                                                                  // Veritabani islemleri icin bunlar lazim olacak.
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetString("phoneNum", phoneNum);

            RememberMe(userName);

            SceneManager.LoadScene("MainScene");

            StartCoroutine(SignUp());
        }


    }
    IEnumerator SignUp()
    {
        string name = inputTexts[0].text;
        string lastName = inputTexts[1].text;
        string userName = inputTexts[2].text;
        string password = inputTexts[3].text;
        string phoneNum = inputTexts[4].text;


        WWWForm form = new();
        form.AddField("name", name);
        form.AddField("lastName", lastName);
        form.AddField("userName", userName);
        form.AddField("password", password);
        form.AddField("phoneNum", phoneNum);

        UnityWebRequest request = UnityWebRequest.Post("https://target_Ip/api/signUp.php", form);                       // Veritabanı adresimiz.

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            if (response == "exists")
            {
                Debug.Log("Bu kullanici adı zaten var!");

                inputTexts[2].transform.GetChild(2).gameObject.SetActive(true);
                inputTexts[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bu kullanici adı zaten var!";
                StartCoroutine(WrongTrial());

            }
            else if (response == "success")
            {

                Debug.Log("Kullanıcı başarıyla eklendi.");

                PlayerPrefs.SetString("userName", userName);                                                              // Veritabani islemleri icin bunlar lazim olacak.
                PlayerPrefs.SetString("password", password);

                RememberMe(userName);

                SceneManager.LoadScene("MainScene");

            }
            else
                Debug.Log("Bilinmeyen hata: " + response);
        }
        else
        {
            Debug.Log("Bağlantı hatası: " + request.error);
        }
    }



    public void Login_Btn()
    {

        bool AllOk = true;

        for (int i = 0; i < 2; i++)
        {
            if (inputTexts[i + 2].text == "")
            {
                inputTexts[i + 2].transform.GetChild(2).gameObject.SetActive(true);
                inputTexts[i + 2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bu alan boş bırakılamaz !";

                if (AllOk)                                                                                                // Birden fazla hata varsa hepsi icin sallanmasina gerek yok. (zaten bir tane de olsa sallancak)
                    StartCoroutine(WrongTrial());

                AllOk = false;

            }
            else
                inputTexts[i + 2].transform.GetChild(2).gameObject.SetActive(false);

        }

        if (AllOk)
        {

            string userName = inputTexts[2].text;
            string password = inputTexts[3].text;
            PlayerPrefs.SetString("userName", userName);                                                                // Veritabani islemleri icin bunlar lazim olacak.
            PlayerPrefs.SetString("password", password);

            RememberMe(userName);                                                                                       // Beni hatirla secenegi etkinse kullaniciyi kaydet.

            SceneManager.LoadScene("MainScene");

            StartCoroutine(Login());
        }

    }
    IEnumerator Login()
    {

        string userName = inputTexts[2].text;
        string password = inputTexts[3].text;

        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post("https://target_Ip/api/login.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;

            if (response == "notExist")
            {
                Debug.Log("Böyle bir kullanıcı adı bulunmuyor !");

                inputTexts[2].transform.GetChild(2).gameObject.SetActive(true);
                inputTexts[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Böyle bir kullanıcı adı bulunmuyor !";
                StartCoroutine(WrongTrial());

            }
            else if (response == "wrong")
            {
                Debug.Log("Şifre yanlış !");

                inputTexts[3].transform.GetChild(2).gameObject.SetActive(true);
                inputTexts[3].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Şifre yanlış !";
                StartCoroutine(WrongTrial());

            }
            else if (response == "success")
            {
                Debug.Log("Giriş başarılı!");

                PlayerPrefs.SetString("userName", userName);                                                                // Veritabani islemleri icin bunlar lazim olacak.
                PlayerPrefs.SetString("password", password);

                RememberMe(userName);                                                                                       // Beni hatirla secenegi etkinse kullaniciyi kaydet.

                SceneManager.LoadScene("MainScene");                                                                        // Giris basariliysa sahne degistir
            }
        }
        else
        {
            Debug.Log("Bağlantı hatası: " + request.error);
        }
    }




    void RememberMe(string userName)
    {
        if (rememberMeToggle.isOn)
        {
            PlayerPrefs.SetString("userName", userName);
            PlayerPrefs.SetInt("rememberMe", 1);
        }
        else
        {
            PlayerPrefs.DeleteKey("userName");
            PlayerPrefs.SetInt("rememberMe", 0);
        }
    }




    IEnumerator WrongTrial()
    {
        for (int i = 0; i < 3; i++)
        {
            loginPanel.DOAnchorPosX(50, .5f);
            yield return new WaitForSeconds(.1f);

            loginPanel.DOAnchorPosX(-50, .3f);
            yield return new WaitForSeconds(.1f);

            loginPanel.DOAnchorPosX(0, .3f);
        }

    }



    void SetPanel(bool state)
    {

        if (state)
        {
            Vector2 newSize = new(loginPanel.sizeDelta.x, 520);
            this.GetComponent<RectTransform>().DOSizeDelta(newSize, 1f);

            for (int i = 0; i < boxesToBeClosed.Length; i++)
            {
                boxesToBeClosed[i].GetComponent<CanvasGroup>().DOFade(0, 1f);

                Vector2 boxSize = new(boxesToBeClosed[i].GetComponent<RectTransform>().sizeDelta.x, 0);
                boxesToBeClosed[i].GetComponent<RectTransform>().DOSizeDelta(boxSize, 1f);

                Vector2 size = new(boxesToBeClosed[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.x, 0);      // Icindeki resimde yatayda ezilsin diye.(normalde 0. indeks ama sonradan obje olusuyor -
                boxesToBeClosed[i].transform.GetChild(1).GetComponent<RectTransform>().DOSizeDelta(size, 1f);                   // ve 1. indekse denk geliyo)

            }

            loginBtn.SetActive(true);
            signUpBtn.SetActive(false);
        }
        else
        {
            Vector2 newSize = new(loginPanel.sizeDelta.x, 900);
            this.GetComponent<RectTransform>().DOSizeDelta(newSize, 1f);

            for (int i = 0; i < boxesToBeClosed.Length; i++)
            {

                boxesToBeClosed[i].GetComponent<CanvasGroup>().DOFade(1, 1f);

                Vector2 boxSize = new(boxesToBeClosed[i].GetComponent<RectTransform>().sizeDelta.x, 100);
                boxesToBeClosed[i].GetComponent<RectTransform>().DOSizeDelta(boxSize, 1f);

                Vector2 size = new(boxesToBeClosed[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.x, 60);
                boxesToBeClosed[i].transform.GetChild(1).GetComponent<RectTransform>().DOSizeDelta(size, 1f);

            }
            loginBtn.SetActive(false);
            signUpBtn.SetActive(true);
        }


    }



}
