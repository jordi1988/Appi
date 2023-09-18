namespace Infrastructure.MsSql
{
    public class UserDto
    {
        public Guid UserID{ get; set; }
        public string UserName{ get; set; } = string.Empty;
    }
}
