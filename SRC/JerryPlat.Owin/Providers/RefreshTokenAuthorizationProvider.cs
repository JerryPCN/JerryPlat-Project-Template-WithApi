﻿using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;

namespace JerryPlat.Owin.Providers
{
    public class RefreshTokenAuthorizationProvider : AuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

        public override void Create(AuthenticationTokenCreateContext context)
        {
            string tokenValue = Guid.NewGuid().ToString("n");

            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddHours(2);

            _refreshTokens[tokenValue] = context.SerializeTicket();

            context.SetToken(tokenValue);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (_refreshTokens.TryRemove(context.Token, out value))
            {
                context.DeserializeTicket(value);
            }
        }
    }
}