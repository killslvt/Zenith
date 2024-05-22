using DiscordRpcDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zenith.RPC
{
    internal partial class rpcstart
    {
        private DiscordRpc.EventHandlers handlers;

        private DiscordRpc.RichPresence presence;

        public static bool isEnabled;

        public void DiscordRPC()
        {
            if (File.Exists("[DATA]/DiscordRPC/discord-rpc-w32.dll"))
            {
                if (isEnabled)
                {
                    handlers = default(DiscordRpc.EventHandlers);
                    DiscordRpc.Initialize("959979634185875516", ref handlers, true, null);
                    presence = default(DiscordRpc.RichPresence);
                    DiscordRpc.Initialize("959979634185875516", ref handlers, true, null);
                    presence.details = "by ilycross";
                    presence.state = "discord.gg/pMAsDK4Z9d";
                    presence.largeImageKey = "gay";
                    //presence.smallImageKey = "blood";
                    presence.largeImageText = "Zenith Injecter";
                    presence.smallImageText = "Fuck Byfron";
                    presence.startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    DiscordRpc.UpdatePresence(ref presence);
                }
                else
                {
                    isEnabled = false;
                    DiscordRpc.Shutdown();
                }
            }
            else
            {
                MessageBox.Show("No discord-rpc-w32.dll found. Please check your files");
            }

        }
    }
}
