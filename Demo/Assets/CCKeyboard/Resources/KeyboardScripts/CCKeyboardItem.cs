using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CCKeyboardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Text title;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//hover in
	public void OnPointerEnter (PointerEventData eventData)
	{  
		title.color = Color.white;
	}

	//hover out
	public void OnPointerExit (PointerEventData eventData)
	{ 
		title.color = new Color (153f / 255, 102f / 255, 255f / 255, 242f / 255);
	}
}
