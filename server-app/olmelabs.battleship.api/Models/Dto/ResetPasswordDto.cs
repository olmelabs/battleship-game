namespace olmelabs.battleship.api.Models.Dto
{
    public class ResetPasswordDto
    {
           public string Code { get; set; }
           public string Password { get; set; }
           public string Password2 { get; set; }
    }
}
