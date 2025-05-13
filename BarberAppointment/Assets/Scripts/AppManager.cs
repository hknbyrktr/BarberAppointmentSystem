using ScrolRect.Extensions;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class AppManager : Singleton<AppManager>
{

    [Tooltip("0-Barbers_Panel / 1- Dates_Panel / 2- Times_Panel / 3- Services_Panel / 4- Comments_Panel / 5- MakeComment_Panel / 6- Warning_Panel")]
    public RectTransform[] Panels;

    [SerializeField] GameObject[] parents;

    [Tooltip("0- InfoPanelMngr / 1- ProfileMngr / 2- MyAppointmentMngr")]
    public Transform[] InfoPanel;

    WarningPanelMngr warningPanelMngr;
    ProfileMngr profileMngr;
    MyAppointmentMngr myAppointmentMngr;


    public Scrollbar scrollBar;

    [SerializeField] TextMeshProUGUI panelTxt;

    [HideInInspector] public int barberID;
    [HideInInspector] public string appointmentDate;
    [HideInInspector] public string appointmentTime;


    string[] panelTexts = { "Berberlerimiz", "Tarihlerimiz", "Saatlerimiz", "Hizmetlerimiz", "Berber için yorumlar", "Berber için yorumunuzu yapın" };

    Color32[] colors = {                                                                           // Panel text yazilarinin renkleri icin.
    new Color32(171, 83, 0, 255),
    new Color32(131, 0, 171, 255),
    new Color32(27, 0, 120, 255),
    new Color32(210, 38, 79, 255),
    new Color32(171, 0, 79, 255),
    new Color32(171, 0, 164, 255)
    };


    [HideInInspector] public int currentPanelIndeks = 0;

    [HideInInspector] public bool animationActive = false;
    public GameObject date;






    private void Start()
    {
        warningPanelMngr = Panels[6].GetComponent<WarningPanelMngr>();
        profileMngr = InfoPanel[1].GetComponent<ProfileMngr>();
        myAppointmentMngr = InfoPanel[2].GetComponent<MyAppointmentMngr>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (warningPanelMngr.ActiveSelf())
            {
                warningPanelMngr.ClosePanel();
                return;
            }

            if (InfoPanel[0].GetComponent<Canvas>().sortingOrder == 2)
            {
                InfoPanel[0].GetComponent<InfoPanelMngr>().BackBtn();
                return;
            }

            RightShift();

        }
    }



    public void ProfileBtn()
    {
        InfoPanel[0].GetComponent<Canvas>().sortingOrder = 2;

        InfoPanel[0].GetComponent<Animator>().SetBool("Anim", true);

        StartCoroutine(profileMngr.SetProfilePanel());

        StartCoroutine(myAppointmentMngr.GetMyAppointment());

    }



    public void LeftShift(int x)
    {
        if (!animationActive)
        {
            animationActive = true;

            RectTransform currentPanel = Panels[currentPanelIndeks];
            currentPanelIndeks += x;
            RectTransform nextPanel = Panels[currentPanelIndeks];

            currentPanel.GetComponent<CanvasGroup>().DOFade(0, 1f);

            currentPanel.DOAnchorPosX(-850, 1f).OnComplete(() =>
            {
                currentPanel.gameObject.SetActive(false);
                animationActive = false;
            });


            if (currentPanel == Panels[2] || currentPanel == Panels[4])                                            // Services_Panel ve makeComment_Panel' de ScrollBar olmasin.
            {
                scrollBar.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
                {
                    scrollBar.gameObject.SetActive(false);
                });
            }


            if (currentPanelIndeks != 3 && currentPanelIndeks != 5)                                                // Butun paneller aynı ScrollBar'i kullaniyor. Bar'in sacmalamamasi icin eskisinden silmemiz lazim.
            {
                parents[currentPanelIndeks - x].transform.parent.GetComponent<MultiTouchScrollRect>().verticalScrollbar = null;
            }

            nextPanel.gameObject.SetActive(true);

            if (currentPanelIndeks != 3 && currentPanelIndeks != 5)                                                // Sonrakine de eklememiz lazim. Manuel koyarsak kullanici geri-ileri yaptiginda barla olan baglanti kalmaz. 
            {
                parents[currentPanelIndeks].transform.parent.GetComponent<MultiTouchScrollRect>().verticalScrollbar = scrollBar;
            }


            nextPanel.GetComponent<CanvasGroup>().DOFade(1, 1f);

            StartCoroutine(PanelText());

        }
    }


    public void RightShift()
    {
        if (!animationActive)
        {
            if (currentPanelIndeks == 0)
            {
                string title = "~Çıkış Yap~";
                string warningText = "Çıkış yapmak istediğinize emin misiniz?";
                warningPanelMngr.OpenWarninPanel(title, warningText, 0);

                return;
            }

            DestroyGameObject();

            int x = 1;
            if (currentPanelIndeks == 4)                                                              // Yorumlar paneli duzgunce sirada olmadigi icin ekstra kod yazmak yerine bu sekilde uyarlandi.
                x = 3;
            else if (currentPanelIndeks == 5)
                StartCoroutine(SetMainPanel(false));

            RectTransform currentPanel = Panels[currentPanelIndeks];
            currentPanelIndeks -= x;
            RectTransform prevPanel = Panels[currentPanelIndeks];

            animationActive = true;

            currentPanel.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
            {
                currentPanel.gameObject.SetActive(false);
                animationActive = false;                                                                            // Animasyon varken farkli bir animasyon olusmasin.
            });

            if (currentPanel == Panels[3] || currentPanel == Panels[5])                                             // Services_Panel ve MakeComment_Panel' de ScrollBar yoktu. Diger panele gecersek ScrollBar'i aktif edelim. 
            {
                scrollBar.gameObject.SetActive(true);
                scrollBar.GetComponent<CanvasGroup>().DOFade(1, 1f);
            }

            if (currentPanelIndeks != 2 && currentPanelIndeks != 4)                                                 // Butun paneller aynı ScrollBar'i kullaniyor. Bar'in sacmalamamasi icin eskisinden silmemiz lazim.
            {
                parents[currentPanelIndeks + x].transform.parent.GetComponent<MultiTouchScrollRect>().verticalScrollbar = null;
            }

            prevPanel.gameObject.SetActive(true);

            if (currentPanelIndeks != 2 && currentPanelIndeks != 4)                                                // Sonrakine de eklememiz lazim. Manuel koyarsak kullanici geri-ileri yaptiginda barla olan baglanti kalmaz. 
            {
                parents[currentPanelIndeks].transform.parent.GetComponent<MultiTouchScrollRect>().verticalScrollbar = scrollBar;
            }

            prevPanel.GetComponent<CanvasGroup>().DOFade(1, 1f);
            prevPanel.DOAnchorPosX(0, 1.5f);

            StartCoroutine(PanelText());

        }
    }




    IEnumerator PanelText()
    {
        string text = panelTexts[currentPanelIndeks];

        for (int i = panelTxt.text.Length; i > 0; i--)
        {
            panelTxt.text = panelTxt.text.Substring(0, panelTxt.text.Length - 1);
            yield return new WaitForSeconds(.02f);
        }

        panelTxt.color = colors[currentPanelIndeks];

        if (currentPanelIndeks == 4)
            panelTxt.fontSize = 65;
        else if (currentPanelIndeks == 5)
            panelTxt.fontSize = 54;
        else
            panelTxt.fontSize = 75;

        for (int i = 0; i < text.Length; i++)
        {
            panelTxt.text += text[i];
            yield return new WaitForSeconds(.02f);
        }


    }


    public IEnumerator SetMainPanel(bool state)                                                                    // Ana paneli buyult - kucult.
    {
        bool active;
        float yPos;
        float hSize;
        if (state)
        {
            active = false; yPos = 200.4f; hSize = 519.21f;
        }
        else
        {
            active = true; yPos = -140; hSize = 1200;
        }

        scrollBar.gameObject.SetActive(active);

        RectTransform mainPanel = this.GetComponent<RectTransform>();

        mainPanel.DOAnchorPos(new Vector2(0, yPos), 1f);

        Vector2 newSize = new(mainPanel.sizeDelta.x, hSize);
        mainPanel.DOSizeDelta(newSize, 1f);

        yield return null;
    }






    public void DestroyGameObject()
    {
        if (currentPanelIndeks != 0 && currentPanelIndeks != 3 && currentPanelIndeks != 5)
        {
            for (int i = 0; i < parents[currentPanelIndeks].transform.childCount; i++)
            {
                Destroy(parents[currentPanelIndeks].transform.GetChild(i).gameObject);
            }

        }

    }





}








