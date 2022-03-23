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
    public Color colorButtonSelectedColor = new Color(0, 1, 1);

    private int[] _currentStyle;
    private int _skinColor;
    private int _hairColor;
    private int _eyesColor;
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
        SetInitialStyles();
    }


    private void Start()
    {
        InitializeBodyPartBySex();
        _avatar = avatarSystem.InstanceAvatar(spawnTransform, true, "Avatar");
        SetSkinColor(0);
        //avatarSystem.SetRandomAvatar(_avatar);
        ShowSexPanel();
    }


    private void SetAvatar()
    {
        avatarSystem.SetAvatar(
            _avatar,
            null, //hair
            0,    //upper
            0,    //lower
            0,    //feet
            null, //facial hair
            null, //earring
            null, //mask
            null, //hat
            1,    //eyes
            1,    //mouth
            1,    //eyebrows
            0,    //skin color
            0     //hair color
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
        avatarSystem.SetHairColor(colorIndex, _avatar);
    }


    public void SetEyesColor(int colorIndex)
    {
        _eyesColor = colorIndex;
        avatarSystem.SetEyesColor(colorIndex, _avatar);
    }


    public void SetBodyMesh(bool isMale)
    {
        _avatar = avatarSystem.SetBaseMesh(isMale, _avatar);

        for (int i = 0; i < _currentStyle.Length; i++)
        {
            if (_currentStyle[i] != -1)
                avatarSystem.SetBodyPart(i, _currentStyle[i], _avatar);
        }
        avatarSystem.SetHairColor(_hairColor, _avatar);
        avatarSystem.SetEyesColor(_eyesColor, _avatar);
        avatarSystem.SetSkinColor(_skinColor, _avatar);

        _isMale = isMale;
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
            button.onClick.AddListener(delegate () { SetBodyPart(bodyPart, index); });
            button.onClick.AddListener(delegate () { SetButtonSelected(button.gameObject, _thumbnailsButtons); });
            _thumbnailsButtons.Add(button);
            if (_currentStyle[bodyPart] == i)
                InstantiateSelectedButton(button.transform, true);
            else
                InstantiateSelectedButton(button.transform);
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
            avatarSystem.SetHairColor(_hairColor, _avatar);

        _currentStyle[bodyPart] = index;
    }

    public void SetHairColor()
    {
    }

    private void DestroyThumbnailsButtons()
    {
        foreach (var item in _thumbnailsButtons)
        {
            Destroy(item.gameObject);
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

    public enum TypePanel { Buttons, NulleableButtons, ButtonsWithColor };
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
    public HorizontalLayoutGroup selectionLayout;
    public Sprite selectedImage;
    private const int _EYES_BUTTON = 0;
    private const int _MOUTH_BUTTON = 1;

    public void ShowPanel(TypePanel typePanel, Panel panel, Section section, SetColorPart setColorMethod = null, bool destroyThumbnails = true, int bodyPart = 0)
    {
        if(destroyThumbnails) DestroyThumbnailsButtons();
        DestroyColorButtons();

        panels[(int)panel].SetActive(true);

        if (typePanel == TypePanel.NulleableButtons)
        {
            SetContainerWithButton(false);
        }
        else if (typePanel == TypePanel.ButtonsWithColor)
        {
            var button = Instantiate(thumbnailButtonPrefab, panels[(int)panel].transform /*.Find(_layoutName)*/);
            button.image.sprite = colorSprite;
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);
            _thumbnailsButtons.Add(button);
            button.onClick.AddListener(delegate () { setColorMethod(); });

            _buttonWidth = button.GetComponent<RectTransform>().sizeDelta.x;

            var cancelButton = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)panel].transform);
            cancelButton.image.sprite = cancelSprite;
            _thumbnailsButtons.Add(cancelButton);
            cancelButton.onClick.AddListener(delegate () { InitializeBodyPart((int)section); });
            cancelButton.onClick.AddListener(delegate () { SetButtonSelected(cancelButton.gameObject, _thumbnailsButtons); });

            if (_currentStyle[bodyPart] == -1)
                InstantiateSelectedButton(cancelButton.transform, true);
            else
                InstantiateSelectedButton(cancelButton.transform);

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

                case Section.Sex:
                    SetSexPanel();
                    break;

                case Section.EyesColor:
                    SetEyesColorPanel();
                    break;

                case Section.SkinColor:
                    SetSkinColorPanel();
                    break;
            }
        }

    }

    private void SetContainerWithButton(bool active)
    {
        if(active)
        {
            selectionButtonsContainer.offsetMin = new Vector2(_buttonWidth, selectionButtonsContainer.offsetMin.y);
            selectionLayout.padding = new RectOffset((int)_buttonWidth + (int)selectionLayout.spacing, 0, 0, 0);
        }
        else
        {
            selectionButtonsContainer.offsetMin = new Vector2(0, selectionButtonsContainer.offsetMin.y);
            selectionLayout.padding = new RectOffset((int)selectionLayout.spacing, (int)selectionLayout.spacing, 0, 0);
        }
    }

    private void SetHeadPanel()
    {
        foreach (var headButton in headButtons)
        {
            headButton.gameObject.SetActive(true);
        }

        headButtons[_EYES_BUTTON].interactable = false;
        headButtons[_MOUTH_BUTTON].interactable = true;
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
        ShowPanel(TypePanel.Buttons, Panel.Subselection, Section.Eyes);
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowEyesPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Subselection, Section.Eyes);
        
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowMouthPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Subselection, Section.Mouth);
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowHairPanel()
    {
        ShowPanel(TypePanel.ButtonsWithColor, Panel.Subselection, Section.Hair, ShowHairColorsPanel, true, AvatarSystem.HAIR);
    }

    public void ShowHairColorsPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Colors, Section.HairColor, null, false);
    }

    public void ShowFacialHairPanel()
    {
        ShowPanel(TypePanel.ButtonsWithColor, Panel.Subselection, Section.FacialHair, ShowHairColorsPanel, true, AvatarSystem.FACIAL_HAIR);
    }

    public void ShowSexPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.Sex);
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowEyesColorPanel()
    {
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.EyesColor);
        SetPivotCenter(selectionLayoutRect);
    }

    public void ShowSkinColorPanel()
    {
        HidePanels();
        ShowPanel(TypePanel.Buttons, Panel.Selection, Section.SkinColor);
        SetPivotCenter(selectionLayoutRect);
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
    }

    private void SetHairColorPanel()
    {
        var colors = avatarSystem.hairColors;

        for (int i = 0; i < colors.Length; i++)
        {
            var button = Instantiate(colorButtonPrefab, panelsLayouts[(int)Panel.Colors].transform);
            var index = i;
            button.image.sprite = colorButtonSprite;
            button.onClick.AddListener(delegate () { SetHairColor(index); });
            button.onClick.AddListener(delegate () { SetColorButtonSelected(button); });

            var buttonInside = button.transform.Find(_colorButtonInsideName).gameObject;
            buttonInside.GetComponent<Image>().color = colors[i];

            _colorButtons.Add(button);

            if (_hairColor == i)
            {
                button.image.color = new Color(0, 1, 1);
            }
            else
            {
                button.image.color = colors[i];
            }
        }
        SetContainerWithButton(true);
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

        var buttonFemale = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)Panel.Selection].transform);
        buttonFemale.image.sprite = femaleButtonSprite;
        buttonFemale.onClick.AddListener(delegate () { SetBodyMesh(false); });
        buttonFemale.onClick.AddListener(delegate () { SetButtonSelected(buttonFemale.gameObject, _thumbnailsButtons); });

        _thumbnailsButtons.Add(buttonMale);
        _thumbnailsButtons.Add(buttonFemale);


        InstantiateSelectedButton(buttonMale.transform);
        InstantiateSelectedButton(buttonFemale.transform);
    }

    public void SetEyesColorPanel()
    {
        var colors = avatarSystem.eyesColors;
        for (int i = 0; i < colors.Length; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)Panel.Selection].transform);
            button.image.color = colors[i];
            var index = i;
            button.onClick.AddListener(delegate () { SetEyesColor(index); });
            button.onClick.AddListener(delegate () { SetButtonSelected(button.gameObject, _thumbnailsButtons); });
            _thumbnailsButtons.Add(button);

            if (_eyesColor == i)
                InstantiateSelectedButton(button.transform, true);
            else
                InstantiateSelectedButton(button.transform);
        }
    }

    public void SetSkinColorPanel()
    {
        var materials = avatarSystem.skinMaterials;

        for (int i = 0; i < materials.Length; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, panelsLayouts[(int)Panel.Selection].transform);
            button.image.color = materials[i].color;
            var index = i;
            button.onClick.AddListener(delegate () { SetSkinColor(index); });
            button.onClick.AddListener(delegate () { SetButtonSelected(button.gameObject, _thumbnailsButtons); });
            _thumbnailsButtons.Add(button);

            if (_skinColor == i)
                InstantiateSelectedButton(button.transform, true);
            else
                InstantiateSelectedButton(button.transform);
        }
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
    public void SetColorButtonSelected(Button button)
    {
        foreach (var b in _colorButtons)
        {
            var buttonInside = b.transform.Find(_colorButtonInsideName).gameObject;
            var color = buttonInside.GetComponent<Image>().color;
            b.image.color = color;
        }
        button.image.color = colorButtonSelectedColor;

    }

    private string _selectedButtonName = "Selected";
    private void InstantiateSelectedButton(Transform parent, bool active = false)
    {
        var selected = Instantiate(selectedButtonPrefab, parent);
        selected.name = _selectedButtonName;
        selected.SetActive(active);
    }

}
