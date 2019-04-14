using UnityEngine;

namespace Assets.Enigma.Enigma.Core.Networking
{
    [RequireComponent(typeof(NetworkEntity))]
    public abstract class NetworkedComponent : MonoBehaviour
    {
        private NetworkEntity _networkEntity;

        public void Start()
        {
            _networkEntity = GetComponent<NetworkEntity>();
        }

        protected void SendAsync()
        {
            _networkEntity.SendAsync(this);
        }

        protected void SendSync()
        {
            _networkEntity.SendAsync(this);
        }

        protected void Update()
        {
            var obj = this; // have to create a reference value to pass 
            _networkEntity.TryGetUpdates(obj);
        }

        /// <summary>
        /// Declare a method here for the server builder to declare this as code to run every server tick.
        /// </summary>
        public abstract void RunServerSide();

    }
}
