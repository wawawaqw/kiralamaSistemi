﻿namespace LokantaSisteme_API.Models.Account
{
    public class ResetPasswordModel
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Token { get; set; }
    }
}
