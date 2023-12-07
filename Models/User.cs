using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class User
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserID { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public string? UserType { get; set; }

    public virtual ICollection<Notice> Notices { get; set; } = new List<Notice>();
}
