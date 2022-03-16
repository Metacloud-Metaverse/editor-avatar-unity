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
    public Transform spawnTransform;

    private int[] _currentStyle;
    private int _skinColor;
    private int _hairColor;
    private int _eyesColor;
    private int _bodyPartsCount;
    private int _thumbnailColumns = 4;
    private bool _isMale = true;
    //private int _tabSelected;
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


    //private void InitializeBodyPart(int bodyPart)
    //{
    //    InitializeBodyPart(bodyPart, 0);
    //}


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


    private void CreateThumbnailsButtons(int bodyPart, int columnsCount, bool isMale)
    {
        List<WereableObject> wereables;
        if (isMale)
            wereables = _maleBodyParts[bodyPart];
        else
            wereables = _femaleBodyParts[bodyPart];

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
            _thumbnailsButtons.Add(button);
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


    public void SetHairColorTab()
    {
        var materials = avatarSystem.hairMaterials;

        for (int i = 0; i < materials.Length; i++)
        {
            var button = Instantiate(thumbnailButtonPrefab, buttonsParent);
            button.image.color = materials[i].color;
            var index = i;
            button.onClick.AddListener(delegate () { SetHairColor(index); });
            _thumbnailsButtons.Add(button);
        }
    }


    public void ChangeTab(int bodyPart)
    {
        //_tabSelected = bodyPart;
        CreateThumbnailsButtons(bodyPart, _thumbnailColumns, _isMale);
    }


    public void SetBodyPart(int bodyPart, int index)
    {
        avatarSystem.SetBodyPart(bodyPart, index, _avatar);

        if (bodyPart == AvatarSystem.HAIR ||
            bodyPart == AvatarSystem.FACIAL_HAIR ||
            bodyPart == AvatarSystem.EYEBROWS)
            avatarSystem.SetHairColor(_hairColor, _avatar);
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
}
