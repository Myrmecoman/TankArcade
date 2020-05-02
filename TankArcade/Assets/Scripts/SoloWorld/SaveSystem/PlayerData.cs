[System.Serializable]
public class PlayerData
{
    public int Currentindex;
    public int maxIndex;


    public PlayerData(CharacterManager player)
    {
        Currentindex = player.CurrentIndex;
        maxIndex = player.maxIndex;
    }
}