using System;
using System.Collections.Generic;

namespace DbFirstEfCore.Models;

public partial class FeeBill
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public DateTime BillingMonth { get; set; }

    public decimal? PreviousArrears { get; set; }

    public decimal TutionFee { get; set; }

    public decimal? AdmissionFee { get; set; }

    public decimal? Fine { get; set; }

    public decimal? StationaryCharges { get; set; }

    public decimal FeePayable { get; set; }

    public decimal? FeePaid { get; set; }

    public decimal? NextArrears { get; set; }

    public virtual Student Student { get; set; } = null!;
}
