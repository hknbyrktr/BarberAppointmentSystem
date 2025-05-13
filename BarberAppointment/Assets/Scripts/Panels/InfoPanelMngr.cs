using System.Collections;
using UnityEngine;
using DG.Tweening;




public class InfoPanelMngr : MonoBehaviour
{

    [SerializeField] MyAppointmentMngr myAppointmentMngr;

    [SerializeField] Transform profilePanel;
    [SerializeField] Transform myAppointmentInfo;

    [Tooltip("Paneldeki elemanlar X' te ne kadar kaycak?")]
    int xAmount = 1000;

    int currentPanelIndex = 0;


    bool animActive = false;

    void Start()
    {
        myAppointmentMngr.SetAppointmentPanel(xAmount);

    }


    public void BackBtn()
    {

        if (currentPanelIndex == 0 && !animActive)
        {
            GetComponent<Canvas>().sortingOrder = 0;
            GetComponent<Animator>().SetBool("Anim",false);
        }
        else
        {
            StartCoroutine(RightShift());
        }

    }


    public IEnumerator LeftShift()
    {
        if (!animActive)
        {
            animActive = true;
            currentPanelIndex++;

            for (int i = 0; i < profilePanel.childCount; i++)
            {
                RectTransform rectT = profilePanel.GetChild(i).GetComponent<RectTransform>();

                Vector2 pos = rectT.anchoredPosition;

                rectT.DOAnchorPosX(pos.x - xAmount, .5f).SetEase(Ease.InBack);

                yield return new WaitForSeconds(.1f);

            }

            yield return new WaitForSeconds(.1f);



            for (int i = 0; i < myAppointmentInfo.childCount; i++)
            {
                Transform myAppointmentInfoChild = myAppointmentInfo.GetChild(i);

                for (int j = 0; j < myAppointmentInfoChild.childCount; j++)
                {
                    RectTransform rectT = myAppointmentInfoChild.GetChild(j).GetComponent<RectTransform>();

                    Vector2 pos = rectT.anchoredPosition;

                    var tween = rectT.DOAnchorPos(new(pos.x - xAmount, pos.y), .5f).SetEase(Ease.InBack);

                    if (i == myAppointmentInfo.childCount - 1)                                                                          // Son elemanin islemi bitince animActive' i duzelt.
                    {

                        if (j == myAppointmentInfoChild.childCount - 1)
                        {
                            tween.OnComplete(() => animActive = false);
                        }
                    }

                    yield return new WaitForSeconds(.1f);

                }
            }



        }

    }


    public IEnumerator RightShift()
    {
        print("mdspvoms");
        if (!animActive)
        {
            print("aaaaaaaaa");
            animActive = true;
            currentPanelIndex--;


            for (int i = 0; i < myAppointmentInfo.childCount; i++)
            {
                Transform info = myAppointmentInfo.GetChild(i);

                for (int j = 0; j < info.childCount; j++)
                {
                    Vector2 pos = info.GetChild(j).GetComponent<RectTransform>().anchoredPosition;

                    info.GetChild(j).GetComponent<RectTransform>().DOAnchorPos(new(pos.x + xAmount, pos.y), .5f).SetEase(Ease.InBack);

                    yield return new WaitForSeconds(.1f);

                }
            }

            yield return new WaitForSeconds(.1f);

            for (int i = 0; i < profilePanel.childCount; i++)
            {
                Vector2 pos = profilePanel.GetChild(i).GetComponent<RectTransform>().anchoredPosition;

                var tween = profilePanel.GetChild(i).GetComponent<RectTransform>().DOAnchorPosX(pos.x + xAmount, .5f).SetEase(Ease.InBack);

                if (i == profilePanel.childCount - 1)                                                                                                       // Son elemanin islemi bitince animActive' i duzelt.
                {
                    tween.OnComplete(() => animActive = false);
                }

                yield return new WaitForSeconds(.1f);

            }
        }

    }

}
