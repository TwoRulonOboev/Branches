using System;
using System.Collections.Generic;

namespace bdapiaaa.Models;

public partial class Abonent
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public decimal? MonthlyPayment { get; set; }
}
