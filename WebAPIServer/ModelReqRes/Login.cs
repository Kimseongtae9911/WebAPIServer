using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes
{
    public class PkLoginReq
    {
        [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
        [StringLength(Constants.ID_LENGTH, ErrorMessage = "ID IS TOO LONG")]
        public string ID { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
        [StringLength(Constants.PW_LENGTH, ErrorMessage = "PASSWORD IS TOO LONG")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class PkLoginRes
    {
        public ErrorCode Result { get; set; } = ErrorCode.None;
    }
}
