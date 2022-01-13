namespace Services.DTO.User
{
    public class UserStaticMapper
    {
        public static Domain.Models.User GetUserFromDTO(UserPostDTO userPost)
        {
            return new Domain.Models.User()
            {
                Username = userPost.Username,
                Login = userPost.Login,
                Email = userPost.Email,
                Password = userPost.Password,
                MasterPassword = userPost.MasterPassword
            };
        }

        public static UserGetDTO GetDTOFromUser(Domain.Models.User user)
        {
            return new UserGetDTO()
            {
                ID = user.ID,
                Email = user.Email,
                Login = user.Login,
                Username = user.Username
            };
        }
    }
}
