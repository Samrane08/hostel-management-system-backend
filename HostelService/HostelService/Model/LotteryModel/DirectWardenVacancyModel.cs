using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.LotteryModel
{
    public class DirectWardenVacancyModel
    {
        public int QPID { get; set; } = 0;
        public string Percentage {  get; set; }=string.Empty;
        public int ExistingStudent { get; set; } = 0;
        public int AvlSeats { get; set; } = 0;
        public int AvlForSecondary { get; set; } = 0;
        public int AvlHigherSecondary { get; set; } = 0;
        public int AvlForNonProfessional { get; set; } = 0;
        public int AvlForProfessional { get; set; } = 0;
        public int AdmtSeatsForSecondary { get; set; } = 0;
        public int AdmtSeatsForHigher { get; set; } = 0;
        public int AdmtForNonProfessional { get; set; } = 0;
        public int AdmtForProfessional { get; set; } = 0;
        public int VacntForSecondary { get; set; } = 0;
        public int VacantForHigher { get; set; } = 0;
        public int VacantForNonProfessional { get; set; } = 0;
        public int VacantForProfessional { get; set; } = 0;
        public int VacantMantralayaSpecial { get; set; } = 0;
        public int VacantCollectorSpecial { get; set; } = 0;
        public int VacantTotalSpecial {  get; set; }= 0;

        public int DSVP { get;set;}=0;
        public int msg { get; set; } = 0;
        public int WorkFlowId { get; set; } = 0;
    }

    public class GETDirectWardenVacancyModel
    {
        public DirectWardenVacancyModel Disabled { get; set; } = null;
        public DirectWardenVacancyModel Orphan { get; set; } = null;
        public DirectWardenVacancyModel Mang { get; set; } = null;
        public DirectWardenVacancyModel Mehtar { get; set; } = null;
        public DirectWardenVacancyModel Other { get; set; } = null;
        public DirectWardenVacancyModel SBC { get; set; } = null;
        public DirectWardenVacancyModel EBCOBC { get; set; } = null;
        public DirectWardenVacancyModel VJNT { get; set; } = null;
        public DirectWardenVacancyModel ST { get; set; } = null;

    }
    public class DropDownData
    {
        public int QPID { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
