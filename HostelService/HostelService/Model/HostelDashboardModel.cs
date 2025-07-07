using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HostelDashboardModel
    {
        public int? TotalApplication { get; set; }
        public int? Approved { get; set; }
        public int? Rejected { get; set; }
        public int? Admitted { get; set; }
        public int? Renewals { get; set; }
        public int? UnderScrutiny { get; set; }
        public int? SentBack { get; set; }
        public int? TotalRegistered { get; set; }
        public int? TotalApplicationFlag { get; set; }
        public int? TotalRegisteredFlag { get; set; }
        public int? ApprovedFlag { get; set; }
        public int? RejectedFlag { get; set; }
        public int? AdmittedFlag { get; set; }
        public int? RenewalsFlag { get; set; }
        public int? UnderScrutinyFlag { get; set; }
        public int? SentBackFlag { get; set; }
        public int? CancelFlag { get; set; }
        public int ? Cancel {get;set;}


    }

    public class ApplicationServiceType
    {
        public int? division { get; set; }
        public int? district { get; set; }
        public int? serviceType { get; set; }
        public string flag { get; set; }
    }


    public class SearchFilter
    {
        public int? division { get; set; }
        public int? district { get; set; }
        public int? serviceType { get; set; }
        public int? applicationType { get; set; }
        public int? courseType { get; set; }
        public int? scrutinyLevel { get; set; }
        public int? hostelName { get; set;}
       // public string flag { get; set; }
    }

    public class SearchFilter1
    {
        public int? division { get; set; }
        public int? district { get; set; }
        public int? serviceType { get; set; }
        public int? applicationType { get; set; }
        public int? courseType { get; set; }
        public int? scrutinyLevel { get; set; }
        public int? hostelName { get; set; }
        public string flag { get; set; }
    }


    public class ClosingDateModel
    {
        public int? id { get; set; }
        public string? closingDate { get; set; }
        public string? DeptId { get; set; }
    }

    public class ClosingDateModel1
    {
        public int? id { get; set; }
        public string? ServiceName { get; set; }
        public string? EndDate { get; set; }
        public string? DeptId { get; set; }
    }

    public class ServiceTypeClosingDateModel
    {
        public int? Id { get; set; }
        public string? ServiceName { get; set; }

        public string? DeptId { get; set; }
    }

    public class ParentIdMenuMapping
    {
        public int? Id { get; set; }
        public string? MenuName { get; set; }
    }

    public class MenuInsertModel
    {
        public string? menuName { get; set; }

        public string? menuNameMr { get; set; }

        public int? parentId { get; set; }

        public string? url { get; set; }

        public string? icon { get; set; }
        public int? status { get; set; }
        public int? sort { get; set; }

    }

    public class RoleAccordDept
    {
        public string? Id { get; set; }
        public string? NormalizedName { get; set; }
    }

    public class MenuMapping
    {
        public string? value { get; set; }
        public string? MenuName { get; set; }
    }

    public class MenuMappingInsert
    {

        public string? EntityMappingId { get; set; }

        public int? MenuId { get; set; }

        public int? Status { get; set; }


    }

}
