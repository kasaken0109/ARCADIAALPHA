using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonArray
{
    [SerializeField]
    private Button[] m_buttons = default;

    public Button[] Buttons => m_buttons;
}
public class ButtonSelectController : MonoBehaviour
{
    [SerializeField]
    private ButtonArray[] m_buttonField = default;

    private int selectHorizontalID = 0;

    private int selectVerticalID = 0;


    // Start is called before the first frame update
    void Start()
    {
      m_buttonField[0].Buttons[0].Select();

    }


    // Update is called once per frame
    //void Update()
    //{
    //    SetArrowValue();

    //}

    //private void SetArrowValue()
    //{
    //    if (Input.GetButtonDown("SelectHorizontalPosi"))
    //    {
    //        ButtonSelect(ArrowValue.right);
    //    }
    //    else if (Input.GetButtonDown("SelectHorizontalNega"))
    //    {
    //        ButtonSelect(ArrowValue.left);
    //    }
    //    if (Input.GetButtonDown("SelectVerticalPosi"))
    //    {
    //        ButtonSelect(ArrowValue.up);
    //    }
    //    else if (Input.GetButtonDown("SelectVerticalNega"))
    //    {
    //        ButtonSelect(ArrowValue.down);
    //    }
    //}

    public void ButtonSelect(Vector2 arrowValue)
    {
        Debug.Log("Calleded");
        if(arrowValue.x > 0) selectHorizontalID = selectHorizontalID >= m_buttonField.Length - 1 ? 0 : selectHorizontalID + 1;
        else selectHorizontalID = selectHorizontalID <= 0 ? m_buttonField.Length - 1 : selectHorizontalID - 1;
        if(arrowValue.y > 0) selectVerticalID = selectVerticalID >= m_buttonField[selectHorizontalID].Buttons.Length - 1 ? 0 : selectVerticalID + 1;
        else selectVerticalID = selectVerticalID <= 0 ? m_buttonField[selectHorizontalID].Buttons.Length - 1 : selectVerticalID - 1;
        //switch (arrowValue)
        //{
        //    case  Vector2.left:
        //        selectHorizontalID = selectHorizontalID <= 0 ? m_buttonField.Length - 1 : selectHorizontalID - 1;
        //        break;
        //    case ArrowValue.right:
        //        selectHorizontalID = selectHorizontalID >= m_buttonField.Length - 1 ? 0 : selectHorizontalID + 1;
        //        break;
        //    case ArrowValue.down:
        //        selectVerticalID = selectVerticalID <= 0 ? m_buttonField[selectHorizontalID].Buttons.Length - 1 : selectVerticalID - 1;
        //        break;
        //    case ArrowValue.up:
        //        selectVerticalID = selectVerticalID >= m_buttonField[selectHorizontalID].Buttons.Length - 1 ? 0 : selectVerticalID + 1;
        //        break;
        //    case ArrowValue.none:
        //        break;
        //    default:
        //        break;
        //}
        if (m_buttonField[selectHorizontalID].Buttons[selectVerticalID]) m_buttonField[selectHorizontalID].Buttons[selectVerticalID].Select();
        else ButtonSelect(arrowValue);
    }

    //public void ButtonSelect(int arrowValue)
    //{
    //    switch (arrowValue)
    //    {
    //        case 1:
    //            ButtonSelect(ArrowValue.left);
    //            break;
    //        case 2:
    //            ButtonSelect(ArrowValue.right);
    //            break;
    //        case 3:
    //            ButtonSelect(ArrowValue.up);
    //            break;
    //        case 4:
    //            ButtonSelect(ArrowValue.down);
    //            break;
    //        case 0:
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public ArrowValue arrowValue = ArrowValue.none;

    public enum ArrowValue
    {
        left,
        right,
        down,
        up,
        none,
    }
}
