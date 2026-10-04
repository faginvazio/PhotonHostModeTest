using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private const float BallSpeed = 10f;
    private const float ShootDelay = 0.5f;
    private const float MoveSpeed = 5f;
    private NetworkCharacterController _cc;

    [SerializeField] private Ball _prefabBall;
    [SerializeField] private PhysxBall _prefabPhysxBall;
    private Vector3 _forward = Vector3.forward;
    

    [Networked]
    private TickTimer _delay {  get; set; }

    

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.Direction.Normalize();
            _cc.Move(MoveSpeed * data.Direction * Runner.DeltaTime);

            if (data.Direction.sqrMagnitude > 0)
            {
                _forward = data.Direction;
            }

            
            if (HasStateAuthority && _delay.ExpiredOrNotRunning(Runner))
            {
                if (data.Buttons.IsSet(NetworkInputData.MouseButton0))
                {
                    _delay = TickTimer.CreateFromSeconds(Runner, ShootDelay);
                    Runner.Spawn(
                        _prefabBall,
                        transform.position + _forward,
                        Quaternion.LookRotation(_forward),
                        Object.InputAuthority,
                        (runner, o) => { o.GetComponent<Ball>().Init(); }
                    );
                } 
                else if (data.Buttons.IsSet(NetworkInputData.MouseButton1))
                {
                    _delay = TickTimer.CreateFromSeconds(Runner, ShootDelay);
                    Runner.Spawn(
                        _prefabPhysxBall,
                        transform.position + _forward,
                        Quaternion.LookRotation(_forward),
                        Object.InputAuthority,
                        (runner, o) => { o.GetComponent<PhysxBall>().Init(BallSpeed * _forward); }
                    );

                }
            }
        }
    }
}
