// <copyright file="AppearanceData.Generated.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

//------------------------------------------------------------------------------
// <auto-generated>
//     This source code was auto-generated by a roslyn code generator.
// </auto-generated>
//------------------------------------------------------------------------------

// ReSharper disable All

namespace MUnique.OpenMU.Persistence.BasicModel;

using MUnique.OpenMU.Persistence.Json;

/// <summary>
/// A plain implementation of <see cref="AppearanceData"/>.
/// </summary>
public partial class AppearanceData : MUnique.OpenMU.DataModel.Entities.AppearanceData, IIdentifiable, IConvertibleTo<AppearanceData>
{
    
    /// <summary>
    /// Gets or sets the identifier of this instance.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets the raw collection of <see cref="EquippedItems" />.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("equippedItems")]
    public ICollection<ItemAppearance> RawEquippedItems { get; } = new List<ItemAppearance>();
    
    /// <inheritdoc/>
    [System.Text.Json.Serialization.JsonIgnore]
    public override ICollection<MUnique.OpenMU.DataModel.Entities.ItemAppearance> EquippedItems
    {
        get => base.EquippedItems ??= new CollectionAdapter<MUnique.OpenMU.DataModel.Entities.ItemAppearance, ItemAppearance>(this.RawEquippedItems);
        protected set
        {
            this.EquippedItems.Clear();
            foreach (var item in value)
            {
                this.EquippedItems.Add(item);
            }
        }
    }

    /// <summary>
    /// Gets the raw object of <see cref="CharacterClass" />.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("characterClass")]
    public CharacterClass RawCharacterClass
    {
        get => base.CharacterClass as CharacterClass;
        set => base.CharacterClass = value;
    }

    /// <inheritdoc/>
    [System.Text.Json.Serialization.JsonIgnore]
    public override MUnique.OpenMU.DataModel.Configuration.CharacterClass CharacterClass
    {
        get => base.CharacterClass;
        set => base.CharacterClass = value;
    }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        var baseObject = obj as IIdentifiable;
        if (baseObject != null)
        {
            return baseObject.Id == this.Id;
        }

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    /// <inheritdoc/>
    public AppearanceData Convert() => this;
}
