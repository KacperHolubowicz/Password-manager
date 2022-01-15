using Services.Infrastructure;
using System;

namespace Services.DTO.ServicePassword
{
    public class ServicePasswordStaticMapper
    {
        public static Domain.Models.ServicePassword GetPasswordFromDTO
            (ServicePasswordPostDTO passwordPost, ISecretsService secretsService, string masterKey)
        {
            Tuple<byte[], byte[]> passwordAndIv =
                secretsService.CipherServicePassword(passwordPost.Password, masterKey);
            return new Domain.Models.ServicePassword()
            {
                Description = passwordPost.Description,
                IV = passwordAndIv.Item2,
                Password = passwordAndIv.Item1
            };
        }
        //TODO fix put dto
        public static Domain.Models.ServicePassword GetPasswordFromDTO
            (ServicePasswordPutDTO passwordPut, ISecretsService secretsService, string masterKey)
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
