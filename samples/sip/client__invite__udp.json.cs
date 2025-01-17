// generated by NiceStateMachineGenerator v1.0.0.0

using System;

using SampleApp.Sip;

namespace SampleApp.Sip.Generated
{
    public partial class client__invite__udp: IDisposable
    {
        
        public delegate void TimerFiredCallback(ITimer timer);
        
        public interface ITimer: IDisposable
        {
            void StartOrReset(double timerDelaySeconds);
            void Stop();
        }
        
        public interface ITimerFactory
        {
            ITimer CreateTimer(string timerName, TimerFiredCallback callback);
        }
        
        
        public enum States
        {
            Calling_Start,
            Calling_Retransmit,
            Proceeding,
            Completed,
            Terminated,
        }
        
        /*INVITE sent*/
        public event Action OnStateEnter__Calling_Start;
        /*INVITE sent*/
        public event Action OnStateEnter__Calling_Retransmit;
        /*The client transaction MUST be destroyed the instant it enters the 'Terminated' state*/
        public event Action OnStateEnter__Terminated;
        
        /*Furthermore, the provisional response MUST be passed to the TU*/
        public event Action<t_packet> OnEventTraverse__SIP_1xx; 
        /*and the response MUST be passed up to the TU*/
        public event Action<t_packet> OnEventTraverse__SIP_2xx; 
        /*The client transaction MUST pass the received response up to the TU, and the client transaction MUST generate an ACK request*/
        public event Action<t_packet> OnEventTraverse__SIP_300_699; 
        /*Inform TU*/
        public event Action OnEventTraverse__TransportError;
        /*the client transaction SHOULD inform the TU that a timeout has occurred.*/
        public event Action OnTimerTraverse__Timer_B;
        /*Any retransmissions of the final response that are received while in the 'Completed' state MUST cause the ACK to be re-passed to the transport layer for retransmission, but the newly received response MUST NOT be passed up to the TU.*/
        public event Action<t_packet> OnEventTraverse__Completed__SIP_300_699; 
        
        private bool m_isDisposed = false;
        private readonly ITimer Timer_A;
        private readonly ITimer Timer_A2;
        private readonly ITimer Timer_B;
        private readonly ITimer Timer_D;
        
        public States CurrentState { get; private set; } = States.Calling_Start;
        
        public client__invite__udp(ITimerFactory timerFactory)
        {
            this.Timer_A = timerFactory.CreateTimer("Timer_A", this.OnTimer);
            this.Timer_A2 = timerFactory.CreateTimer("Timer_A2", this.OnTimer);
            this.Timer_B = timerFactory.CreateTimer("Timer_B", this.OnTimer);
            this.Timer_D = timerFactory.CreateTimer("Timer_D", this.OnTimer);
        }
        
        public void Dispose()
        {
            if (!this.m_isDisposed)
            {
                this.Timer_A.Dispose();
                this.Timer_A2.Dispose();
                this.Timer_B.Dispose();
                this.Timer_D.Dispose();
                this.m_isDisposed = true;
            }
        }
        
        private void CheckNotDisposed()
        {
            if (this.m_isDisposed)
            {
                throw new ObjectDisposedException("client__invite__udp");
            }
        }
        
        public void Start()
        {
            CheckNotDisposed();
            this.CurrentState = States.Calling_Start;
            this.Timer_A.StartOrReset(0.5);
            this.Timer_B.StartOrReset(32);
            OnStateEnter__Calling_Start?.Invoke();
        }
        
        private void OnTimer(ITimer timer)
        {
            CheckNotDisposed();
            switch (this.CurrentState)
            {
            case States.Calling_Start:
                if (timer == this.Timer_A)
                {
                    SetState(States.Calling_Retransmit);
                }
                else if (timer == this.Timer_B)
                {
                }
                else 
                {
                    throw new Exception("Unexpected timer finish in state Calling_Start. Timer was " + timer);
                }
                break;
                
            case States.Calling_Retransmit:
                if (timer == this.Timer_A2)
                {
                    SetState(States.Calling_Retransmit);
                }
                else if (timer == this.Timer_B)
                {
                    OnTimerTraverse__Timer_B?.Invoke();
                    SetState(States.Terminated);
                }
                else 
                {
                    throw new Exception("Unexpected timer finish in state Calling_Retransmit. Timer was " + timer);
                }
                break;
                
            case States.Completed:
                if (timer == this.Timer_D)
                {
                    SetState(States.Terminated);
                }
                else 
                {
                    throw new Exception("Unexpected timer finish in state Completed. Timer was " + timer);
                }
                break;
                
            default:
                throw new Exception("No timer events expected in state " + this.CurrentState);
            }
        }
        
