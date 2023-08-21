using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityRefresh : MonoBehaviour
{
   public List<Image> _abilityIcons;
   public List<Image> _imageToCopy;

    // Start is called before the first frame update
    void awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        for(int i=0; i <= _abilityIcons.Count -1; i++)
        {
            _abilityIcons[i].sprite = _imageToCopy[i].sprite;
        }
    }

}
