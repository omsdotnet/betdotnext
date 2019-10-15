using System;
using System.Collections.Generic;
using BetDotNext.Commands;

namespace BetDotNext.Services
{
    public class ActiveCommandService
    {
        private static readonly Dictionary<string, Type> Commands = new Dictionary<string, Type>();

        public ActiveCommandService()
        {
            Commands.Add(HelloCommand.Command, typeof(HelloCommand));
        }

        public Type GetCommand(string command) => 
            Commands.ContainsKey(command) ? Commands[command] : null;
    }
}