using System;
using System.Collections.Generic;

namespace bdapiaaa.Models;

public partial class PaymentRegistration
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? MonthlyPayment { get; set; }

    public decimal? PaymentAmount { get; set; }

    public decimal? DebtOrOverpayment { get; set; }
}
