using System;

namespace model
{
    public enum MessageType : int
    {
        BlogReply = 0,
        ForumReply = 1,
        PrivateMsg = 2,
        SystemMsg = 3
    }

    //enum扩展类，提供ToInt功能
    public static class EnumExtension
    {
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }
}