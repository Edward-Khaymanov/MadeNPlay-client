using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class ServersContainer : MonoBehaviour
{
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private ServersNotFoundView _serversNotFoundView;
    [SerializeField] private ServerView _serverViewTemplate;

    private readonly List<ServerInfo> _servers = new List<ServerInfo>();
    private ServerView _selectedServerView;
    private ColorTheme _currentTheme;

    public event Action<ServerInfo> ServerSelected;

    public ServerInfo SelectedServer => _servers.FirstOrDefault(x => x.View == _selectedServerView);
    public IReadOnlyList<ServerInfo> Servers => _servers;

    private void Start()
    {
        _currentTheme = ThemeManager.CurrentTheme;
    }

    public ServerInfo AddServer(NetworkLobbyData networkLobbyData, IPEndPoint endPoint)
    {
        var view = CreateServerView();
        var server = new ServerInfo()
        {
            Address = endPoint.Address,
            Data = networkLobbyData,
            View = view
        };

        _servers.Add(server);

        StartCoroutine(CheckPingCoroutine(endPoint.Address.MapToIPv4().ToString(), (ping) =>
        {
            view.Render(server.Data, ping);
            view.gameObject.SetActive(true);
        }));

        return server;
    }

    public void Clear()
    {
        _selectedServerView = null;

        foreach (var serverInfo in _servers)
        {
            serverInfo.View.Selecting -= OnViewSelecting;
            GameObject.Destroy(serverInfo.View.gameObject);
        }

        _serversNotFoundView.gameObject.SetActive(false);
        _servers.Clear();
    }

    public void ShowNotFoundMessage()
    {
        _serversNotFoundView.gameObject.SetActive(true);
    }

    private ServerView CreateServerView()
    {
        var serverView = Instantiate(_serverViewTemplate, _contentContainer);
        serverView.Init(
            _currentTheme.JoinMenuTheme.ServersLineDeselected,
            _currentTheme.JoinMenuTheme.ServersLineSelected,
            _currentTheme.JoinMenuTheme.ServersLineHovered);

        serverView.Selecting += OnViewSelecting;
        serverView.gameObject.SetActive(false);
        return serverView;
    }

    private IEnumerator CheckPingCoroutine(string ip, Action<int> callback)
    {
        var ping = new Ping(ip);
        while (ping.isDone == false)
        {
            yield return null;
        }
        var time = ping.time;
        ping.DestroyPing();
        callback(time);
    }

    private void OnViewSelecting(ServerView view)
    {
        if (view == null)
            return;

        if (_selectedServerView == view)
            return;

        if (_selectedServerView != null)
            _selectedServerView.Deselect();

        _selectedServerView = view;
        _selectedServerView.Select();

        ServerSelected?.Invoke(SelectedServer);
    }
}