using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WarningPanelMngr : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI warningText;

    public GameObject closeButton;

    int state;

    Canvas warningCanvas;

    private void Start()
    {
        warningCanvas = GetComponent<Canvas>();
    }


    /// <summary>
    /// Uyari Panelini acar.
    /// <list type="table">
    /// <item><param name="_title"><em> _Title - </em>Panelin basligi.</param></item>
    /// <item><param name="_warningText"><em> _warningText - </em>Panelde yazacak olan uyari metni.</param></item>
    /// <item><param name="_state"><em> _state - </em>uc tane durum var.
    /// <para>-------- 0- uygulamadan cik.</para>
    /// <para>-------- 1- Oturumu kapat.</para>
    /// <para>-------- 2- Ana menuye don.</para>
    /// </param></item> 
    /// <item><param name="titleSize"><em> fontSize - </em>Baslik boyutu.</param></item>
    /// </list>
    /// </summary>
    public void OpenWarninPanel(string _title, string _warningText, int _state, int titleSize = 50)
    {
        title.text = _title;
        warningText.text = _warningText;

        state = _state;

        title.fontSize = titleSize;

        warningCanvas.sortingOrder = 3;
    }


    public void ClosePanel()
    {
        warningCanvas.sortingOrder = 0;
    }


    public void ExitPanelYes()
    {
        if (state == 0)                                                                                                                   // Uygulamadan cik.
        {
            Application.Quit();
        }
        else if (state == 1)                                                                                                              // Oturumu kapat.
        {
            PlayerPrefs.SetInt("rememberMe", 0);
            SceneManager.LoadScene("LoginScene");
        }
        else                                                                                                                              // Randevu alindi basa don.
        {

            int indeks = AppManager.Instance.currentPanelIndeks;

            if(indeks == 5)                                                                                                               // Eger MakeComment_Panelde ise.
            {
                FindObjectOfType<MakeCommentPanelMngr>().ClearCommentPanel();                                                             // MakeComment_Panel'i temizle.
            }

            AppManager.Instance.Panels[indeks].GetComponent<CanvasGroup>().alpha = 0;                                                     // Hangi panel aciksa onu kapat.
            AppManager.Instance.Panels[indeks].gameObject.SetActive(false);

            AppManager.Instance.Panels[0].gameObject.SetActive(true);                                                                     // Berberleri terkar ac.
            AppManager.Instance.Panels[0].GetComponent<CanvasGroup>().DOFade(1, 1f);

            for (int i = 0; i < AppManager.Instance.Panels.Length; i++)
            {
                Vector2 pos = new(0, AppManager.Instance.Panels[i].GetComponent<RectTransform>().anchoredPosition.y);                    // Butun panelleri ortaya geri getirdik.
                AppManager.Instance.Panels[i].GetComponent<RectTransform>().anchoredPosition = pos;


                if(AppManager.Instance.currentPanelIndeks >0)                                                                            // parents icin ayri bir for kullanmak gerekirdi ama hazir dongu bulmusuz kullan iste.
                {
                    AppManager.Instance.currentPanelIndeks--;

                    AppManager.Instance.DestroyGameObject();
                }
            }
            

            StartCoroutine(AppManager.Instance.SetMainPanel(false));                                                                     // Ana paneli buyut.

            warningCanvas.sortingOrder = 0;

            if (!closeButton.activeSelf)
                closeButton.SetActive(true);

        }
    }


    public bool ActiveSelf()
    {
        if (warningCanvas.sortingOrder == 3)
            return true;
        else
            return false;
    }
}
