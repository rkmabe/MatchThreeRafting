public interface IInventoryManager
{
    public int GetNumCannonballs();

    public int GetNumCoins();
    public int GetNumCrab();
    public int GetNumFish();
    public int GetNumJunk();

    public void AdjustNumCannonballs(int num);

    public void AdjustNumCoins(int num);

    public void AdjustNumCrab(int num);

    public void AdjustNumFish(int num);

    public void AdjustNumJunk(int num);


    public int GetNumCatfish();
    public void AdjustNumCatfish(int num);

    public int GetNumLobster();
    public void AdjustNumLobster(int num);

    public int GetNumTrout();
    public void AdjustNumTrout(int num);

    public int GetNumTurtle();
    public void AdjustNumTurtle(int num);

    public int GetNumSnake();
    public void AdjustNumSnake(int num);

    //public int GetNumLifeVest();
    //public void AdjustNumLifeVest(int num);

    //public int GetNumPaddle();
    //public void AdjustNumPaddle(int num);

}
