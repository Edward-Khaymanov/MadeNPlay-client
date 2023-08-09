using LobbySystem;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class JoinMenuPresenter
{
    private readonly Button _joinButton;
    private readonly Canvas _canvas;
    private readonly ServerDiscovery _serverDiscovery;
    private readonly ServerInfoView _serverInfoView;
    private readonly ServersContainer _serversContainer;

    private IEnumerator _findCoroutine;

    public JoinMenuPresenter(Canvas canvas, ServerInfoView serverInfoView, ServersContainer serversContainer, Button joinButton)
    {
        _canvas = canvas;
        _serverDiscovery = new ServerDiscovery(AppContext.Current.Enviroment.ApplicationKey);
        _serverInfoView = serverInfoView;
        _serversContainer = serversContainer;
        _joinButton = joinButton;
    }

    public void Show()
    {
        _serversContainer.ServerSelected += OnServerSelected;
        FindServers();
        _canvas.enabled = true;
    }

    public void Hide()
    {
        _canvas.enabled = false;
        _serversContainer.ServerSelected -= OnServerSelected;
        StopFindServers();
        Clear();
    }

    public void Refresh()
    {
        StopFindServers();
        Clear();
        FindServers();
    }

    public void Connect()
    {
        var selectedServer = _serversContainer.SelectedServer;
        if (selectedServer == null)
            return;

        var lobbyData = selectedServer.Data;
        new ConnectionManager().TryConnect(
            lobbyData.NetworkGameData.Id,
            lobbyData.NetworkGameData.Version,
            selectedServer.Address.MapToIPv4().ToString(),
            lobbyData.ConnectionPort
        );
        Hide();
    }

    private void Clear()
    {
        _serverInfoView.Clear();
        _serversContainer.Clear();
    }

    private void AddServer(NetworkLobbyData networkLobbyData, IPEndPoint endPoint)
    {
        StopFindCoroutine();
        _serversContainer.AddServer(networkLobbyData, endPoint);
    }

    private void FindServers()
    {
        _serverDiscovery.FindServers(AddServer);
        StartFindCorotine(TimeSpan.FromSeconds(5));
    }

    private void StopFindServers()
    {
        StopFindCoroutine();
        _serverDiscovery.StopFindServers();
    }

    private void StartFindCorotine(TimeSpan timeOut)
    {
        _findCoroutine = FindTimeOut(timeOut);
        Coroutiner.Instance.StartCoroutine(_findCoroutine);
    }

    private void StopFindCoroutine()
    {
        if (_findCoroutine == null)
            return;

        Coroutiner.Instance.StopCoroutine(_findCoroutine);
        _findCoroutine = null;
    }

    private IEnumerator FindTimeOut(TimeSpan timeOut)
    {
        yield return new WaitForSeconds(timeOut.Seconds);

        _serverDiscovery.StopFindServers();

        if (_serversContainer.Servers.Count == 0)
            _serversContainer.ShowNotFoundMessage();
    }

    private void OnServerSelected(ServerInfo serverInfo)
    {
        _joinButton.enabled = true;
        _serverInfoView.Render(serverInfo.Data);
    }
}