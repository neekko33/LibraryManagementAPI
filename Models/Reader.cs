using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Reader
{
    /// <summary>
    /// 读者ID
    /// </summary>
    public int ReaderID { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
}
