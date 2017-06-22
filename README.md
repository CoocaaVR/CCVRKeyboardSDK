# CCVRKeyboardSDK
### 更新日志

---

###### Version:2.0.0.0 Date:2017.06.22
1. 重写键盘单例模式，更为方便调用
2. 重写输入文本变化事件
3. 重命名图片资源、预设、脚本文件名，方便区分
4. 增加键盘文本赋值接口
5. 增加单个字符点击事件
6. 增加空格键点击事件
7. 增加删除键点击事件


###### Version:1.0.0.0 Date:2016.12.12
1. 建档

---

### 导入和使用

1. 在你的项目中，从`Assets -> Import Package -> Custom Package`导入`CoocaaVR_Keyboard_SDK.unitypackage`文件。
2. 此时在Unity中出现对话框，请保留所有复选框，并选择导入。
3. 将 `Keyboard.prefab`(路径为:`/Resources/KeyboardPrefabs/`)拖动到你场景中创建的Canvas中。  
4. 使用方法，在需要使用键盘的脚本中按照以下步骤添加：

  ##### Step 1: 订阅事件
  ```
  void OnEnable () {
	CCKeyboard.Instance.OnValueChangedEvent += ValueChanged;
	CCKeyboard.Instance.OnCharkeyClickEvent += CharkeyClick;
	CCKeyboard.Instance.OnSpaceKeyClickEvent += SpaceKeyClick;
	CCKeyboard.Instance.OnDeleteKeyClickEvent += DeleteKeyClick; 
  }
 ```  
 ##### Step 2: 取消订阅
 ```
  void OnDisable () {
    CCKeyboard.Instance.OnValueChangedEvent -= ValueChanged;
		CCKeyboard.Instance.OnCharkeyClickEvent -= CharkeyClick;
		CCKeyboard.Instance.OnSpaceKeyClickEvent -= SpaceKeyClick;
		CCKeyboard.Instance.OnDeleteKeyClickEvent -= DeleteKeyClick; 
  }
  ```
  ##### Step 3: 显示键盘
  ```
  CCKeyboard.Instance().ShowKeyboard();  
  ```
  
  ##### Step 4: 隐藏键盘
  ```
  CCKeyboard.Instance().HiddenKeyboard(); 
  ```
  
  ##### Step 5: 清空当前文本
  ```
  CCKeyboard.Instance().ClearText(); 
  ```
  
  ##### Step 6: 手动设置键盘文本
  ```
  CCKeyboard.Instance.inputText.Append("Input your text.");
  ```

---

### 其他事项
---

1. 所有的UI元素必须是一个Canvas的子对象。如果场景中还没有Canvas对象，可以使用GameObject->UI->Canvas创建新的Canvas。
2. 如果发现 `Canvas` 的 `Rect Transform` 不可编辑，则将 `Canvas` 组件中的 `Render Mode` 选项更改为 `World Space`。
  
---

**END**
