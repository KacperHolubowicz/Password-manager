namespace Services.DTO.ServicePassword
{
    public class ServicePasswordStaticMapper
    {
        public static Domain.Models.ServicePassword GetPasswordFromDTO
            (ServicePasswordPostDTO passwordPost)
        {
            return new Domain.Models.ServicePassword()
            {
                Description = passwordPost.Description,
                //Password = passwordPost.Password
            };
        }

        public static Domain.Models.ServicePassword GetPasswordFromDTO
            (ServicePasswordPutDTO passwordPut)
        {
            return new Domain.Models.ServicePassword()
            {
                Description = passwordPut.Description,
                Password = passwordPut.Password
            };
        }

        public static ServicePasswordGetDTO GetDTOFromPassword(Domain.Models.ServicePassword password)
        {
            return new ServicePasswordGetDTO()
            {
                ID = password.ID,
                Description = password.Description,
                Password = password.Password
            };
        }
    }
}
