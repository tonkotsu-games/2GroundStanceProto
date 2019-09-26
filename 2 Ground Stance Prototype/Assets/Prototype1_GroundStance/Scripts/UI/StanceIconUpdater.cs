using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanceIconUpdater : MonoBehaviour
{
    [SerializeField] Image iconDisplay;

    [SerializeField] List<Sprite> agilityIcons;
    [SerializeField] List<Sprite> aggroIcons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovement.currentStance == PlayerMovement.Stances.Agility)
        {
            iconDisplay.sprite = agilityIcons[PlayerMovement.stanceChargeLevel];
        }
        else if(PlayerMovement.currentStance == PlayerMovement.Stances.Aggro)
        {
            iconDisplay.sprite = aggroIcons[PlayerMovement.stanceChargeLevel];
        }
    }
}
