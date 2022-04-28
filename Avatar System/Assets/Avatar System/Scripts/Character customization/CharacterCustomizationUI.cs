using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomizationUI : MonoBehaviour
{
    public AvatarSystem avatarSystem;
    public Transform buttonsParent;
    public Transform buttonsPanel;
    public Button thumbnailButtonPrefab;
    public GameObject buttons;
    public Sprite[] buttonSprites;
    public Image headerImage;
    public ScrollRect scrollRect;
    public Sprite maleButtonSprite;
    public Sprite femaleButtonSprite;
    public Sprite colorButtonSprite;
    public Transform spawnTransform;
    public Button colorButtonPrefab;
    public Color buttonOverlaySelectedColor = new Color(0, 1, 1);

    private int[] _currentStyle;
    private int[] _defaultMaleStyle;
    private int[] _defaultFemaleStyle;
    private int[] _buttonSelected;

    private int _skinColor;
    private int _hairColor;
    private int _facialHairColor;
    private int _eyebrowsColor;
    private int _eyesColor;
    private int _sex;
    private int _bodyPartsCount;
    private bool _isMale = true;
    private List<Button> _thumbnailsButtons = new List<Button>();
    private List<WereableObject>[] _femaleBodyParts;
    private List<WereableObject>[] _maleBodyParts;
    private GameObject _avatar;


    private void Awake()
    {
        _bodyPartsCount = avatarSystem.bodyPartsCount;
        _currentStyle = new int[_bodyPartsCount];
        _buttonSelected = new int[_bodyPartsCount];

        SetInitialStyles();
        SetDefaultStyles();
    }


    private void Start()
    {
        InitializeBodyPartBySex();
        _avatar = avatarSystem.InstanceAvatar(spawnTransform, true, "Avatar");
        SetAvatar();
        ShowBodyPanel();
    }


    private void SetAvatar()
    {
        _currentStyle[AvatarSystem.UPPER] = 0;
        _currentStyle[AvatarSystem.LOWER] = 0;
        _currentStyle[AvatarSystem.FEET] = 0;
        _currentStyle[AvatarSystem.EYES] = 0;
        _currentStyle[AvatarSystem.MOUTH] = 0;
        _currentStyle[AvatarSystem.EYEBROWS] = 0;

        _hairColor = 0;
        _facialHairColor = 0;
        _eyebrowsColor = 0;
        _skinColor = 0;
        _eyesColor = 0;
        _sex = 0;

        avatarSystem.SetAvatar(
            _avatar,
            null,                                  //hair
            _currentStyle[AvatarSystem.UPPER],     //upper
            _currentStyle[AvatarSystem.LOWER],     //lower
            _currentStyle[AvatarSystem.FEET],      //feet
            null,                                  //facial hair
            null,                                  //earring
            null,                                  //mask
            null,                                  //hat
            _currentStyle[AvatarSystem.EYES],      //eyes
            _currentStyle[AvatarSystem.MOUTH],     //mouth
            _currentStyle[AvatarSystem.EYEBROWS],  //eyebrows
            _skinColor,                            //skin color
            _hairColor,                            //hair color
            _eyesColor                             //eyes color
         );

    }


    private void SetInitialStyles()
    {
        for (int i = 0; i < _currentStyle.Length; i++)
        {
            _currentStyle[i] = -1;
        }
    }


    public void SetSkinColor(int colorIndex)
    {
        _skinColor = colorIndex;
        avatarSystem.SetSkinColor(colorIndex, _avatar);

    }


    public void SetHairColor(int colorIndex)
    {
        _hairColor = colorIndex;
        avatarSystem.SetHeadHairColor(colorIndex, _avatar);
    }


    public void SetFacialHairColor(int colorIndex)
    {
        _facialHairColor = colorIndex;
        avatarSystem.SetFacialHairColor(colorIndex, _avatar);
    }


    public void SetEyesColor(int colorIndex)
    {
        _eyesColor = colorIndex;
        avatarSystem.SetEyesColor(colorIndex, _avatar);
    }

    public void SetEyebrowsColor(int colorIndex)
    {
        _eyebrowsColor = colorIndex;
        avatarSystem.SetEyebrowsColor(colorIndex, _avatar);
    }


    public void SetMask(int index)
    {
        _currentStyle[AvatarSystem.MASK] = index;
        avatarSystem.SetBodyPart(AvatarSystem.MASK, index, _avatar);
    }


    public void SetBodyMesh(bool isMale)
    {
        _avatar = avatarSystem.SetBaseMesh(isMale, _avatar);

        if (isMale)
            SetStyle(_defaultMaleStyle);
        else
            SetStyle(_defaultFemaleStyle);

        ResetSelectedOption();

        for (int i = 0; i < _currentStyle.Length; i++)
        {
            print(i);
            if (_currentStyle[i] != -1)
                avatarSystem.SetBodyPart(i, _currentStyle[i], _avatar);
        }
        avatarSystem.SetHeadHairColor(_hairColor, _avatar);
        avatarSystem.SetFacialHairColor(_facialHairColor, _avatar);
        avatarSystem.SetEyebrowsColor(_eyebrowsColor, _avatar);
        avatarSystem.SetEyesColor(_eyesColor, _avatar);
        avatarSystem.SetSkinColor(_skinColor, _avatar);

        _isMale = isMale;
        _sex = (_isMale) ? 0 : 1;
    }

    private void SetDefaultStyles()
    {
        _defaultFemaleStyle = new int[_bodyPartsCount];
        _defaultMaleStyle = new int[_bodyPartsCount];

        _defaultMaleStyle[AvatarSystem.UPPER] = 0;
        _defaultMaleStyle[AvatarSystem.LOWER] = 0;
        _defaultMaleStyle[AvatarSystem.FEET] = 0;
        _defaultMaleStyle[AvatarSystem.EYES] = 0;
        _defaultMaleStyle[AvatarSystem.MOUTH] = 0;
        _defaultMaleStyle[AvatarSystem.EYEBROWS] = 0;
        _defaultMaleStyle[AvatarSystem.HAIR] = -1;
        _defaultMaleStyle[AvatarSystem.FACIAL_HAIR] = -1;
        _defaultMaleStyle[AvatarSystem.EARRING] = -1;
        _defaultMaleStyle[AvatarSystem.HAT] = -1;
        _defaultMaleStyle[AvatarSystem.HEAD] = -1;
        _defaultMaleStyle[AvatarSystem.MASK] = -1;

        _defaultFemaleStyle[AvatarSystem.UPPER] = 2;
        _defaultFemaleStyle[AvatarSystem.LOWER] = 2;
        _defaultFemaleStyle[AvatarSystem.FEET] = 0;
        _defaultFemaleStyle[AvatarSystem.EYES] = 0;
        _defaultFemaleStyle[AvatarSystem.MOUTH] = 0;
        _defaultFemaleStyle[AvatarSystem.EYEBROWS] = 0;
        _defaultFemaleStyle[AvatarSystem.HAIR] = 3;
        _defaultFemaleStyle[AvatarSystem.FACIAL_HAIR] = -1;
        _defaultFemaleStyle[AvatarSystem.EARRING] = -1;
        _defaultFemaleStyle[AvatarSystem.HAT] = -1;
        _defaultFemaleStyle[AvatarSystem.HEAD] = -1;
        _defaultFemaleStyle[AvatarSystem.MASK] = -1;


    }

    private void SetStyle(int[] style)
    {
        for (int i = 0; i < style.Length; i++)
        {
            _currentStyle[i] = style[i];
        }
    }

    private void InitializeBodyPart(int bodyPart, int initializationIndex = 0)
    {
        if (avatarSystem.IsBodyPartRemoveable(bodyPart))
        {
            avatarSystem.RemoveBodyPart(bodyPart, _avatar);
            _currentStyle[bodyPart] = -1;
        }
        else
            _currentStyle[bodyPart] = initializationIndex;
    }


    private void InitializeBodyPartBySex()
    {
        _femaleBodyParts = new List<WereableObject>[_bodyPartsCount];
        _maleBodyParts = new List<WereableObject>[_bodyPartsCount];

        for (int i = 0; i < _bodyPartsCount; i++)
        {
            _femaleBodyParts[i] = avatarSystem.GetBodyPartsBySex(i, false);
            _maleBodyParts[i] = avatarSystem.GetBodyPartsBySex(i, true);
        }
    }

    public GameObject selectedButtonPrefab;
    private void CreateThumbnailsButtons(int bodyPart, bool isMale, Transform parent = null)
    {
        List<WereableObject> wereables;
        if (isMale)
            wereables = _maleBodyParts[bodyPart];
        else
            wereables = _femaleBodyParts[bodyPart];

        var buttonsParent = (parent == null) ? this.buttonsParent : parent;

        for (int i = 0; i < wereables.Count; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, buttonsParent);
            var tex = avatarSystem.GetThumbnail(bodyPart, wereables[i].index);
            button.image.sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0, 1)
            );
            var index = wereables[i].index;
            var option = i;
            button.onClick.AddListener(delegate () { SetBodyPart(bodyPart, index); });
            button.onClick.AddListener(delegate () { SetButtonSelected(button.gameObject, _thumbnailsButtons); });
            button.onClick.AddListener(delegate () { SaveSelectedOption(bodyPart, option); });
            _thumbnailsButtons.Add(button);
            if (_buttonSelected[bodyPart] == i)
                InstantiateSelectedButton(button.transform, true);
            else
                InstantiateSelectedButton(button.transform);
        }
    }

    private void SaveSelectedOption(int bodyPart, int index)
    {
        _buttonSelected[bodyPart] = index;
    }

    private void ResetSelectedOption()
    {
        for (int i = 0; i < _buttonSelected.Length; i++)
        {
            _buttonSelected[i] = 0;
        }
    }

    public void SetSexTab()
    {
        var buttonMale = Instantiate(thumbnailButtonPrefab, buttonsParent);
        buttonMale.image.sprite = maleButtonSprite;
        buttonMale.onClick.AddListener(delegate () { SetBodyMesh(true); });

        var buttonFemale = Instantiate(thumbnailButtonPrefab, buttonsParent);
        buttonFemale.image.sprite = femaleButtonSprite;
        buttonFemale.onClick.AddListener(delegate () { SetBodyMesh(false); });

        _thumbnailsButtons.Add(buttonMale);
        _thumbnailsButtons.Add(buttonFemale);
    }


    public void SetSkinTab()
    {
        var materials = avatarSystem.skinMaterials;

        for (int i = 0; i < materials.Length; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, buttonsParent);
            button.image.color = materials[i].color;
            var index = i;
            button.onClick.AddListener(delegate () { SetSkinColor(index); });
            _thumbnailsButtons.Add(button);
        }
    }


    public void SetEyesColorTab()
    {
        var colors = avatarSystem.eyesColors;
        for (int i = 0; i < colors.Length; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, buttonsParent);
            button.image.color = colors[i];
            var index = i;
            button.onClick.AddListener(delegate () { SetEyesColor(index); });
            _thumbnailsButtons.Add(button);
        }
    }


    public void SetHairColorTab(Transform parent = null)
    {
        //var materials = avatarSystem.hairMaterials;
        //var buttonsParent = (parent == null) ? this.buttonsParent : parent;

        //for (int i = 0; i < materials.Length; i++)
        //{
        //    var button = Instantiate(thumbnailButtonPrefab, buttonsParent);
        //    button.image.color = materials[i].color;
        //    var index = i;
        //    button.onClick.AddListener(delegate () { SetHairColor(index); });
        //    _thumbnailsButtons.Add(button);
        //}

        var colors = avatarSystem.hairColors;
        var buttonsParent = (parent == null) ? this.buttonsParent : parent;

        for (int i = 0; i < colors.Length; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, buttonsParent);
            button.image.color = colors[i];
            var index = i;
            button.onClick.AddListener(delegate () { SetHairColor(index); });
            _thumbnailsButtons.Add(button);
        }
    }


    public void ChangeTab(int bodyPart)
    {
        //_tabSelected = bodyPart;
        CreateThumbnailsButtons(bodyPart, _isMale);
    }


    public void SetBodyPart(int bodyPart, int index)
    {
        avatarSystem.SetBodyPart(bodyPart, index, _avatar);

        if (bodyPart == AvatarSystem.HAIR ||
            bodyPart == AvatarSystem.FACIAL_HAIR ||
            bodyPart == AvatarSystem.EYEBROWS)
        {
            avatarSystem.SetHeadHairColor(_hairColor, _avatar);
            avatarSystem.SetFacialHairColor(_facialHairColor, _avatar);
            avatarSystem.SetEyebrowsColor(_eyebrowsColor, _avatar);
        }

        //avatarSystem.SetHeadHairColor(_hairColor, _avatar);
        //avatarSystem.SetFacialHairColor(_facialHairColor, _avatar);
        //avatarSystem.SetEyebrowsColor(_eyebrowsColor, _avatar);


        _currentStyle[bodyPart] = index;
    }

    public void Randomize()
    {
        avatarSystem.SetRandomAvatar(_avatar, _isMale);
    }

    private void DestroyThumbnailsButtons()
    {
        foreach (var item in _thumbnailsButtons)
        {
            DestroyImmediate(item.gameObject);
        }
        _thumbnailsButtons.Clear();
    }


    public void SetHeaderSprite(int index)
    {
        headerImage.sprite = buttonSprites[index];
    }


    public void SetPanelVisible(bool isVisible)
    {
        buttonsPanel.gameObject.SetActive(isVisible);
        buttons.SetActive(!isVisible);
        if (!isVisible) DestroyThumbnailsButtons();
    }

    public enum TypePanel { Buttons, NulleableButtons, ButtonsWithColor, NulleableButtonsWithColor };
    public enum Panel { Selection, Subselection, Colors }
    public enum Section {
        Hair,
        Upper,
        Lower,
        Feet,
        FacialHair,
        Earring,
        Mask,
        Hat,
        Head,
        Eyebrows,
        Eyes,
        Mouth,
        Sex,
        EyesColor,
        HairColor,
        FacialHairColor,
        EyebrowsColor,
        SkinColor
    }
    public GameObject[] panels;
    public GameObject[] panelsLayouts;
    public Button[] headButtons;
    public RectTransform selectionButtonsContainer;
    public Sprite colorSprite;
    public Sprite cancelSprite;
    public delegate void SetColorPart();
    private List<Button> _colorButtons = new List<Button>();
    private float _buttonWidth;
    public RectTransform selectionLayoutRect;
    public ScrollRect selectionScrollRect;
    public HorizontalLayoutGroup subselectionLayout;
    public HorizontalLayoutGroup selectionLayout;
    public GameObject bodyPanel;
    public HorizontalLayoutGroup sexButtonsLayout;
    public Sprite selectedImage;
    public Button[] sexButtons;
    private Vector2 _middleLeftAnchorMin = new Vector2(0, 0.5f);
    private Vector2 _middleLeftAnchorMax = new Vector2(0, 0.5f);
    private Vector2 _middleLeftPivot = new Vector2(0, 0.5f);

    public void ShowPanel(TypePanel typePanel, Panel panel, Section section, SetColorPart setColorMethod = null, bool destroyThumbnails = true, int bodyPart = 0)
    {
        if(destroyThumbnails) DestroyThumbnailsButtons();
        DestroyColorButtons();
        bodyPanel.SetActive(false);
        selectionLayout.gameObject.SetActive(true);

        panels[(int)panel].SetActive(true);

        if (typePanel == TypePanel.NulleableButtons)
        {
            AddDisabledButton(panel, section, bodyPart);
            SetContainerWithButton(false);
        }
        else if (typePanel == TypePanel.NulleableButtonsWithColor)
        {
            AddColorButton(panel, setColorMethod);
            AddDisabledButton(panel, section, bodyPart);
            
            SetContainerWithButton(true);
        }
        else if(typePanel == TypePanel.ButtonsWithColor)
        {
            AddColorButton(panel, setColorMethod);

            SetContainerWithButton(true);
        }
        else
            SetContainerWithButton(false);


        if (section >= 0 && (int)section < avatarSystem.bodyPartsCount && section != Section.Head)
        {
            CreateThumbnailsButtons((int)section, _isMale, panelsLayouts[(int)panel].transform);
        }
        else
        {
            switch(section)
            {
                case Section.Head:
                    SetHeadPanel();
                    break;

                case Section.HairColor:
                    SetHairColorPanel();
                    break;

                case Section.FacialHairColor:
                    SetFacialHairColorPanel();
                    break;

                case Section.Sex:
                    SetSexPanel();
                    break;

                case Section.EyesColor:
                    SetEyesColorPanel();
                    break;

                case Section.EyebrowsColor:
                    SetEyebrowsColorPanel();
                    break;


                case Section.SkinColor:
                    SetSkinColorPanel();
                    break;
            }
        }
    }

    private void AddColorButton(Panel panel, SetColorPart setColorMethod)
    {
        var button = Instantiate(thumbnailButtonPrefab, panels[(int)panel].transform /*.Find(_layoutName)*/);
        button.image.sprite = colorSprite;
        var rt = button.GetComponent<RectTransform>();
        rt.anchorMin = _middleLeftAnchorMin;
        rt.anchorMax = _middleLeftAnchorMax;
        rt.pivot = _middleLeftPivot;
        rt.anchoredPosition = new Vector2(subselectionLayout.spacing, 0);
        _thumbnailsButtons.Add(button);
        button.onClick.AddListener(delegate () { setColorMethod(); });

        _buttonWidth = button.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void AddDisabledButton(Panel panel, Section section, int bodyPart)
    {
        var cancelButton = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)panel].transform);
        cancelButton.image.sprite = cancelSprite;
        _thumbnailsButtons.Add(cancelButton);
        cancelButton.onClick.AddListener(delegate () { InitializeBodyPart((int)section); });
        cancelButton.onClick.AddListener(delegate () { SetButtonSelected(cancelButton.gameObject, _thumbnailsButtons); });

        if (_currentStyle[bodyPart] == -1)
            InstantiateSelectedButton(cancelButton.transform, true);
        else
            InstantiateSelectedButton(cancelButton.transform);
    }

    private void SetContainerWithButton(bool active)
    {
        if(active)
        {
            selectionButtonsContainer.offsetMin = new Vector2(_buttonWidth + subselectionLayout.spacing, selectionButtonsContainer.offsetMin.y);
            subselectionLayout.padding = new RectOffset((int)_buttonWidth + (int)subselectionLayout.spacing * 2, (int)_buttonWidth, 0, 0);
        }
        else
        {
            selectionButtonsContainer.offsetMin = new Vector2(0, selectionButtonsContainer.offsetMin.y);
            subselectionLayout.padding = new RectOffset((int)subselectionLayout.spacing, (int)subselectionLayout.spacing, 0, 0);
        }
    }

    private void SetHeadPanel()
    {
        foreach (var headButton in headButtons)
        {
            headButton.gameObject.SetActive(true);
        }
    }

    public void ShowUpperPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.Upper);
        SetPivotLeft(selectionLayoutRect);
    }

    public void ShowLowerPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.Lower);
        SetPivotLeft(selectionLayoutRect);
    }

    public void ShowFeetPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.Feet);
        SetPivotLeft(selectionLayoutRect);
    }

    public void ShowHeadPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.Head);
        ShowPanel(TypePanel.ButtonsWithColor, Panel.Subselection, Section.Eyes, ShowEyesColorPanel, true, AvatarSystem.EYES);
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowEyesPanel()
    {
        HideColorPanel();
        ShowPanel(TypePanel.ButtonsWithColor, Panel.Subselection, Section.Eyes, ShowEyesColorPanel, true, AvatarSystem.EYES);        
    }

    public void ShowMouthPanel()
    {
        HideColorPanel();
        ShowPanel(TypePanel.Buttons, Panel.Subselection, Section.Mouth);
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowHairPanel()
    {
        HideColorPanel();
        ShowPanel(TypePanel.NulleableButtonsWithColor, Panel.Subselection, Section.Hair, ShowHairColorsPanel, true, AvatarSystem.HAIR);
    }

    public void ShowHairColorsPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Colors, Section.HairColor, null, false);
    }

    public void ShowFacialHairColorsPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Colors, Section.FacialHairColor, null, false);
    }

    public void ShowFacialHairPanel()
    {
        HideColorPanel();
        ShowPanel(TypePanel.NulleableButtonsWithColor, Panel.Subselection, Section.FacialHair, ShowFacialHairColorsPanel, true, AvatarSystem.FACIAL_HAIR);
    }

    public void ShowEyebrowsPanel()
    {
        HideColorPanel();
        ShowPanel(TypePanel.ButtonsWithColor, Panel.Subselection, Section.Eyebrows, ShowEyebrowsColorPanel, true, AvatarSystem.EYEBROWS);
    }

    private bool _addedSexButtonsDelegates;
    public void ShowSexPanel()
    {
        HidePanels();
        panels[(int)Panel.Selection].SetActive(true);
        sexButtonsLayout.gameObject.SetActive(true);
        SetButtonOverlaySelected(sexButtons[_sex], sexButtons,null,true);
        if (!_addedSexButtonsDelegates)
            AddSexButtonsDelegate();
    }

    private void AddSexButtonsDelegate()
    {
        foreach (var button in sexButtons)
        {
            button.onClick.AddListener(delegate { SetButtonOverlaySelected(button, sexButtons, null, true); });
        }
    }

    public void ShowEyesColorPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Colors, Section.EyesColor, null, false);
    }

    public void ShowEyebrowsColorPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Colors, Section.EyebrowsColor, null, false);
    }

    

    public void ShowMaskPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.NulleableButtons, Panel.Selection, Section.Mask, null, true, AvatarSystem.MASK);
        SetPivotLeft(selectionLayoutRect);
    }

    public void ShowBodyPanel()
    {
        HidePanels();

        panels[(int)Panel.Selection].SetActive(true);
        selectionLayout.gameObject.SetActive(false);
        bodyPanel.SetActive(true);
        ShowSkinColorPanel();
    }

    public void ShowSkinColorPanel()
    {
        DestroyThumbnailsButtons();
        DestroyColorButtons();
        HidePanels();
        panels[(int)Panel.Selection].SetActive(true);
        panels[(int)Panel.Subselection].SetActive(true);
        SetSkinColorPanel();
        SetContainerWithButton(false);
    }

    private Vector2 _pivotCenter = new Vector2(0.5f,0.5f);
    private void SetPivotCenter(RectTransform rt)
    {
        rt.pivot = _pivotCenter;
    }

    private Vector2 _pivotLeft = new Vector2(0f, 0.5f);
    private void SetPivotLeft(RectTransform rt)
    {
        rt.pivot = _pivotLeft;
    }

    private void HidePanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }

        foreach (var headButton in headButtons)
        {
            headButton.gameObject.SetActive(false);
        }

        sexButtonsLayout.gameObject.SetActive(false);
    }

    private void SetHairColorPanel()
    {
        SetColorPanel(avatarSystem.hairColors, SetHairColor, _hairColor, Panel.Colors);
    }

    private void SetFacialHairColorPanel()
    {
        SetColorPanel(avatarSystem.hairColors, SetFacialHairColor, _facialHairColor, Panel.Colors);
    }

    private void SetEyesColorPanel()
    {
        SetColorPanel(avatarSystem.eyesColors, SetEyesColor, _eyesColor, Panel.Colors);
    }

    private void SetEyebrowsColorPanel()
    {
        SetColorPanel(avatarSystem.hairColors, SetEyebrowsColor, _eyebrowsColor, Panel.Colors);
    }

    private delegate void SetColorFunction(int index);

    private void SetColorPanel(Color[] colors, SetColorFunction SetColor, int savedConfig, Panel panel )
    {
        for (int i = 0; i < colors.Length; i++)
        {
            CreateColorButton(colors[i], panel, i, SetColor, savedConfig);
        }
        SetContainerWithButton(true);
    }

    private void SetColorPanel(Material[] materials, SetColorFunction SetColor, int savedConfig, Panel panel)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            CreateColorButton(materials[i].color, panel, i, SetColor, savedConfig, 60, 55);
        }
        SetContainerWithButton(true);
    }

    private void CreateColorButton(Color color, Panel panel, int index, SetColorFunction setColor, int savedConfig, float buttonSize = 35, float buttonInsideSize = 30)
    {
        var button = Instantiate(colorButtonPrefab, panelsLayouts[(int)panel].transform);
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonSize, buttonSize);
        button.image.sprite = colorButtonSprite;
        button.onClick.AddListener(delegate () { setColor(index); });
        button.onClick.AddListener(delegate () { SetColorButtonSelected(button); });
        button.onClick.AddListener(delegate () { SetButtonOverlaySelected(button, _colorButtons, _colorButtonInsideName); });

        var buttonInside = button.transform.Find(_colorButtonInsideName).gameObject;
        buttonInside.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonInsideSize, buttonInsideSize);
        buttonInside.GetComponent<Image>().color = color;

        _colorButtons.Add(button);

        if (savedConfig == index)
        {
            button.image.color = new Color(0, 1, 1);
        }
        else
        {
            button.image.color = color;
        }

    }

    private void DestroyColorButtons()
    {
        foreach (var item in _colorButtons)
        {
            Destroy(item.gameObject);
        }
        _colorButtons.Clear();
    }

    public void HideColorPanel()
    {
        panels[(int)Panel.Colors].SetActive(false);
    }

    private void SetSexPanel()
    {
        var buttonMale = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)Panel.Selection].transform);
        buttonMale.image.sprite = maleButtonSprite;
        buttonMale.onClick.AddListener(delegate () { SetBodyMesh(true); });
        buttonMale.onClick.AddListener(delegate () { SetButtonSelected(buttonMale.gameObject, _thumbnailsButtons); });
        _thumbnailsButtons.Add(buttonMale);
        InstantiateSelectedButton(buttonMale.transform);

        //var buttonFemale = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)Panel.Selection].transform);
        //buttonFemale.image.sprite = femaleButtonSprite;
        //buttonFemale.onClick.AddListener(delegate () { SetBodyMesh(false); });
        //buttonFemale.onClick.AddListener(delegate () { SetButtonSelected(buttonFemale.gameObject, _thumbnailsButtons); });
        //_thumbnailsButtons.Add(buttonFemale);
        //InstantiateSelectedButton(buttonFemale.transform);
    }


    public void SetSkinColorPanel()
    {
        var materials = avatarSystem.skinMaterials;
        SetColorPanel(materials, SetSkinColor, _skinColor, Panel.Subselection);
    }

    public void SetButtonSelected(GameObject button, List<Button> buttons)
    {
        foreach (var b in buttons)
        {
            var selectedImage = b.transform.Find(_selectedButtonName);
            if(selectedImage != null) selectedImage.gameObject.SetActive(false);
        }
        var selected = button.transform.Find(_selectedButtonName);
        if (selected != null) selected.gameObject.SetActive(true);
    }

    private string _colorButtonInsideName = "Color Button Inside";
    private string _sexButtonInsideName = "Sex Button Inside";
    public void SetColorButtonSelected(Button button)
    {
        foreach (var b in _colorButtons)
        {
            var buttonInside = b.transform.Find(_colorButtonInsideName).gameObject;
            var color = buttonInside.GetComponent<Image>().color;
            b.image.color = color;
        }
        button.image.color = buttonOverlaySelectedColor;
    }

    private void SetButtonOverlaySelected(Button button, List<Button> buttons, string buttonInsideName)
    {
        foreach (var b in buttons)
        {
            UnselectOverlayButton(b, buttonInsideName);
        }
        button.image.color = buttonOverlaySelectedColor;
    }

    private void SetButtonOverlaySelected(Button button, Button[] buttons, string buttonInsideName, bool transparent = false)
    {
        foreach (var b in buttons)
        {
            UnselectOverlayButton(b, buttonInsideName, transparent);
        }
        button.image.color = buttonOverlaySelectedColor;
    }

    private Color _transparentColor = new Color(0, 0, 0, 0);
    private void UnselectOverlayButton(Button b, string buttonInsideName, bool transparent = false)
    {
        if (transparent)
            b.image.color = _transparentColor;
        else
        {
            var buttonInside = b.transform.Find(buttonInsideName).gameObject;
            var color = buttonInside.GetComponent<Image>().color;
            b.image.color = color;
        }
    }

    private string _selectedButtonName = "Selected";
    private void InstantiateSelectedButton(Transform parent, bool active = false)
    {
        var selected = Instantiate(selectedButtonPrefab, parent);
        selected.name = _selectedButtonName;
        selected.SetActive(active);
    }

}
