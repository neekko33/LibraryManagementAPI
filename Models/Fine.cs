using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Fine
{
    public int FineID { get; set; }

    public int BorrowID { get; set; }

    public decimal FineAmount { get; set; }

    public bool IsPaid { get; set; }

    public virtual Borrow Borrow { get; set; } = null!;
}
