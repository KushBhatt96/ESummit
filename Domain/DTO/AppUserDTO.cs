namespace Domain.DTO
{
    public class AppUserDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string  Username { get; set; }

        public AppUserDTO(AppUser user)
        {
            Firstname = user.FirstName;
            Lastname = user.LastName;
            DateOfBirth = user.DateOfBirth.ToString();
            Email = user.Email;
            Username = user.UserName;
        }
    }
}
