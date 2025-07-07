namespace MasterService.Models.ReqModel
{
    public class CurrentCourse_Docs
    {
        public long SrNo { get; set; }
        public int? DocumentID { get; set; }
        public int? MappingID { get; set; }
        public byte[]? Attachment { get; set; }
        public string ContentType { get; set; }=string.Empty;
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public string FlagStatus { get; set; }=string.Empty ;
        public char? ApplyStatus { get; set; }
        public char? UsedStatus { get; set; }
        public bool? IsReapply { get; set; }
        public string AttachmentName { get; set; } = string.Empty;
        public bool? Flg_bucket { get; set; }
        public string FolderName { get; set; } = string.Empty;
    }
}
