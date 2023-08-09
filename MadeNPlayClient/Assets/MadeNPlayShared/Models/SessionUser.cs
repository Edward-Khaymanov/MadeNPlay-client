namespace MadeNPlayShared
{
    public class SessionUser
    {
        public ulong UserId { get; set; }
        public NetworkUser User { get; set; }
        public UserState State { get; set; }
        public SessionTeam Team { get; set; }

        public bool IsValid => User.Equals(default(NetworkUser)) == false && Team != null;
    }
}