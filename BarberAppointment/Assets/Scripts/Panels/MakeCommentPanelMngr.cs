using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;


public class MakeCommentPanelMngr : MonoBehaviour
{
    WarningPanelMngr warningPanelMngr;

    public Transform[] pointBtns;

    public TMP_InputField commentInput;

    [SerializeField] GameObject SendButon;

    [SerializeField] GameObject warningText;
    [SerializeField] GameObject warningText1;

    [NonSerialized] public int point = -1;

    bool commentInputIsOk;
    bool pointIsOk;



    private void Start()
    {
        warningPanelMngr = AppManager.Instance.Panels[6].GetComponent<WarningPanelMngr>();
    }



    public void SetPointButtons(int id)
    {
        point = id + 1;                                                                                                // Indeks' in bir fazlasi puani verir.
        pointIsOk = true;

        warningText1.SetActive(false);                                                                                 // Onceden acildiysa kapansin diye.

        if (commentInputIsOk)
        {
            SendButon.GetComponent<CanvasGroup>().alpha = 1;
        }

        for (int i = 0; i < pointBtns.Length; i++)
        {
            if (i <= id)
            {
                pointBtns[i].GetChild(0).gameObject.SetActive(true);                                                   // Secili olan puana kadar olanlari da aktif et.
            }
            else
                pointBtns[i].GetChild(0).gameObject.SetActive(false);

            pointBtns[id].GetChild(1).gameObject.SetActive(true);                                                      // Puanin altindaki yazi icin.

            if (i != id)
                pointBtns[i].GetChild(1).gameObject.SetActive(false);                                                  // Puanin altindaki yazi icin.
        }

    }

    public void SendsComment()
    {

        if (commentInput.text.Length > 0)
        {
            if (point != -1)
            {
                warningPanelMngr.closeButton.SetActive(false);

                warningPanelMngr.OpenWarninPanel("~Yorum İletildi~", "Ana menüye dönmek istermisiniz ?", 2, 40);
                StartCoroutine(SendsCommentRoutine());
            }
            else
            {
                warningText.SetActive(false);
                warningText1.SetActive(true);

                RectTransform stars = pointBtns[0].parent.GetComponent<RectTransform>();
                StartCoroutine(Shake(stars));

            }
        }
        else
        {
            RectTransform inputPanel = commentInput.GetComponent<RectTransform>();
            warningText.SetActive(true);
            StartCoroutine(Shake(inputPanel));
        }

    }


    public IEnumerator SendsCommentRoutine()
    {
        WWWForm form = new();

        form.AddField("userName", PlayerPrefs.GetString("userName"));
        form.AddField("barberID", AppManager.Instance.barberID);
        form.AddField("commentText", commentInput.text);
        form.AddField("commentDate", DateTime.Now.Date.ToString("yyyy-MM-dd"));
        form.AddField("barberPoint", point);

        UnityWebRequest request = UnityWebRequest.Post("https://henka.test/APIs/add_comment.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler?.text;

            if (response == "Success")
            {
                Debug.Log("Yorum başarıyla eklendi.");

                warningPanelMngr.closeButton.SetActive(false);

                warningPanelMngr.OpenWarninPanel("~Yorum İletildi~", "Ana menüye dönmek istermisiniz ?", 2, 40);
            }
            else
                Debug.LogError("Bilinmeyen hata: " + response);

        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }


    }



    IEnumerator Shake(RectTransform panel)
    {
        for (int i = 0; i < 3; i++)
        {
            panel.DOAnchorPosX(50, .5f);
            yield return new WaitForSeconds(.1f);
            panel.DOAnchorPosX(-50, .3f);
            yield return new WaitForSeconds(.1f);
            panel.DOAnchorPosX(0, .3f);
        }
    }



    public void LatterAdded()
    {
        if (commentInput.text.Length > 0)
        {
            commentInputIsOk = true;

            warningText.SetActive(false);

            if (pointIsOk)
                SendButon.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            commentInputIsOk = false;
            SendButon.GetComponent<CanvasGroup>().alpha = .7f;
        }

    }



    public void ClearCommentPanel()
    {
        commentInput.text = "";

        for (int i = 0; i < pointBtns.Length; i++)
        {
            pointBtns[i].GetChild(0).gameObject.SetActive(false);                                                  // Acik olan puanlari kapat.
            pointBtns[i].GetChild(1).gameObject.SetActive(false);                                                  // Acik olan puanin altindaki yaziyi da sil.
        }
    }


}
