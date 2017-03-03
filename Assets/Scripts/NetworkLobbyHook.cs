using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NetworkedPlayerScript player = gamePlayer.GetComponent<NetworkedPlayerScript>();

        player.playerName = lobby.playerName;
        player.playerColour = lobby.playerColor;
    }
}
