using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateUI : MonoBehaviour
{
    private Slate slate;
    public Slate Slate { get { return slate; } set { slate = value; } }
    

    [SerializeField]
    private List<Magic> magics = new List<Magic>();

  



    public void SlateSet(Slate slate)
    { 
        this.slate = slate;
        this.magics = slate.Magics;
    }

    public void MagicDescSet(MagicUI magic)
    {
        
    }

    
}
