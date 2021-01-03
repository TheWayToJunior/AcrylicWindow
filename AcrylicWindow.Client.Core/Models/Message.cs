﻿using AcrylicWindow.Client.Core.IContract;

namespace AcrylicWindow.Client.Core.Model
{
    public class UserMessage : IMessage
    {
        public string UserName { get; set; }

        public UserMessage(string userName)
        {
            UserName = userName;
        }
    }

    public class LoginMessage : IMessage
    {
        public AuthenticationState State { get; set; }

        public string Email { get; set; }

        public LoginMessage(AuthenticationState state, string email)
        {
            State = state;
            Email = email;
        }
    }

    public class LogoutMessage : IMessage
    {
        public string Email { get; set; }

        public LogoutMessage(string email)
        {
            Email = email;
        }
    }
}
