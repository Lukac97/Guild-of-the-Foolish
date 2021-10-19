using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildProfessions : MonoBehaviour
{
    private static GuildProfessions _instance;
    public static GuildProfessions Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]private List<ArtisanProfessionMould> TESTStartingProfessions;

    public List<ArtisanProfession> artisanProfessions = new List<ArtisanProfession>();

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        foreach (ArtisanProfessionMould apMould in TESTStartingProfessions)
        {
            artisanProfessions.Add(new ArtisanProfession(apMould));
        }
    }
}
