namespace ValheimTwitch.Events
{
    public class HUDMessageAction
    {
        public static void PlayerMessage(string message, bool center = true)
        {
            var messageType = center ? MessageHud.MessageType.Center : MessageHud.MessageType.TopLeft;

            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.Message(messageType, message);
            }
        }

        public static void Run()
        {


            PlayerMessage($"fat bald and retarded, enjoy 2 minutes", true);
        }
    }
}