using Proto;

public static class NetFn
{
    public static void EnterGame(int characterId)
    {
        GameEnterRequest request = new()
        {
            CharacterId = characterId,
        };
        NetClient.Send(request);
    }
}