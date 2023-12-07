using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Book
{
    /// <summary>
    /// 图书ID
    /// </summary>
    public int BookID { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// 出版年份
    /// </summary>
    public DateOnly? PublicationDate { get; set; }

    /// <summary>
    /// 出版社
    /// </summary>
    public string? PublishingHouse { get; set; }

    /// <summary>
    /// ISBN码
    /// </summary>
    public string? ISBN { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public string? Price { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryID { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public string? AvailabilityStatus { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 图片地址
    /// </summary>
    public string? ImgUrl { get; set; }

    /// <summary>
    /// 图书简介
    /// </summary>
    public string? Introduction { get; set; }

    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();

    public virtual Category Category { get; set; } = null!;
}
