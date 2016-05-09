using System.Collections.Generic;

namespace Portal.Dto
{
    public class FindClientUserResponse : PagerFindResponse<ClientUserDto>
    {
        public FindClientUserResponse(int total, IEnumerable<ClientUserDto> roles):base(total,roles)
        {
            
        }
    }
}
