﻿namespace LokantaSisteme_API.Models.Account
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Persistent { get; set; }
    }
}
