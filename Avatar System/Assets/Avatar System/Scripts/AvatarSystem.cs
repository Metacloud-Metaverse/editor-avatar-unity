using System.Collections.Generic;
using UnityEngine;
using GLTFast;

public class AvatarSystem : MonoBehaviour
{
    public GameObject[] hairMeshes;
    public Texture2D[] hairThumbnails;

    public GameObject[] upperMeshes;
    public Texture2D[] upperThumbnails;

    public GameObject[] lowerMeshes;
    public Texture2D[] lowerThumbnails;

    public GameObject[] feetMeshes;
    public Texture2D[] feetThumbnails;

    public GameObject[] facialHairMeshes;
    public Texture2D[] facialHairThumbnails;

    public GameObject[] earringMeshes;
    public Texture2D[] earringThumbnails;

    public GameObject[] maskMeshes;
    public Texture2D[] maskThumbnails;

    public GameObject[] hatMeshes;
    public Texture2D[] hatThumbnails;

    public Texture[] eyebrowsTextures;
    public Texture[] eyesTextures;
    public Texture[] eyesMasks;
    public Texture[] mouthTextures;

    public Color[] eyesColors;

    public GameObject baseMale;
    public GameObject baseFemale;


    public Material[] skinMaterials;
    public Material[] hairMaterials;
    private string _rootBoneName = "Bip001";
    public int bodyPartsCount { get { return _bodyPartsCount; } }

    private BodyPart[] _bodyParts;
    private int _bodyPartsCount = 12;
    private string _skinMaterialName = "Character Male";//"AvatarSkin_MAT";
    private string _hairMaterialName = "11 Hair";
    private string _instanceMaterialName = " (Instance)";
    private string[] _materialNames;

    private Material _skinMaterial;
    private Material _hairMaterial;


    public const int HAIR = 0;
    public const int UPPER = 1;
    public const int LOWER = 2;
    public const int FEET = 3;
    public const int FACIAL_HAIR = 4;
    public const int EARRING = 5;
    public const int MASK = 6;
    public const int HAT = 7;
    public const int HEAD = 8;
    public const int EYEBROWS = 9;
    public const int EYES = 10;
    public const int MOUTH = 11;

    public enum MaterialType { SKIN = 0, HAIR = 1 }


    private void Start()
    {
        SetMaterialsNames();
        SetBodyParts();
    }


    private void SetMaterialsNames()
    {
        _materialNames = new string[2];
        _materialNames[0] = _skinMaterialName;
        _materialNames[1] = _hairMaterialName;
    }


    private void SetAllMaterials(MaterialType materialType, GameObject baseMesh)
    {
        foreach (var bodyPart in _bodyParts)
        {
            var partTransform = baseMesh.transform.Find(bodyPart.name);

            if (partTransform != null)
            {
                var rend = partTransform.gameObject.GetComponent<Renderer>();
                if (rend == null) rend = partTransform.gameObject.GetComponentInChildren<Renderer>();

                SetMaterial(rend, materialType);
            }
        }
    }


    private void SetHairMaterial(GameObject baseMesh)
    {
        var hairTransform = baseMesh.transform.Find(BodyPart.HAIR_NAME);
        var facialHairTransform = baseMesh.transform.Find(BodyPart.FACIAL_HAIR_NAME);

        var renderers = new Renderer[2];
        if (hairTransform != null)
            renderers[0] = hairTransform.gameObject.GetComponentInChildren<Renderer>();

        if (facialHairTransform != null)
            renderers[1] = facialHairTransform.gameObject.GetComponentInChildren<Renderer>();


        foreach (var renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.material = _hairMaterial;
            renderer.material.name = _hairMaterialName;
        }
    }


    public void SetEyesColor(int colorIndex, GameObject baseMesh)
    {
        var eyes = baseMesh.transform.Find(BodyPart.EYES_NAME).gameObject;

        eyes.GetComponent<Renderer>().material.color = eyesColors[colorIndex];
    }


