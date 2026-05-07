using UnityEngine;

public class WfcTileVariant : MonoBehaviour
{
        public WfcTile prefab;
        public int[] sockets;
        public int rotation; // 0, 90, 180, 270

        public WfcTileVariant(WfcTile prefab, int[] sockets, int rotation)
        {
            this.prefab = prefab;
            this.sockets = sockets;
            this.rotation = rotation;
        }
}
