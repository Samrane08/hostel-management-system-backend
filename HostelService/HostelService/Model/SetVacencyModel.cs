namespace Model
{
    public class SetVacencyModel
    {
        public long? Id { get; set; }
        public long? CasteCategoryId { get; set; }
        public string CasteCategory { get; set; } = "";
        public long? CasteId { get; set; }
        public string Caste { get; set; } = "";
        public int? Vacency { get; set; }
        public int? Year { get; set; }
    }

    public class FetchVacancyModel
    {
        public long? CasteCategoryVacancyId { get; set; }
        public string? VacancyCategoryName { get; set; } = "";
        public int? Percentage { get; set; }
        public int? CourseTypeQuotaType_Vacancy { get; set; }
        public decimal? Actual_Vacancy { get; set; }
        public int? Actual_Vacancy_SetBy_Warden { get; set; }
        public int? VacancyCategoryInPercentage { get; set; }
        public decimal? Calculated_Vacancy { get; set; }
        public int? CategoryCappingPercentage { get; set; }
        public int? CasteCategoryVacancy_setBy_Warden { get; set; }
        public long? CasteVacancyId { get; set; }
        public string? VacancyCasteName { get; set; } = "";
        public int? CasteVacancyPercentage { get; set; }
        public decimal? Caste_Calculated_Vacancy { get; set; }
        public int? CasteVacancy_setBy_Warden { get; set; }

    }
    public class FetchVacancyResponse
    {
        public long? CasteCategoryVacancyId { get; set; }
        public string? VacancyCategoryName { get; set; } = "";
        public int? Percentage { get; set; }
        public int? CourseTypeQuotaType_Vacancy { get; set; }
        public decimal? Actual_Vacancy { get; set; }
        public int? Actual_Vacancy_SetBy_Warden { get; set; }
        public int? VacancyCategoryInPercentage { get; set; }
        public decimal? Calculated_Vacancy { get; set; }
        public int? CategoryCappingPercentage { get; set; }
        public int? CasteCategoryVacancy_setBy_Warden { get; set; }
        public List<FetchVacancyCasteWise>? VacancyListCasteWise { get; set; }
    }
    public class FetchVacancyCasteWise
    {
        public long? CasteVacancyId { get; set; }
        public string? VacancyCasteName { get; set; } = "";
        public int? CasteVacancyPercentage { get; set; }
        public decimal? Caste_Calculated_Vacancy { get; set; }
        public int? CasteVacancy_setBy_Warden { get; set; }
    }

    public class VacencySetListModel
    {
        public long? Id { get; set; }
        public int? Vacancy { get; set; }
        public List<CasteVacancyListModel>? VacancyListCasteWise { get; set; }

    }

    public class CasteVacancyListModel
    {
        public long? CasteVacancyId { get; set; }
        public int? CasteVacancy { get; set; }
    }

    public class VacencySetModel
    {
        public int HostelId { get; set; }
        public int CourseTypeId { get; set; }
        public int ActualVacancy { get; set; }
        public List<VacencySetListModel>? Vacancies { get; set; }
    }

    public class FetchVacancyQuotaTypeResponse
    {
        public long? Id { get; set; }
        public decimal? Calculated_Vacancy { get; set; }
        public string? Quota_Type { get; set; }
        public int? Vacancy_SetBy_Warden { get; set; }
        public int? Percentage { get; set; }
    }

    public class FetchVacancyCourseTypeResponse
    {
        public int? Total_Vacancy { get; set; }
        public List<FetchVacancyCourseTypeList> CourseTypeVacancyList { get; set; }    
       
    }
    public class FetchVacancyCourseTypeList
    {
        public long? Id { get; set; }
        public decimal? Calculated_Vacancy { get; set; }
        public int? Vacancy_SetBy_Warden { get; set; }
        public int? Percentage { get; set; }
        public string? CourseType { get; set; }
        public int? Total_Vacancy { get; set; }
    }
    public class QuotaTypeVacancyModel
    {
        public int HostelId { get; set; }
        public List<QuotaTypeVacancySetListModel>? QuotaTypeVacancies { get; set; }
    }

    public class QuotaTypeVacancySetListModel
    {
        public long? Id { get; set; }
        public int? Vacancy { get; set; }
    }

    public class QuotaTypeObjectClass
    {
        public int? SpecialQuota_Vacancy_setby_warden { get; set; }
        public int? CourseQuota_Vacancy_setby_Warden { get; set; }
    }
    public class CourseTypeSetVacancyModel
    {
        public int HostelId { get; set; }
        public List<CourseTypeVacancySetListModel>? CourseTypeVacancies { get; set; }
    }
    public class CourseTypeVacancySetListModel
    {
        public long? Id { get; set; }
        public int? Vacancy { get; set; }
    }

    public class CapapcitySetModel
    {
        public int? ActualHostelCapacity { get; set; }
        public int? SanctionedHostelCapacity { get; set; }
    }
  
}