    private void SetMaterial(Renderer renderer, MaterialType materialType) //0 = skin, 1 = hair
    {
        var materials = new Material[renderer.materials.Length];
        materials = renderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name == _materialNames[(int)materialType] || materials[i].name == _materialNames[(int)materialType] + _instanceMaterialName)
            {
                switch (materialType)
                {
                    case MaterialType.SKIN:
                        materials[i] = _skinMaterial;
                        break;

                    case MaterialType.HAIR:
                        materials[i] = _hairMaterial;
                        break;
                }
                materials[i].name = _materialNames[(int)materialType];
            }
        }

        renderer.materials = materials; //No se puede setear el material por el indice     
    }


    public void SetSkinColor(int colorIndex, GameObject baseMesh)
    {
        _skinMaterial = skinMaterials[colorIndex];
        SetAllMaterials(MaterialType.SKIN, baseMesh);
    }


    public void SetHairColor(int colorIndex, GameObject baseMesh)
    {
        _hairMaterial = hairMaterials[colorIndex];
        SetHairMaterial(baseMesh);
        SetEyebrowsColor(_hairMaterial, baseMesh);
    }


    public void SetEyebrowsColor(Material referenceMaterial, GameObject baseMesh)
    {
        var bodyPart = baseMesh.transform.Find(BodyPart.EYEBROWS_NAME).gameObject;

        bodyPart.GetComponent<Renderer>().material.color = referenceMaterial.color;
    }


    private void SetBodyParts()
    {
        _bodyParts = new BodyPart[_bodyPartsCount];

        _bodyParts[HAIR] = new BodyPart();
        _bodyParts[HAIR].name = BodyPart.HAIR_NAME;
        _bodyParts[HAIR].parentName = BodyPart.HEAD_NAME;
        _bodyParts[HAIR].meshes = hairMeshes;
        _bodyParts[HAIR].thumbnails = hairThumbnails;
        _bodyParts[HAIR].bodyPartType = BodyPart.BodyPartType.mesh;
        _bodyParts[HAIR].isRemoveable = true;

        _bodyParts[UPPER] = new BodyPart();
        _bodyParts[UPPER].name = BodyPart.UPPER_NAME;
        _bodyParts[UPPER].parentName = BodyPart.AVATAR_NAME;
        _bodyParts[UPPER].meshes = upperMeshes;
        _bodyParts[UPPER].thumbnails = upperThumbnails;
        _bodyParts[UPPER].bodyPartType = BodyPart.BodyPartType.mesh;

        _bodyParts[LOWER] = new BodyPart();
        _bodyParts[LOWER].name = BodyPart.LOWER_NAME;
        _bodyParts[LOWER].parentName = BodyPart.AVATAR_NAME;
        _bodyParts[LOWER].meshes = lowerMeshes;
        _bodyParts[LOWER].thumbnails = lowerThumbnails;
        _bodyParts[LOWER].bodyPartType = BodyPart.BodyPartType.mesh;

        _bodyParts[FEET] = new BodyPart();
        _bodyParts[FEET].name = BodyPart.FEET_NAME;
        _bodyParts[FEET].parentName = BodyPart.AVATAR_NAME;
        _bodyParts[FEET].meshes = feetMeshes;
        _bodyParts[FEET].thumbnails = feetThumbnails;
        _bodyParts[FEET].bodyPartType = BodyPart.BodyPartType.mesh;

        _bodyParts[FACIAL_HAIR] = new BodyPart();
        _bodyParts[FACIAL_HAIR].name = BodyPart.FACIAL_HAIR_NAME;
        _bodyParts[FACIAL_HAIR].parentName = BodyPart.HEAD_NAME;
        _bodyParts[FACIAL_HAIR].meshes = facialHairMeshes;
        _bodyParts[FACIAL_HAIR].thumbnails = facialHairThumbnails;
        _bodyParts[FACIAL_HAIR].bodyPartType = BodyPart.BodyPartType.mesh;
        _bodyParts[FACIAL_HAIR].isRemoveable = true;

        _bodyParts[EARRING] = new BodyPart();
        _bodyParts[EARRING].name = BodyPart.EARRING_NAME;
        _bodyParts[EARRING].parentName = BodyPart.HEAD_NAME;
        _bodyParts[EARRING].meshes = earringMeshes;
        _bodyParts[EARRING].thumbnails = earringThumbnails;
        _bodyParts[EARRING].bodyPartType = BodyPart.BodyPartType.mesh;
        _bodyParts[EARRING].isRemoveable = true;

        _bodyParts[MASK] = new BodyPart();
        _bodyParts[MASK].name = BodyPart.MASK_NAME;
        _bodyParts[MASK].parentName = BodyPart.HEAD_NAME;
        _bodyParts[MASK].meshes = maskMeshes;
        _bodyParts[MASK].thumbnails = maskThumbnails;
        _bodyParts[MASK].bodyPartType = BodyPart.BodyPartType.mesh;
        _bodyParts[MASK].isRemoveable = true;

        _bodyParts[HAT] = new BodyPart();
        _bodyParts[HAT].name = BodyPart.HAT_NAME;
        _bodyParts[HAT].parentName = BodyPart.HEAD_NAME;
        _bodyParts[HAT].meshes = hatMeshes;
        _bodyParts[HAT].thumbnails = hatThumbnails;
        _bodyParts[HAT].bodyPartType = BodyPart.BodyPartType.mesh;
        _bodyParts[HAT].isRemoveable = true;

        _bodyParts[HEAD] = new BodyPart();
        _bodyParts[HEAD].name = BodyPart.HEAD_NAME;
        _bodyParts[HEAD].parentName = BodyPart.AVATAR_NAME;
        _bodyParts[HEAD].bodyPartType = BodyPart.BodyPartType.mesh;
        //_bodyParts[HEAD].meshes = headMeshes;

        _bodyParts[EYEBROWS] = new BodyPart();
        _bodyParts[EYEBROWS].name = BodyPart.EYEBROWS_NAME;
        _bodyParts[EYEBROWS].parentName = BodyPart.HEAD_NAME;
        _bodyParts[EYEBROWS].textures = eyebrowsTextures;
        _bodyParts[EYEBROWS].bodyPartType = BodyPart.BodyPartType.texture;

        _bodyParts[EYES] = new BodyPart();
        _bodyParts[EYES].name = BodyPart.EYES_NAME;
        _bodyParts[EYES].parentName = BodyPart.HEAD_NAME;
        _bodyParts[EYES].textures = eyesTextures;
        _bodyParts[EYES].bodyPartType = BodyPart.BodyPartType.texture;

        _bodyParts[MOUTH] = new BodyPart();
        _bodyParts[MOUTH].name = BodyPart.MOUTH_NAME;
        _bodyParts[MOUTH].parentName = BodyPart.HEAD_NAME;
        _bodyParts[MOUTH].textures = mouthTextures;
        _bodyParts[MOUTH].bodyPartType = BodyPart.BodyPartType.texture;
    }


    public GameObject SetBaseMesh(bool isMale, GameObject avatar)
    {
        Transform avatarTransform = avatar.transform;
        Destroy(avatar);

        GameObject newAvatar;

        if (isMale)
            newAvatar = Instantiate(baseMale);
        else
            newAvatar = Instantiate(baseFemale);

        newAvatar.transform.position = avatarTransform.position;
        newAvatar.transform.rotation = avatarTransform.rotation;

        SetBones(newAvatar);

        return newAvatar;
    }


    public Transform[] SetBones(GameObject avatar)
    {
        Transform[] bones;

        //if (hasBonesContainer)
        //    bones = avatar.transform.Find(bonesContainerName).Find(_rootBoneName).GetComponentsInChildren<Transform>();
        //else
        bones = avatar.transform.Find(_rootBoneName).GetComponentsInChildren<Transform>();

        return bones;
    }


    public void SetBodyPart(int bodyPartIndex, int index, GameObject baseMesh)
    {
        if (_bodyParts[bodyPartIndex].bodyPartType == BodyPart.BodyPartType.mesh)
            SetBodyPartMesh(bodyPartIndex, index, baseMesh);

        else if (_bodyParts[bodyPartIndex].bodyPartType == BodyPart.BodyPartType.texture)
            SetBodyPartTexture(bodyPartIndex, index, baseMesh);
    }


    public void SetBodyPart(int bodyPartIndex, string url, GameObject baseMesh)
    {
        if (_bodyParts[bodyPartIndex].bodyPartType == BodyPart.BodyPartType.mesh)
            SetBodyPartMesh(bodyPartIndex, 0, baseMesh, url);

        else if (_bodyParts[bodyPartIndex].bodyPartType == BodyPart.BodyPartType.texture)
            SetBodyPartTexture(bodyPartIndex, 0, baseMesh, url);

    }


    private void SetBodyPartMesh(int bodyPartIndex, int index, GameObject baseMesh, string url = null)
    {
        var bodyPart = _bodyParts[bodyPartIndex];

        var part = baseMesh.transform.Find(bodyPart.name);

        if (part != null) Destroy(part.gameObject);

        var parent = baseMesh.transform;

        GameObject newPart;

        if (string.IsNullOrEmpty(url))
        {
            newPart = Instantiate(bodyPart.meshes[index], parent);
            InitializeNewPart(newPart, bodyPart, parent);
        }
        else
        {
            DownloadAndInstantiateBodyPart(url, bodyPart, parent);
        }
    }


    public async void DownloadAndInstantiateBodyPart(string url, BodyPart bodyPart, Transform parent)
    {
        var gltf = new GltfImport();
        bool success = await gltf.Load(url);

        if (success)
        {
            var go = new GameObject();
            gltf.InstantiateMainScene(go.transform);
            var instance = go.transform.Find("Scene");
            instance.parent = null;
            Destroy(go);
            InitializeNewPart(instance.gameObject, bodyPart, parent);
        }
        else
            Debug.LogException(new System.Exception($"Download failed. URL: {url}"));
    }


    private void InitializeNewPart(GameObject newPart, BodyPart bodyPart, Transform parent)
    {
        newPart.transform.localPosition = Vector3.zero;
        newPart.transform.name = bodyPart.name;

        if (parent != null) newPart.transform.parent = parent;

        var renderers = newPart.GetComponentsInChildren<SkinnedMeshRenderer>();
        var bones = SetBones(parent.gameObject);

        foreach (var renderer in renderers)
        {
            if (bodyPart.name == BodyPart.HAIR_NAME || bodyPart.name == BodyPart.FACIAL_HAIR_NAME)
                SetMaterial(renderer, MaterialType.HAIR);

            else if (bodyPart.name == BodyPart.UPPER_NAME || bodyPart.name == BodyPart.LOWER_NAME || bodyPart.name == BodyPart.FEET_NAME)
                SetMaterial(renderer, MaterialType.SKIN);

            renderer.rootBone = bones[0];

            Transform[] tmpBones = renderer.bones;
            for (int i = 0; i < tmpBones.Length; i++)
            {
                for (int j = 0; j < bones.Length; j++)
                {
                    if (tmpBones[i].name == bones[j].name)
                    {
                        tmpBones[i] = bones[j];
                    }
                }
            }
            renderer.bones = tmpBones;

        }
        Destroy(newPart.transform.Find(_rootBoneName).gameObject);

    }


    TextureDownloader _textureDownloader = new TextureDownloader();
    private string _eyesMaskTexName = "_IrisMask";
    private void SetBodyPartTexture(int bodyPartIndex, int index, GameObject baseMesh, string url)
    {
        var bodyPart = _bodyParts[bodyPartIndex];

        var part = baseMesh.transform.Find(bodyPart.name);

        var renderer = part.gameObject.GetComponentInChildren<Renderer>();

        if (string.IsNullOrEmpty(url))
        {
            renderer.material.mainTexture = bodyPart.textures[index];
            if (bodyPart.masks != null)
                renderer.material.SetTexture(_eyesMaskTexName, bodyPart.masks[index]);
        }
        else
        {
            StartCoroutine(_textureDownloader.GetTextureAndRender(url, renderer));
        }
    }


    private void SetBodyPartTexture(int bodyPartIndex, int index, GameObject baseMesh)
    {
        SetBodyPartTexture(bodyPartIndex, index, baseMesh, null);
    }


    public int GetBodyPartLength(int bodyPart)
    {
        if (_bodyParts[bodyPart].bodyPartType == BodyPart.BodyPartType.mesh)
            return _bodyParts[bodyPart].meshes.Length;

        else if (_bodyParts[bodyPart].bodyPartType == BodyPart.BodyPartType.texture)
            return _bodyParts[bodyPart].textures.Length;

        else
            throw new System.Exception("Body part has not body part type");
    }


    public void SetAvatar(GameObject avatar, int? hair, int upper, int lower, int feet, int? facialHair, int? earring, int? mask, int? hat, int eyes, int mouth, int eyebrows, int skinColor, int hairColor)
    {
        _skinMaterial = skinMaterials[skinColor];
        _hairMaterial = hairMaterials[hairColor];

        SetBodyPart(UPPER, upper, avatar);
        SetBodyPart(LOWER, lower, avatar);
        SetBodyPart(FEET, feet, avatar);

        SetNulleableBodyPart(HAIR, hair, avatar);
        SetNulleableBodyPart(FACIAL_HAIR, facialHair, avatar);
        SetNulleableBodyPart(EARRING, earring, avatar);
        SetNulleableBodyPart(MASK, mask, avatar);
        SetNulleableBodyPart(HAT, hat, avatar);

        SetBodyPart(EYES, eyes, avatar);
        SetBodyPart(EYEBROWS, eyebrows, avatar);
        SetBodyPart(MOUTH, mouth, avatar);

        SetSkinColor(skinColor, avatar);
        SetHairColor(hairColor, avatar);
    }


    public void SetRandomAvatar(GameObject avatar)
    {
        SetBones(avatar);

        var skinColor = Random.Range(0, skinMaterials.Length);
        var hairColor = Random.Range(0, hairMaterials.Length);

        _skinMaterial = skinMaterials[skinColor];
        _hairMaterial = hairMaterials[hairColor];
        SetBodyPart(UPPER, Random.Range(0, upperMeshes.Length), avatar);
        SetBodyPart(LOWER, Random.Range(0, lowerMeshes.Length), avatar);
        SetBodyPart(FEET, Random.Range(0, feetMeshes.Length), avatar);

        SetPartNulleableRandom(FACIAL_HAIR, facialHairMeshes, avatar);
        SetPartNulleableRandom(HAIR, hairMeshes, avatar);

        //SetNulleableBodyPart(EARRING, Random.Range(0, earringMeshes.Length), avatar);
        //SetNulleableBodyPart(MASK, Random.Range(0, maskMeshes.Length), avatar);
        //SetNulleableBodyPart(HAT, Random.Range(0, hatMeshes.Length), avatar);

        SetBodyPart(EYES, Random.Range(0, eyesTextures.Length), avatar);
        SetBodyPart(EYEBROWS, Random.Range(0, eyebrowsTextures.Length), avatar);
        SetBodyPart(MOUTH, Random.Range(0, mouthTextures.Length), avatar);

        SetSkinColor(skinColor, avatar);
        SetHairColor(hairColor, avatar);
        SetEyesColor(Random.Range(0, eyesColors.Length), avatar);
    }

    private void SetPartNulleableRandom(int part, GameObject[] meshes, GameObject avatar)
    {
        var rnd = Random.Range(-1, meshes.Length);
        if (rnd >= 0)
            SetNulleableBodyPart(part, rnd, avatar);
    }

    private void SetNulleableBodyPart(int bodyPart, int? index, GameObject avatar)
    {
        if (index == null) return;
        SetBodyPart(bodyPart, (int)index, avatar);
    }


    public void RemoveBodyPart(int bodyPartIndex, GameObject avatar)
    {
        var partTransform = avatar.transform.Find(_bodyParts[bodyPartIndex].name);

        if (partTransform != null)
            Destroy(partTransform.gameObject);
    }


    public bool IsBodyPartRemoveable(int index)
    {
        return _bodyParts[index].isRemoveable;
    }


    public List<WereableObject> GetBodyPartsBySex(int bodyPart, bool isMale)
    {
        var parts = new List<WereableObject>();

        if (_bodyParts[bodyPart].meshes != null)
        {
            for (int i = 0; i < _bodyParts[bodyPart].meshes.Length; i++)
            {
                CheckFileIsMale(isMale, parts, i, _bodyParts[bodyPart].meshes[i]);

            }
        }
        else if (_bodyParts[bodyPart].textures != null)
        {
            for (int i = 0; i < _bodyParts[bodyPart].textures.Length; i++)
            {
                CheckFileIsMale(isMale, parts, i, _bodyParts[bodyPart].textures[i]);

            }
        }

        return parts;
    }


    private void CheckFileIsMale(bool isMale, List<WereableObject> parts, int index, Object part)
    {
        var split = part.name.Split('_');
        if (isMale)
        {
            if (split[0] == "M")
            {
                var w = new WereableObject(WereableObject.Sex.male, part, index);
                parts.Add(w);
            }
        }
        else
        {
            if (split[0] == "F")
            {
                var w = new WereableObject(WereableObject.Sex.female, part, index);
                parts.Add(w);
            }
        }
        if (split[0] == "U")
        {
            var w = new WereableObject(WereableObject.Sex.unisex, part, index);
            parts.Add(w);
        }
    }


    public Texture2D GetThumbnail(int bodyPart, int index)
    {
        if (_bodyParts[bodyPart].thumbnails != null)
            return _bodyParts[bodyPart].thumbnails[index];
        else
            return (Texture2D)_bodyParts[bodyPart].textures[index];
    }


    public GameObject InstanceAvatar(Transform initialTransform, bool isMale, string avatarTag = null)
    {
        var baseMesh = (isMale) ? baseMale : baseFemale;
        var avatar = Instantiate(baseMesh, initialTransform.position, initialTransform.rotation);

        if (!string.IsNullOrEmpty(avatarTag)) avatar.tag = avatarTag;

        return avatar;
    }
}
