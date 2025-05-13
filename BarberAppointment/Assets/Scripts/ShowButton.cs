using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;


public class ShowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    [SerializeField] GameObject visibleText;
    [SerializeField] GameObject invisbleText;

    [SerializeField] Sprite visible;
    [SerializeField] Sprite invisible;



    public void OnPointerDown(PointerEventData eventData)
    {
        invisbleText.SetActive(false);
        visibleText.gameObject.SetActive(true);

        this.gameObject.GetComponent<Image>().sprite = visible;
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        invisbleText.SetActive(true);
        visibleText.gameObject.SetActive(false);

        this.gameObject.GetComponent<Image>().sprite = invisible;
    }


}
