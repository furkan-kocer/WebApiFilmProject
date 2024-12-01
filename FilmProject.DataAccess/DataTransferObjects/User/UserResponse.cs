﻿namespace FilmProject.DataAccess.DataTransferObjects.User
{
    public class UserResponse
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string? country { get; set; }
        public string? city { get; set; }
        public string? region { get; set; }
        public string? state { get; set; }
        public string identityNumber { get; set; }
        public string? imgUrl { get; set; }
    }
}
