using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Notice
{
    /// <summary>
    /// 公告ID
    /// </summary>
    public int NoticeID { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// 发布日期
    /// </summary>
    public DateOnly CreationDate { get; set; }

    /// <summary>
    /// 作者ID
    /// </summary>
    public int UserID { get; set; }

    public virtual User User { get; set; } = null!;
}
