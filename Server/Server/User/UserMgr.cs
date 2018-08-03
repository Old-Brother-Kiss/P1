using Google.Protobuf;
using System.Collections.Generic;

class UserMgr
{
    private Dictionary<int, User> connect_id_to_user = new Dictionary<int, User>();
    private Dictionary<int, User> user_id_to_user = new Dictionary<int, User>();

    public void AddUserByConnectId(User user)
    {
        this.connect_id_to_user[user.ConnectId] = user;
    }

    public User GetUserByConnectId(int connect_id)
    {
        if (!this.connect_id_to_user.ContainsKey(connect_id))
        {
            return null;
        }
        return this.connect_id_to_user[connect_id];
    }

    public void AddUserByUserId(User user)
    {
        this.user_id_to_user[user.UserId] = user;
    }

    public User GetUserByUserId(int user_id)
    {
        if (!this.user_id_to_user.ContainsKey(user_id))
        {
            return null;
        }
        return this.user_id_to_user[user_id];
    }

    public void DelUser(User user)
    {
        if (this.connect_id_to_user.ContainsKey(user.ConnectId))
        {
            this.connect_id_to_user.Remove(user.ConnectId);
        }

        if (this.user_id_to_user.ContainsKey(user.UserId))
        {
            this.user_id_to_user.Remove(user.UserId);
        }

        Log.Debug("Del User Id:" + user.UserId);
    }
}
