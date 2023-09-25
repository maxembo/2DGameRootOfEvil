using UnityEngine;
using InputData;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    #region Vars

    [FormerlySerializedAs("_playerAnim")]
    [Header("Components:")] 
    [SerializeField] private Animator _anim;
    [SerializeField] private PlayerAudioHandler _audioHandler;

    private GameModes _gameMode;

    private Warrior _player;
    private TurnHandler _turnHandler;
    private ItemHandler _itemHandler;

    private float _verticalAxis = 0;
    private float _horizontalAxis = 0;

    #endregion

    #region Base Methods

    private void Start()
    {
        _player = GetComponent<Warrior>();
        _turnHandler = GetComponent<TurnHandler>();
        _itemHandler = GetComponent<ItemHandler>();
        
        EventHandler.OnGameModeChanged.AddListener(ChangeGameMode);
    }

    private void Update()
    {
        if (_gameMode != GameModes.Playing)
            return;

        SetAxes();

        ItemPickOrPut();

        UseItem_Primary();
        UseItem_Secondary();

        _anim.SetFloat("SpeedHorizontal", Mathf.Abs(GetMovementVector().x));
        _anim.SetFloat("SpeedUp", GetMovementVector().y);
        _anim.SetFloat("SpeedDown", -GetMovementVector().y);
    }

    private void FixedUpdate()
    {
        if (_gameMode != GameModes.Playing)
            return;

        if (IsPlayerMoving())
        {
            _player.MoveTo(GetMovementVector());

            PlayerSideChanger();

            _audioHandler.TakeStep();
        }
        else
        {
            _player.StopMove();

            _audioHandler.StopPlaying();
        }
    }

    #endregion

    #region Other

    private void ChangeGameMode(GameModes mode) => _gameMode = mode;
    
    private bool IsPlayerMoving() => _verticalAxis != 0 || _horizontalAxis != 0;

    #endregion

    #region Items

    private void PickUpItem()
    {
        _itemHandler.PickUpItem();

        SetPlayerSide(GetLastPlayerSide());
    }

    private void PutItem() => _itemHandler.PutItem();

    #endregion

    #region Rotation

    private TurnHandler.playerSides GetLastPlayerSide() => _turnHandler.currentSide;

    private void PlayerSideChanger()
    {
        if (_verticalAxis < 0)
            SetPlayerSide(TurnHandler.playerSides.Front);

        if (_verticalAxis > 0)
            SetPlayerSide(TurnHandler.playerSides.Back);

        if (_horizontalAxis < 0)
            SetPlayerSide(TurnHandler.playerSides.Left);

        if (_horizontalAxis > 0)
            SetPlayerSide(TurnHandler.playerSides.Right);
    }

    private void SetPlayerSide(TurnHandler.playerSides side) => _turnHandler.SetPlayerSide(side);

    #endregion

    #region Inputs

    private void SetAxes()
    {
        _verticalAxis = InputFunctions.GetVerticalAxis();
        _horizontalAxis = InputFunctions.GetHorizontalAxis();
    }

    private void ItemPickOrPut()
    {
        if (InputFunctions.GetKeyE_Up())
        {
            PutItem();

            PickUpItem();
        }
    }

    private void UseItem_Primary()
    {
        if (InputFunctions.GetLMB_Up())
            _player.UsePrimaryAction();
    }

    private void UseItem_Secondary()
    {
        if (InputFunctions.GetRMB_Up())
            _player.UseSecondaryAction();
    }

    private Vector2 GetMovementVector()
    {
        var v = _player.transform.up * _verticalAxis;
        var h = _player.transform.right * _horizontalAxis;

        Vector2 vector = h + v;

        return vector;
    }

    #endregion
}