using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Category
{
    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryID { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string CategoryName { get; set; } = null!;

    /// <summary>
    /// 分类字符（中图法）
    /// </summary>
    public string? CategoryChar { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
