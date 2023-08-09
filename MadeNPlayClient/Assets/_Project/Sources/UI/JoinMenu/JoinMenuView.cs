using UnityEngine;
using UnityEngine.UI;

public class JoinMenuView : MonoBehaviour, IScreen
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _refreshButton;
    [SerializeField] private Button _filterButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private ServerInfoView _serverInfoView;
    [SerializeField] private ServersContainer _serversContainer;

    private JoinMenuPresenter _presenter;

    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        _presenter = new JoinMenuPresenter(canvas, _serverInfoView, _serversContainer, _joinButton);
    }

    private void OnEnable()
    {
        _joinButton.onClick.AddListener(_presenter.Connect);
        _refreshButton.onClick.AddListener(_presenter.Refresh);
    }

    private void OnDisable()
    {
        _joinButton.onClick.RemoveListener(_presenter.Connect);
        _refreshButton.onClick.RemoveListener(_presenter.Refresh);
    }

    public void Show()
    {
        _presenter.Show();
    }

    public void Hide()
    {
        _presenter.Hide();
    }

    public void OnRegisterScreenListeners()
    {
        _backButton.onClick.AddListener(ScreenSwitch.Instance.ShowMainMenu);
    }

    public void OnRemoveScreenListeners()
    {
        _backButton.onClick.RemoveListener(ScreenSwitch.Instance.ShowMainMenu);
    }
}