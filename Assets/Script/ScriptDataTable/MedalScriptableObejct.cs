using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Medal", order = 1)]
public class MedalScriptableObejct : BaseScriptableObject
{
    public enum MedalTag
    {
        Player ,
        Monster 
    }

    public MedalTag Tag;
    public string Name;
    public string Description;
    public float value;
    public Sprite Sprite;

    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        Name = values[2].Trim();
        Description = values[3].Trim();
        if (values[4].Trim() == "0")
        {
            Tag = MedalTag.Player;
        }else
        {
            Tag = MedalTag.Monster;
        }

        value = float.Parse(values[5].Trim());


        Sprite = Resources.Load<Sprite>("Sprite/Medal/" + id.ToString());




    }




}
