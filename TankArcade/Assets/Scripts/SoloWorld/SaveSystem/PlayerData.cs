[System.Serializable]
public class PlayerData
{
    public int Currentindex;
    public int maxIndex;
    public double[] times;


    public PlayerData(CharacterManager player)
    {
        Currentindex = player.CurrentIndex;
        maxIndex = player.maxIndex;
        times = new double[maxIndex + 1];
        for (int i = 0; i <= maxIndex; i++)
            times[i] = player.times[i];
    }
}