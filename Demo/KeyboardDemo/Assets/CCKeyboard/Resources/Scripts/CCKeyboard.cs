/**
*   键盘封装，使用简要说明
*   调用:
* 		展示
* 		CCKeyboard.Instance().ShowKeyboard();
* 
* 		关闭
*  		CCKeyboard.Instance().HiddenKeyboard();
* 
*  		清空文本
*  		CCKeyboard.Instance().ClearText();
* 
*  		监听回调，返回当前输入的文本
* 		CCKeyboard.Instance ().OnValueChanged = OnValueChanged;
* 
* @author: Raw
* @date: 2016/12/08
*
*/ 

using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CCKeyboard : MonoBehaviour
{
	//输入文本变化监听回调
	public delegate void OnValueChangeDelegate (string inputText);

	public OnValueChangeDelegate OnValueChanged;

	//键盘panel
	public GameObject keyboardPanel;

	//按钮预设
	private Button keyboardItem;

	//存放英文字母按钮数组
	private List<Button> wordKeyList;

	//存放需要改变的按钮数组
	private List<Button> changeKeyList;

	//用来判断当前键盘显示状态 true:显示英文字母 false:显示符号
	private bool isWord = true;

	//大小写切换 true：显示大写，false：显示小写
	private bool isCaps = false;

	//字母数字
	private List<string> wordList;

	//特殊字符
	private List<string> charList;

	//记录按下的字母
	private StringBuilder inputText;

	//大小写切换键
	private Button capsBtn;

	//符号切换键
	private Button charBtn;

	private static CCKeyboard keyboard;

	public static CCKeyboard Instance ()
	{
		if (!keyboard) {	
			keyboard = FindObjectOfType (typeof(CCKeyboard)) as CCKeyboard;
			if (!keyboard) {
				Debug.LogError ("There needs to one active CCKeyboard script on a GameObject of your scene.");
			}
		}
		return keyboard;
	}
		
	void Awake ()
	{
		wordKeyList = new List<Button> ();

		changeKeyList = new List<Button> ();

		inputText = new StringBuilder ();
	}


	void Start ()
	{
		wordList = new List<string> {
			"q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "7", "8", "9",
			"a", "s", "d", "f", "g", "h", "j", "k", "l", "4", "5", "6",
			"", "z", "x", "c", "v", "b", "n", "m", "1", "2", "3",
			"", "", "0", ""

		};

		charList = new List<string> {
			"#", "%", "^", "*", "$", "@", "&", "¥", "·", "`", "－", "_", "|",
			"\\", "/", ":", ";", "<", ">", "(", ")", "[", "]", "{", "}",
			"", ".", ",", "?", "!", "~", "‘", "“", "+", "-", "=",
			"", "", "...", ""
		};

		//初始化键盘
		if (keyboardPanel != null) {
			InitKeyboard ();
			keyboardPanel.SetActive (false);
		} 
	}

	//初始化键盘
	private void InitKeyboard ()
	{
		int rowNum = 0;
		int j = 0;

		Transform parentTrans = GameObject.Find("Keyboard").transform;

		//构建第一行按钮
		for (int i = 0; i < wordList.Count; i++) { 
			Button keyBtn = Instantiate(Resources.Load("Prefabs/KeyboardItem", typeof(Button))) as Button;
			RectTransform rectTransform = keyBtn.GetComponent<RectTransform> () as RectTransform;

			//处理换行
			if (i == 13) {
				rowNum = 1;
				j = 0;
			} else if (i == 25) {
				rowNum = 2;
				j = 0;
			} else if (i == 36) {
				rowNum = 3;
				j = 0;
			}

			//计算各个按钮的坐标位置
			float preX = rowNum * 31 + 4 + j * (rectTransform.rect.width + 2);
			float preY = rectTransform.rect.height / 2 + 4 + rowNum * (rectTransform.rect.height + 2);

			keyBtn.transform.position = new Vector3 (preX, -preY, 0);
			keyBtn.transform.SetParent (parentTrans, false);

			string title = wordList [i].ToString ();

			//处理26个英文字母
			if (title.Length == 1) {
				ASCIIEncoding asciiEncoding = new ASCIIEncoding ();
				int intAsciiCode = (int)asciiEncoding.GetBytes (title) [0];

				if (intAsciiCode >= 97 && intAsciiCode <= 122) {
					ColorBlock colorsBlock = keyBtn.GetComponent<Button> ().colors;
					colorsBlock.normalColor = new Color (72f / 255, 31f / 255, 140f / 255, 0.95f);
					keyBtn.GetComponent<Button> ().colors = colorsBlock;
					wordKeyList.Add (keyBtn);
				}
			} 

			//特殊符号按钮
			if (rowNum == 2 && j == 0) {
				ChangeBtnDefaultImage (ref keyBtn, "Images/symbol_n");
				ChangeBtnHighlightImage (ref keyBtn, "Images/symbol_h");
				ChangeBtnSelectedImage (ref keyBtn, "Images/symbol_s");
				keyBtn.onClick.AddListener (() => ExchangeChar (keyBtn));
				charBtn = keyBtn;
			} else if (rowNum == 3 && j == 0) {
				//大小写按键
				ChangeBtnDefaultImage (ref keyBtn, "Images/caps_n");
				ChangeBtnHighlightImage (ref keyBtn, "Images/caps_h");
				ChangeBtnSelectedImage (ref keyBtn, "Images/caps_s");
				keyBtn.onClick.AddListener (() => ReloadKeyboardWithCaps (keyBtn));
				capsBtn = keyBtn;
			} else if (rowNum == 3 && j == 1) {
				//空格
				Vector2 newSize = new Vector2 (370, rectTransform.rect.height);
				rectTransform.sizeDelta = newSize;
				ChangeBtnDefaultImage (ref keyBtn, "Images/space_n");
				ChangeBtnHighlightImage (ref keyBtn, "Images/space_h");
				keyBtn.onClick.AddListener (() => SpaceBtnClick ());
			} else if (rowNum == 3 && j == 2) {
				//数字键0
				var position = keyBtn.transform.localPosition;
				position.x += 310f;
				keyBtn.transform.localPosition = position;

				keyBtn.onClick.AddListener (() => KeyboardItemClick (keyBtn));
			} else if (rowNum == 3 && j == 3) {
				//删除键
				Vector2 newSize = new Vector2 (122, rectTransform.rect.height);
				rectTransform.sizeDelta = newSize;
				ChangeBtnDefaultImage (ref keyBtn, "Images/del_n");
				ChangeBtnHighlightImage (ref keyBtn, "Images/del_h");
				var position = keyBtn.transform.localPosition;
				position.x += 310f;
				keyBtn.transform.localPosition = position;	
				keyBtn.onClick.AddListener (() => DeleteInput ());
			} else {
				keyBtn.onClick.AddListener (() => KeyboardItemClick (keyBtn));
			}

			changeKeyList.Add (keyBtn);
			Text text = keyBtn.GetComponentInChildren<Text> ();
			text.text = title;
			j++;
		}
	}

	//符号键盘切换
	private void ExchangeChar (Button charBtn)
	{
		isWord = !isWord;

		if (!isWord) {
			ChangeBtnDefaultImage (ref charBtn, "Images/symbol_s");
		} else {
			ChangeBtnDefaultImage (ref charBtn, "Images/symbol_n");
		}

		List<string> titleArry = new List<string> (); 
		if (isWord) {
			titleArry = wordList;
		} else {
			titleArry = charList;
		}

		for (int i = 0; i < changeKeyList.Count; i++) {
			Button tmpBtn = changeKeyList [i];
			Text text = tmpBtn.GetComponentInChildren<Text> ();
			if (isCaps && titleArry [i].Length == 1) {
				text.text = titleArry [i].ToUpper ();
			} else {
				text.text = titleArry [i];
			}
		}
	}

	//输出按键文本
	private void KeyboardItemClick (Button btn)
	{
		Text text = btn.GetComponentInChildren<Text> ();
		inputText.Append (text.text);

		if (OnValueChanged != null) {
			OnValueChanged (inputText.ToString ());
		}
	}

	//切换大小写
	private void ReloadKeyboardWithCaps (Button capsBtn)
	{
		//如果当前为特殊符号，不处理。
		if (!isWord) {
			return;
		}

		isCaps = !isCaps;

		if (isCaps) {
			ChangeBtnDefaultImage (ref capsBtn, "Images/caps_s");
		} else {
			ChangeBtnDefaultImage (ref capsBtn, "Images/caps_n");
		}

		foreach (Button btn in wordKeyList) {
			Text text = btn.GetComponentInChildren<Text> ();
			if (isCaps) {
				text.text = text.text.ToUpper ();
			} else {
				text.text = text.text.ToLower ();
			}
		}
	}

	//空格按钮事件
	private void SpaceBtnClick ()
	{
		inputText.Append (" ");

		if (OnValueChanged != null) {
			OnValueChanged (inputText.ToString ());
		}
	}

	//删除按钮事件
	private void DeleteInput ()
	{
		if (inputText.Length > 0) {
			inputText.Remove (inputText.Length - 1, 1);

			if (OnValueChanged != null) {
				OnValueChanged (inputText.ToString ());
			}
		}
	}

	//设置button的默认图片
	private void ChangeBtnDefaultImage (ref Button btn, string imagePath)
	{
		//重置背景色
		ColorBlock colorsBlock = btn.GetComponent<Button> ().colors;
		colorsBlock.normalColor = Color.white;
		btn.GetComponent<Button> ().colors = colorsBlock;

		//修改按钮为图片状态切换样式
		btn.GetComponent<Button> ().transition = Selectable.Transition.SpriteSwap;

		btn.image.type = Image.Type.Simple;
		Sprite sprite = new Sprite ();
		sprite = Resources.Load (imagePath, sprite.GetType ()) as Sprite;
		btn.image.sprite = sprite;
	}

	//设置button的点击图片
	private void ChangeBtnSelectedImage (ref Button btn, string imagePath)
	{
		Sprite sprite = new Sprite ();
		sprite = Resources.Load (imagePath, sprite.GetType ()) as Sprite;

		var spriteState = btn.GetComponent<Button> ().spriteState;
		spriteState.pressedSprite = sprite;
		btn.GetComponent<Button> ().spriteState = spriteState;
	}

	//设置button的高亮图片
	private void ChangeBtnHighlightImage (ref Button btn, string imagePath)
	{
		Sprite sprite = new Sprite ();
		sprite = Resources.Load (imagePath, sprite.GetType ()) as Sprite;
		var spriteState = btn.GetComponent<Button> ().spriteState;
		spriteState.highlightedSprite = sprite;
		btn.GetComponent<Button> ().spriteState = spriteState;
	}

	//重置按钮样式
	private void ResetKeyboard ()
	{
		isCaps = false;
		isWord = true;

		ChangeBtnDefaultImage (ref charBtn, "Images/symbol_n");
		ChangeBtnDefaultImage (ref capsBtn, "Images/caps_n");

		for (int i = 0; i < changeKeyList.Count; i++) {
			Button tmpBtn = changeKeyList [i];
			Text text = tmpBtn.GetComponentInChildren<Text> ();
			if (isCaps && wordList [i].Length == 1) {
				text.text = wordList [i].ToUpper ();
			} else {
				text.text = wordList [i];
			}
		}

		foreach (Button btn in wordKeyList) {
			Text text = btn.GetComponentInChildren<Text> ();
			text.text = text.text.ToLower ();
		}
	}
		
	//显示键盘
	public void ShowKeyboard ()
	{
		if (keyboardPanel != null) {
			keyboardPanel.SetActive (true);
		}
	}

	// 隐藏键盘
	public void HiddenKeyboard ()
	{
		//清空输入文本
		inputText.Remove (0, inputText.Length);

		ResetKeyboard ();

		if (keyboardPanel != null) {
			keyboardPanel.SetActive (false);
		}
	}

	//清空已输入的文本
	public void ClearText()
	{
		//清空输入文本
		inputText.Remove (0, inputText.Length);
	}
		
}

