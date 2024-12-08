/*--------------------------------------
   Email  : hamza95herbou@gmail.com
   Github : https://github.com/herbou
----------------------------------------*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI ;

[RequireComponent (typeof(Button))]
public class ButtonRightClickListener : MonoBehaviour,IPointerClickHandler {
   
    public UnityEvent onRightClick;

    private Button button;



    private void Awake () {
      button = GetComponent<Button> () ;
    }

   public void OnPointerClick (PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right)
            StartCoroutine(waitClick());
   }
    IEnumerator waitClick()
    {
        yield return null;
        onRightClick?.Invoke();
    }

}
