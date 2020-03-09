using UnityEngine;
public struct PlayerData
{
    public string Name;
    public int Health;
    public bool Visible;
    public override string ToString() => $"Name {Name} Health {Health} Visible{Visible}";
  

}
public class SinglePlayer : Unit
{

    private ISetData _data;

    void Start()
    {
        _data = new JSONData();
        Health = 100;
        PlayerData SinglePlayer = new PlayerData
        {
            Name = Name,
            Health = Health,
            Visible = IsVisible

        };
        _data.Save(SinglePlayer);
        PlayerData newPlayer = _data.Load();
        Debug.Log(newPlayer);

       //PlayerPrefs.SetString("Name", SinglePlayer.Name);
       // PlayerPrefs.SetInt("Health", SinglePlayer.Health);
       //PlayerPrefs.Save();
       // Debug.Log(PlayerPrefs.GetString("Name"));
       // PlayerPrefs.DeleteAll();
       // Debug.Log(PlayerPrefs.GetString("Name"));
    }

 
    void Update()
    {
        
    }
}