        public void ProcessEvent__SIP_1xx(t_packet packet)
        {
            CheckNotDisposed();
            switch (this.CurrentState)
            {
            case States.Calling_Start:
                OnEventTraverse__SIP_1xx?.Invoke(packet);
                SetState(States.Proceeding);
                break;
                
            case States.Calling_Retransmit:
                OnEventTraverse__SIP_1xx?.Invoke(packet);
                SetState(States.Proceeding);
                break;
                
            case States.Proceeding:
                OnEventTraverse__SIP_1xx?.Invoke(packet);
                SetState(States.Proceeding);
                break;
                
            case States.Completed:
                break;
                
            default:
                throw new Exception("Event SIP_1xx is not expected in state " + this.CurrentState);
            }
        }
        
        public void ProcessEvent__SIP_2xx(t_packet packet)
        {
            CheckNotDisposed();
            switch (this.CurrentState)
            {
            case States.Calling_Start:
                OnEventTraverse__SIP_2xx?.Invoke(packet);
                SetState(States.Terminated);
                break;
                
            case States.Calling_Retransmit:
                OnEventTraverse__SIP_2xx?.Invoke(packet);
                SetState(States.Terminated);
                break;
                
            case States.Proceeding:
                OnEventTraverse__SIP_2xx?.Invoke(packet);
                SetState(States.Terminated);
                break;
                
            case States.Completed:
                break;
                
            default:
                throw new Exception("Event SIP_2xx is not expected in state " + this.CurrentState);
            }
        }
        
        public void ProcessEvent__SIP_300_699(t_packet packet)
        {
            CheckNotDisposed();
            switch (this.CurrentState)
            {
            case States.Calling_Start:
                OnEventTraverse__SIP_300_699?.Invoke(packet);
                SetState(States.Completed);
                break;
                
            case States.Calling_Retransmit:
                OnEventTraverse__SIP_300_699?.Invoke(packet);
                SetState(States.Completed);
                break;
                
            case States.Proceeding:
                OnEventTraverse__SIP_300_699?.Invoke(packet);
                SetState(States.Completed);
                break;
                
            case States.Completed:
                OnEventTraverse__Completed__SIP_300_699?.Invoke(packet);
                SetState(States.Completed);
                break;
                
            default:
                throw new Exception("Event SIP_300_699 is not expected in state " + this.CurrentState);
            }
        }
        
        public void ProcessEvent__TransportError()
        {
            CheckNotDisposed();
            switch (this.CurrentState)
            {
            case States.Calling_Start:
                OnEventTraverse__TransportError?.Invoke();
                SetState(States.Terminated);
                break;
                
            case States.Calling_Retransmit:
                OnEventTraverse__TransportError?.Invoke();
                SetState(States.Terminated);
                break;
                
            case States.Proceeding:
                OnEventTraverse__TransportError?.Invoke();
                SetState(States.Terminated);
                break;
                
            case States.Completed:
                OnEventTraverse__TransportError?.Invoke();
                SetState(States.Terminated);
                break;
                
            default:
                throw new Exception("Event TransportError is not expected in state " + this.CurrentState);
            }
        }
        
        private void SetState(States state)
        {
            CheckNotDisposed();
            switch (state)
            {
            case States.Calling_Start:
                this.CurrentState = States.Calling_Start;
                this.Timer_A.StartOrReset(0.5);
                this.Timer_B.StartOrReset(32);
                OnStateEnter__Calling_Start?.Invoke();
                break;
                
            case States.Calling_Retransmit:
                this.CurrentState = States.Calling_Retransmit;
                this.Timer_A.Stop();
                this.Timer_A2.StartOrReset(1);
                OnStateEnter__Calling_Retransmit?.Invoke();
                break;
                
            case States.Proceeding:
                this.CurrentState = States.Proceeding;
                this.Timer_A.Stop();
                this.Timer_A2.Stop();
                this.Timer_B.Stop();
                break;
                
            case States.Completed:
                this.CurrentState = States.Completed;
                this.Timer_A.Stop();
                this.Timer_A2.Stop();
                this.Timer_B.Stop();
                this.Timer_D.StartOrReset(32);
                break;
                
            case States.Terminated:
                this.CurrentState = States.Terminated;
                OnStateEnter__Terminated?.Invoke();
                break;
                
            default:
                throw new Exception("Unexpected state " + state);
            }
        }
        
    }
}
