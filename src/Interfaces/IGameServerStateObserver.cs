﻿// <copyright file="IGameServerStateObserver.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Interfaces;

using System.Net;

/// <summary>
/// An interface for an object which observes the state of game servers.
/// </summary>
public interface IGameServerStateObserver
{
    /// <summary>
    /// Registers the game server, so that it can be accessed through the connect server.
    /// </summary>
    /// <param name="gameServer">The game server information.</param>
    /// <param name="publicEndPoint">The public end point.</param>
    void RegisterGameServer(ServerInfo gameServer, IPEndPoint publicEndPoint);

    /// <summary>
    /// Un-registers the game server from the observer.
    /// </summary>
    /// <param name="gameServerId">The game server identifier.</param>
    void UnregisterGameServer(ushort gameServerId);

    /// <summary>
    /// Is called when the number of <see cref="ServerInfo.CurrentConnections"/> changed for a server.
    /// </summary>
    /// <param name="serverId">The server id.</param>
    /// <param name="currentConnections">The number of current connections, <see cref="ServerInfo.CurrentConnections"/>.</param>
    void CurrentConnectionsChanged(ushort serverId, int currentConnections);
}