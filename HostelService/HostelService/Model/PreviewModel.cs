using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PreviewModel
    {
        public AadhaarDetailsPreviewModel? AadharData { get; set; }
        public PersonalDetailsPreviewModel? PersonalDetails { get; set; }
        public CastDetailsPreviewModel? CastDetails { get; set; }
        public DomicilePreviewModel? DomicileDetails { get; set; }
        public AddressDetailsPreviewModel? PermanentAddress { get; set; }
        public AddressDetailsPreviewModel? PresentAddress { get; set; }
        public AddressDetails2PreviewModel? ParentAddress { get; set; }
        public AddressDetails2PreviewModel? GuardianAddress { get; set; }
        public OtherDetailsPreviewModel? OtherDetails { get; set; }
        public List<HostelPreferencePreviewModel>? Preferences { get; set; }
        public List<DocumentPreviewModel>? Documents { get; set; }
        public List<CurrentCoursePreviewModel>? CurrentCourse { get; set; }
        public List<PastCoursePreviewModel>? PastCourse { get; set; }
        public List<PreCurrentCoursePreviewModel>? PreCurrentCourse { get; set; }
        public List<PrePastCoursePreviewModel>? PrePastCourse { get; set; }
    }
    public class AadhaarDetailsPreviewModel
    {
        public string? AadhaarImage { get; set; }
        public string? AadhaarImageSrc { get; set; }
        public string? UIDNo { get; set; }
        public string? UIDNoDec { get; set; }
        public string? ApplicantName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        public string? Pincode { get; set; }
    }
    public class AadhaarPreviewResponseModel
    {
        [DisplayName("Aadhar Number")]
        public string? UIDNo { get; set; }
        [DisplayName("Applicant Name")]
        public string? ApplicantName { get; set; }
        [DisplayName("Date of Birth")]
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        public string? Pincode { get; set; }
    }
    public class PersonalDetailsPreviewModel
    {
        [DisplayName("Are you New or an Existing Applicant?")]
        public string? IsNewApplicant { get; set; }
        [DisplayName("Course Type")]
        public string? CourseType { get; set; }
        [DisplayName("Hostel District")]
        public string? HostelDistrict { get; set; }
        [DisplayName("Hostel Taluka")]
        public string? HostelTaluka { get; set; }
        public string? HostelName { get; set; }
        [DisplayName("Name")]
        public string? ApplicantFullName { get; set; }
        [DisplayName("Date of Birth")]
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Age { get; set; }
        [DisplayName("Marrital Status")]
        public string? MarritalStatus { get; set; }
        [DisplayName("Is Father Alive?")]
        public string? IsFatherAlive { get; set; }
        [DisplayName("Father Name")]
        public string? FatherName { get; set; }
        [DisplayName("Is Mother Alive?")]
        public string? IsMotherAlive { get; set; }
        [DisplayName("Mother Name")]
        public string? MotherName { get; set; }
        [DisplayName("Is Orphan")]
        public string? IsOrphan { get; set; }
        [DisplayName("Guardian Name")]
        public string? GuardianName { get; set; }
        [DisplayName("Mobile")]
        public string? MobileNo { get; set; }
        [DisplayName("Email")]
        public string? EmailId { get; set; }        
    }
    public class CastDetailsPreviewModel
    {
        [DisplayName("Caste Category")]
        public string? CasteCategory { get; set; }
        [DisplayName("Caste")]
        public string? Cast { get; set; }
        [DisplayName("Certificate Number")]
        public string? CertificateNumber { get; set; }
        [DisplayName("Certificate Issue District")]
        public string? CertificateIssueDistrict { get; set; }
        [DisplayName("Issuing Authority")]
        public string? IssuingAuthority { get; set; }
        [DisplayName("Certificate Applicant Name")]
        public string? CastCertificateApplicantName { get; set; }
        [DisplayName("Issue Date")]
        public string? IssueDate { get; set; }
    }
    public class DomicilePreviewModel
    {
        [DisplayName("Is Maharastra Domicile?")]
        public string? IsMaharastraDomicile { get; set; }
        [DisplayName("Domicile Certificate No.")]
        public string? DomicileCertificateNo { get; set; }
        [DisplayName("Domicile Issuing Authority")]
        public string? DomicileIssuingAuthority { get; set; }
    }
    public class AddressDetailsPreviewModel
    {
        [DisplayName("Address 1")]
        public string? Address1 { get; set; }
        [DisplayName("Address 2")]
        public string? Address2 { get; set; }
        [DisplayName("Address 3")]
        public string? Address3 { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        public string? Village { get; set; }
        public string? Pincode { get; set; }
    }
    public class AddressDetails2PreviewModel
    {
        [DisplayName("Address 1")]
        public string? Address1 { get; set; }
        [DisplayName("Address 2")]
        public string? Address2 { get; set; }
        [DisplayName("Address 3")]
        public string? Address3 { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        public string? Village { get; set; }
        public string? Pincode { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
    }
    public class OtherDetailsPreviewModel
    {
        [DisplayName("Is Availed Maintenance Allowance?")]
        public string? isAvailedMaintainaceAllowance { get; set; }
        [DisplayName("Maintenance Allowance UserId")]
        public string? AvailedMaintainaceAllowanceUserId { get; set; }
        [DisplayName("Is Disable?")]
        public string? IsApplicantDisable { get; set; }
        [DisplayName("Disability Percentage")]
        public string? DisabilityPercentage { get; set; }
        [DisplayName("Is Course Available In District?")]
        public string? IsCourseAvailableInDistrict { get; set; }
        [DisplayName("Has lived in Govt. hostel before?")]
        public string? IsLivedInGovermentHostel { get; set; }
        [DisplayName("Hostel District")]
        public string? HostelDistrict { get; set; }
        [DisplayName("Hostel Name")]
        public string? NameOfHostel { get; set; }
        [DisplayName("Duration From")]
        public string? DurationFrom { get; set; }
        [DisplayName("Duration To")]
        public string? DurationTo { get; set; }
        [DisplayName("Have all materials of said hostel been return?")]
        public string? IsMaterialsReturn { get; set; }

        [DisplayName("Are You Salaried Or Self Employed")]
        public string? AreYouSalariedOrSelfEmployed { get; set; }

        [DisplayName("Do You Do Any Kind Of Business")]
        public string? DoYouDoAnyKindOfBusiness { get; set; }
    }
    public class HostelPreferencePreviewModel
    {
        [DisplayName("Hostel Name")]
        public string? HostelName { get; set; }
        public int Preference { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
    }
    public class DocumentPreviewModel
    {
        public string? DocumentName { get; set; }
        public string? FilePath { get; set; }
    }
    public class CurrentCoursePreviewModel
    {
        [DisplayName("Admission Year")]
        public string? Year { get; set; }
        [DisplayName("Stream")]
        public string? CourseCategoryName { get; set; }
        [DisplayName("Course Name")]
        public string? CourseName { get; set; }
        [DisplayName("Course Type")]
        public string? CourseType { get; set; }
        [DisplayName("Qualification Level")]
        public string? QualificationType { get; set; }
        [DisplayName("Year of Study")]
        public string? EducationYear { get; set; }
        [DisplayName("Institute Name")]
        public string? CollegeName { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        [DisplayName("Mode of Study")]
        public string? Type { get; set; }
        [DisplayName("Admission Through")]
        public string? AdmissionType { get; set; }
        [DisplayName("Admission Date")]
        public string? AdmissionDate { get; set; }
        [DisplayName("Completed or Pursuing ?")]
        public string? IsCompleted { get; set; }
        public string? Result { get; set; }
        public string? Percentage { get; set; }
        [DisplayName("Start Year")]
        public string? StartYear { get; set; }
        [DisplayName("Gap Years")]
        public string? GapYears { get; set; }
        [DisplayName("Gap Reason")]
        public string? GapReason { get; set; }
    }
    public class PastCoursePreviewModel
    {
        [DisplayName("Admission Year")]
        public string? Year { get; set; }
        [DisplayName("Stream")]
        public string? CourseCategoryName { get; set; }
        [DisplayName("Course Name")]
        public string? CourseName { get; set; }
        [DisplayName("Qualification Level")]
        public string? CourseType { get; set; }
        [DisplayName("Institute Name")]
        public string? CollegeName { get; set; }
        [DisplayName("Board / University Name")]
        public string? UniversityName { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        [DisplayName("Mode of Study")]
        public string? Type { get; set; }
        [DisplayName("Is Completed ?")]
        public string? IsCompleted { get; set; }
        public string? Result { get; set; }
        [DisplayName("Passing Year")]
        public string? PassingYear { get; set; }
        public string? Percentage { get; set; }
        public string? Attempts { get; set; }
        [DisplayName("Gap Years")]
        public string? GapYears { get; set; }
        [DisplayName("Gap Reason")]
        public string? GapReason { get; set; }
    }

    public class PreCurrentCoursePreviewModel
    {
        [DisplayName("School Name")]
        public string? SchoolName { get; set; }
        [DisplayName("School Udise Code")]
        public string? schoolUDISE { get; set; }
        [DisplayName("Standard/Class")]
        public string? Standard { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        [DisplayName("Is Completed Or Pursuing")]
        public string? IsCompleted { get; set; }
        [DisplayName("Admission Date")]
        public string? Admissiondate { get; set; }
        public string? Attendance { get; set; }
        public string? Percentage { get; set; }
        [DisplayName("Class Rank")]
        public string? PrevRank { get; set; }
        public string? Result { get; set; }
        public string? Gap { get; set; }
    }

    public class PrePastCoursePreviewModel
    {
        [DisplayName("School Name")]
        public string? SchoolName { get; set; }
        [DisplayName("School Udise Code")]
        public string? schoolUDISE { get; set; }
        [DisplayName("Standard/Class")]
        public string? Standard { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Taluka { get; set; }
        [DisplayName("Is Completed Or Pursuing")]
        public string? IsCompleted { get; set; }
        [DisplayName("Course Start Year")]
        public string? StartYear { get; set; }
        [DisplayName("Course End Year")]
        public string? PassingYear { get; set; }
        public string? Percentage { get; set; }
        [DisplayName("Class Rank")]
        public string? PrevRank { get; set; }
        public string? Result { get; set; }
    }
}
