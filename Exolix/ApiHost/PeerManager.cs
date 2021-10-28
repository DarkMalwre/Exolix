using Exolix.ApiBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.ApiHost
{
    public class PeerManager
    {
        private List<Tuple<string, ApiConnector>> Peers = new List<Tuple<string, ApiConnector>>();

        public void AddWorkingPeerNode(string identifier, ApiConnector node)
        {
            Peers.Add(Tuple.Create(identifier, node));
        }

        public ApiConnector? GetPeer(string identifier)
        {
            foreach (var peer in Peers)
            {
                if (peer.Item1 == identifier)
                {
                    return peer.Item2;
                }
            }

            return null;
        }
    }
}
