﻿namespace FileCabinetApp.CommandHandlers;

internal class AppCommandRequest
{
    public string Command { get; }

    public string Parameters { get; }

    public AppCommandRequest(string command, string parameters)
    {
        this.Command = command;
        this.Parameters = parameters;
    }
}