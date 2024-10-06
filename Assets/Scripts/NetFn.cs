using Proto;

public static class NetFn
{
    public static void EnterGame(int characterId)
    {
        var request = new GameEnterRequest()
        {
            CharacterId = characterId,
        };
        NetClient.conn.Send(request);
    }
}