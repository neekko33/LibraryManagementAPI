using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Borrow
{
    public int BorrowID { get; set; }

    public int BookID { get; set; }

    public int ReaderID { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public bool IsOverdue { get; set; }

    public DateOnly DueDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual Reader Reader { get; set; } = null!;
}
