﻿// <copyright file="PlayerOnlineStateArguments.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.ServerClients;

/// <summary>
/// Arguments for the online state change of a player.
/// </summary>
public record PlayerOnlineStateArguments(Guid CharacterId, string CharacterName, byte ServerId, uint GuildId = 0);