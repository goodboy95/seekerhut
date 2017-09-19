using System;

namespace model
{
    public enum MessageType : int
    {
        blogReply = 0,
        forumReply = 1,
        privateMsg = 2,
        systemMsg = 3
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