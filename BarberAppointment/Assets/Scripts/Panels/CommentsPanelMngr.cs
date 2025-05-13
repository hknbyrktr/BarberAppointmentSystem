using UnityEngine.Networking;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using System;
using TMPro;



//------------------------------------------- Time' lar icin.
[System.Serializable]
public class Comment
{
    public string name;
    public string lastName;
    public string commentText;
    public string commentDate;
    public int barberPoint;
}

[System.Serializable]
public class CommentList
{
    public Comment[] comments;
}
//------------------------------------------- 




public class CommentsPanelMngr : MonoBehaviour
{

    [SerializeField] Transform commentsTransform;
    [SerializeField] Transform evaluation;

    [SerializeField] GameObject commentObject;

    [SerializeField] Transform noValuePanel;


    public void MakeCommentButton()
    {
        StartCoroutine(AppManager.Instance.SetMainPanel(true));
        AppManager.Instance.LeftShift(1);
    }


    public IEnumerator CommentsForBarberCreate()
    {

        UnityWebRequest request = UnityWebRequest.Get("https://henka.test/APIs/get.comments.php?barberID=" + AppManager.Instance.barberID);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string DBText = request.downloadHandler?.text;
            CommentList commentList = JsonUtility.FromJson<CommentList>(DBText);
            int totalPoint = 0;

            for (int i = 0; i < commentList.comments.Length; i++)
            {
                Instantiate(commentObject, commentsTransform);

                commentsTransform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =                                     // Yorumu yapanin isminin ve soyisminin ilk harflerini profile yaz.
                    commentList.comments[i].name[0].ToString().ToUpper() + commentList.comments[i].lastName[0].ToString().ToUpper();

                commentsTransform.GetChild(i).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = commentList.comments[i].barberPoint.ToString();         // Yorumu yapanin berbere verdigi puan.

                commentsTransform.GetChild(i).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = commentList.comments[i].commentDate.ToString();                    // Yorum tarihi.

                commentsTransform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = commentList.comments[i].commentText.ToString();

                totalPoint += commentList.comments[i].barberPoint;

            }
            if (commentList.comments.Length > 0)
            {
                float avgPoint = (float)totalPoint / commentList.comments.Length;
                avgPoint = Math.Round(avgPoint, 1).ConvertTo<float>();

                evaluation.GetChild(1).GetComponent<TextMeshProUGUI>().text = avgPoint.ToString();                                                                                  // Berberin ort. puani.
                evaluation.GetChild(2).GetComponent<TextMeshProUGUI>().text = commentList.comments.Length.ToString() + " Değerlendirme";                                            // Berberin yorum sayisi. 
            }
            else
            {
                evaluation.GetChild(1).GetComponent<TextMeshProUGUI>().text = "?";
                evaluation.GetChild(2).GetComponent<TextMeshProUGUI>().text = "0 Değerlendirme";

                Transform panel = Instantiate(noValuePanel, commentsTransform);
                panel.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bu berber için yorum yapılmamış. Yorum yap seçeneğinden berberi ilk değerlendiren olabilirsiniz.";
            }


        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }

    }




}
