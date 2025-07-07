using System.ComponentModel.DataAnnotations;

namespace System.ComponentModel;

    public class reqModel
    {
        public int rowId {  get; set; }
    }

public class pfmsreqmodel
{
    [Required]
    public string ifsccode { get; set; }
}
public class BranchDetail
{
    public string BankStatus { get; set; }
    public string BranchStatus { get; set; }
    public string BranchCode { get; set; }
    public string BankName { get; set; }
}

public class BranchDetailsByIFSCcODE
{
    public int Id { get; set; }

    public string BankName { get; set; } = string.Empty;

    public string BranchCode { get; set; } = string.Empty;
    public string BankStatus { get; set; } = string.Empty;
    public  string BranchStatus { get; set; } = string.Empty;
}

public class BranchCodelist
{
    public string BranchCode { get; set; } = string.Empty;

}

public class branchDetails
{
    public BranchDetailsByIFSCcODE BankDetails {  get; set; }
   public List<string> BranchCode { get; set; }
}
