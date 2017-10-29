using System;

namespace OwnCloud.Model
{
    [Flags]
    public enum OcsSharePermission
    {
        None = 0,
        Read = 1,
        Update = 2,
        Create = 4,
        Delete = 8,
        Share = 16,
    }
}
