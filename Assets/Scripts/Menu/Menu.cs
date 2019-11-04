using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject actualMenu;
    public List<SubMenu> subMenus;


    private void Awake()
    {
        subMenus.ForEach(s => s.button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                actualMenu?.SetActive(false);
                actualMenu = s.subMenu;
                s.subMenu?.SetActive(true);
            })) );
        
    }



}

[System.Serializable]
public class SubMenu
{
    public Button button;
    public GameObject subMenu;
}
