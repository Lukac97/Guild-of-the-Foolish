using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildProfessions : MonoBehaviour
{
    [SerializeField]private List<ArtisanProfessionMould> TESTStartingProfessions;

    public List<ArtisanProfession> artisanProfessions = new List<ArtisanProfession>();

    private void Start()
    {
        foreach(ArtisanProfessionMould apMould in TESTStartingProfessions)
        {
            artisanProfessions.Add(new ArtisanProfession(apMould));
        }
    }
}
