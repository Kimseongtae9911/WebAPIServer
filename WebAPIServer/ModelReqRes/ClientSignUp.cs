using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes
{
    public class PkClientSignUpReq
    {
        [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
        [StringLength(20, ErrorMessage = "ID IS TOO LONG")]
        public string ID { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
        [StringLength(20, ErrorMessage = "PASSWORD IS TOO LONG")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class PkClientSignUpRes
    {
        public ErrorCode Result { get; set; } = ErrorCode.None;
    }
}
