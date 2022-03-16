using UnityEngine;

public class AvatarParser
{
    public AvatarSystem cc;

    public void ApplyConfigToMesh(string json, GameObject avatar)
    {
        var o = JsonUtility.FromJson<AvatarInfo> (json);

        SetBodyPart(AvatarSystem.HAIR, o.hair, avatar);
        SetBodyPart(AvatarSystem.UPPER, o.upper, avatar);
        SetBodyPart(AvatarSystem.LOWER, o.lower, avatar);
        SetBodyPart(AvatarSystem.FEET, o.feet, avatar);

        SetBodyPart(AvatarSystem.MASK, o.mask, avatar);
        SetBodyPart(AvatarSystem.HAT, o.hat, avatar);
        SetBodyPart(AvatarSystem.EARRING, o.earring, avatar);

        SetBodyPart(AvatarSystem.EYES, o.eyes, avatar);
        SetBodyPart(AvatarSystem.MOUTH, o.mouth, avatar);
        SetBodyPart(AvatarSystem.EYEBROWS, o.eyebrows, avatar);

        cc.SetSkinColor(o.skin.index, avatar);
        cc.SetEyesColor(o.eyesColor.index, avatar);
        cc.SetHairColor(o.hairColor.index, avatar);
    }

    private void SetBodyPart(int bodyPartIndex, WereableInfo info, GameObject avatar)
    {
        if (info == null) return;

        if(info.type == "default")
            cc.SetBodyPart(bodyPartIndex, info.index, avatar);

        else if(info.type == "custom")      
            cc.SetBodyPart(bodyPartIndex, info.url, avatar);
        
    }
}

[System.Serializable]
public class WereableInfo
{
    public int index;
    public string type;
    public string thumbnail;
    public string url;
}

[System.Serializable]
public class ColorInfo
{
    public int index;
}

[System.Serializable]
public class AvatarInfo
{
    public WereableInfo hair;
    public WereableInfo upper;
    public WereableInfo lower;
    public WereableInfo feet;
    public WereableInfo mask;
    public WereableInfo hat;
    public WereableInfo earring;
    public WereableInfo eyes;
    public WereableInfo mouth;
    public WereableInfo eyebrows;
    public ColorInfo skin;
    public ColorInfo eyesColor;
    public ColorInfo hairColor;

}
