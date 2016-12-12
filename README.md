# CCVRKeyboardSDK
### 更新日志

---

###### Version:1.0.0.0 Date:2016.12.12
1. 建档

---

### 导入和使用

1. 在你的项目中，从`Assets -> Import Package -> Custom Package`导入`CoocaaVR_Keyboard_1.0.0.unitypackage`文件。
2. 此时在Unity中出现对话框，请保留所有复选框，并选择导入。
3. 将 `Keyboard.prefab`(路径为:`/CCKeyboard/Resources/Prefabs/Keyboard`)拖动到你场景中创建的Canvas中。  
4. 使用方法

  ##### Step 1: 添加输入监听 
  ```
  void OnEnable () {
    CCKeyboard.Instance ().OnValueChanged += OnValueChanged; 
  }
  ```
  ##### Step 2: 显示键盘，在需要展示的地方调用
  ```
  CCKeyboard.Instance().ShowKeyboard();  
  ```
  ##### Step 3: 关闭键盘，在需要关闭的地方调用
  ```
  CCKeyboard.Instance().HiddenKeyboard(); 
  ```
  ##### Step 4: 清空当前已输入的字符
  ```
  CCKeyboard.Instance().ClearText(); 
  ```
  ##### Step 5: 取消输入监听
  ```
  void OnDisable(){
    CCKeyboard.Instance ().OnValueChanged -= OnValueChanged; 
  }
```

---

### 其他事项
---

1. 所有的UI元素必须是一个Canvas的子对象。如果场景中还没有Canvas对象，可以使用GameObject->UI->Canvas创建新的Canvas。
2. 如果发现 `Canvas` 的 `Rect Transform` 不可编辑，则将 `Canvas` 组件中的 `Render Mode` 选项更改为 `World Space`。
  
---

**END**
