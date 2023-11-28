namespace Domain.DTO
{
    public class AppUserDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string  Username { get; set; }
        public string Jwt { get; set; }

        public AppUserDTO(AppUser user, string jwt)
        {
            Firstname = user.FirstName;
            Lastname = user.LastName;
            DateOfBirth = user.DateOfBirth.ToString();
            Email = user.Email;
            Username = user.UserName;
            Jwt = jwt;
        }
    }
}
