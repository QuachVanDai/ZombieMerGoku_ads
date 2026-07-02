
using ExampleProject.Gameplay.Scenes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedJoystick : Joystick
{
    public RectTransform DragToPlay;
    public RectTransform hand;
    public RectTransform shadow;
    public GameObject left, right;


    // private void Update()
    // {
    //    if(Manager.Instance.GetRatioScreen()>= 1)
    //    {
    //        thisJoystick.anchoredPosition = new Vector2(550,-56f);
    //        thisJoystick.localScale = Vector2.one*sizethisJoystick;
    //    }
    //    else
    //    {
    //        thisJoystick.anchoredPosition = new Vector2(0,-447f);
    //        thisJoystick.localScale = Vector2.one;
    //    }
    // }

    public void SetDirection(bool _isLeft)
    {
        if (_isLeft)
        {
            if (left != null) left.SetActive(true);
            if (right != null) right.SetActive(false);
        }
        else
        {
            if (left != null) left.SetActive(false);
            if (right != null) right.SetActive(true);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (DragToPlay != null && DragToPlay.gameObject.activeSelf)
        {
            DragToPlay.gameObject.SetActive(false);
            if (hand != null) hand.gameObject.SetActive(false);
            if (shadow != null) shadow.gameObject.SetActive(false);
            /*Manager.SpawnTrafficJamVehicle();
            Manager.State = TrafficJamGameState.Playing;
            Manager.SoundBGMPlay();*/

        }
    }
}
