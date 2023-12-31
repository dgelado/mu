﻿// <copyright file="GameServerRegistry.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.LoginServer.Host;

using System.Threading;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// The registry for game servers.
/// </summary>
/// <seealso cref="System.IDisposable" />
public sealed class GameServerRegistry : IDisposable
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(20);
    private readonly TimeSpan _newServerUptimeLimit = TimeSpan.FromSeconds(60);
    private readonly CancellationTokenSource _disposeCts = new();
    private readonly ILogger<GameServerRegistry> _logger;
    private readonly Dictionary<ushort, DateTime> _entries = new();
    private readonly AsyncLock _lock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GameServerRegistry"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public GameServerRegistry(ILogger<GameServerRegistry> logger)
    {
        this._logger = logger;

        async Task RunCleanupLoop()
        {
            try
            {
                await this.CleanupLoopAsync(this._disposeCts.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error in cleanup loop");
            }
        }

        _ = RunCleanupLoop();
    }

    /// <summary>
    /// Occurs when a game server was added to the registry.
    /// </summary>
    public event AsyncEventHandler<ushort>? GameServerAdded;

    /// <summary>
    /// Occurs when a new (=freshly started) game server was added to the registry.
    /// </summary>
    public event AsyncEventHandler<ushort>? NewGameServerAdded;

    /// <summary>
    /// Occurs when a game server was removed from the registry.
    /// </summary>
    public event AsyncEventHandler<ushort>? GameServerRemoved;

    /// <inheritdoc />
    public void Dispose()
    {
        this._disposeCts.Cancel();
        this._disposeCts.Dispose();
    }

    /// <summary>
    /// Updates the registration of the game server.
    /// </summary>
    /// <param name="gameServerId">The game server identifier.</param>
    /// <param name="upTime">The up-time of the server.</param>
    public async Task UpdateRegistrationAsync(ushort gameServerId, TimeSpan upTime)
    {
        using var l = await this._lock.LockAsync();
        var timestamp = DateTime.UtcNow;
        if (this._entries.TryAdd(gameServerId, timestamp))
        {
            if (upTime <= this._newServerUptimeLimit)
            {
                this.NewGameServerAdded?.SafeInvokeAsync(gameServerId);
            }
            else
            {
                this.GameServerAdded?.SafeInvokeAsync(gameServerId);
            }
        }
        else
        {
            this._entries[gameServerId] = timestamp;
        }
    }

    private async Task CleanupLoopAsync(CancellationToken cancellationToken)
    {
        var tempRemoved = new List<ushort>();
        while (!this._disposeCts.IsCancellationRequested)
        {
            await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            using var l = await this._lock.LockAsync();
            foreach (var serverId in this._entries.Keys)
            {
                var lastUpdate = this._entries[serverId];
                var diff = DateTime.UtcNow - lastUpdate;
                if (diff > this._timeout)
                {
                    this._logger.LogInformation("Difference of {0} higher than timeout for server {1}", diff, serverId);
                    this.GameServerRemoved?.SafeInvokeAsync(serverId);
                    tempRemoved.Add(serverId);
                }
            }

            foreach (var serverId in tempRemoved)
            {
                this._entries.Remove(serverId, out _);
            }
        }
    }
}